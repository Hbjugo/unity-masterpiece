using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.IO;

/**
 * The current status of the game
 * All data for the game should be stored here
 * 
 * 
 **/
public class GameStatus : MonoBehaviour {
	bool hasStarted = false;

	// Singleton object
	private void Awake() {
		GameStatus[] gss = FindObjectsOfType<GameStatus>();
		if (gss.Length > 1) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	// Start is called before the first frame update
	void Start() {
		Grid grid = FindObjectOfType<Grid>();

		FindObjectOfType<EquipmentBank>().Unlock("0000");
	}

	private void Update() {
		if (!hasStarted) {
			SaveState("");
			StartCoroutine("LoadWorld", "");

			hasStarted = true;
		}
	}

	/**
	 * Called at the beginning of a battle
	 * Just call the scene loader to change the scene
	 * input: battleCoords -> the coordinates of the player on the world, juste before he entered the battle
	 **/
	public void EnterBattle(string sceneName) {
		string eventName = sceneName.Replace(" ", "");
		SaveState(Char.ToLowerInvariant(eventName[0]) + eventName.Substring(1));
		SceneManager.LoadScene(sceneName);
	}

	// TODO handle differently the fact that the battle is won or not (should inform the event handler)
	public void BattleWon() {
		SceneManager.LoadScene("World");
		StartCoroutine("LoadWorld", "Won");
	}

	public void BattleLost() {
		SceneManager.LoadScene("World");
		StartCoroutine("LoadWorld", "Lost");
	}

	private void SaveState(string eventName) {
		Save save = new Save();

		save.currEvent = eventName;

		Vector3Int currCell = FindObjectOfType<PartyMap>().GetCurrCell();
		save.partyCellX = currCell.x;
		save.partyCellY = currCell.y;

		Place currPlace = FindObjectOfType<EventHandler>().GetPlace();
		save.currPlace = currPlace ? currPlace.GetID() : "";

		save.money = FindObjectOfType<Wallet>().GetMoney();

		QuestLog log = FindObjectOfType<QuestLog>();
		save.log = log.GetLog();
		save.pendingQuestID = log.getPendingQuestID();

		EquipmentBank equipBank = FindObjectOfType<EquipmentBank>();
		save.unlockedEquipment = equipBank.GetUnlockedEquipment();

		CharacterBank charBank = FindObjectOfType<CharacterBank>();
		save.waitingChars = charBank.GetWaitingChars();

		QuestBank questBank = FindObjectOfType<QuestBank>();
		save.sideActivated = questBank.GetSideActivated();

		PartyManager party = FindObjectOfType<PartyManager>();
		Character leader = party.GetLeader();
		save.party.Add(leader.GetName());
		save.charEquipments.Add(leader.GetEquipment().GetID());
		foreach (Character c in party.GetParty()) {
			save.party.Add(c.GetName());
			save.charEquipments.Add(c.GetEquipment().GetID());
		}
		

		foreach (Place p in FindObjectsOfType<Place>()) {
			save.placesObjQuests.Add(p.GetID(), p.GetObjQuest());
			save.placesRecQuests.Add(p.GetID(), p.GetRecQuest());
			save.activatedPlaces.Add(p.GetID(), p.IsActivated());
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
		bf.Serialize(file, save);
		file.Close();

		Debug.Log("file saved");
	}

	IEnumerator LoadWorld(string eventName) {
		while (SceneManager.GetActiveScene().name != "World")
			yield return null;

		if (SceneManager.GetActiveScene().name == "World") {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
			Save save = (Save)bf.Deserialize(file);
			file.Close();

			FindObjectOfType<PartyMap>().Load(save);

			FindObjectOfType<QuestLog>().Load(save);

			FindObjectOfType<EquipmentBank>().Load(save);

			FindObjectOfType<CharacterBank>().Load(save);

			FindObjectOfType<QuestBank>().Load(save);

			FindObjectOfType<PartyManager>().Load(save);

			FindObjectOfType<Wallet>().Load(save);

			foreach (Place p in FindObjectsOfType<Place>()) {
				p.Load(save);
				if (p.GetID() == save.currPlace)
					FindObjectOfType<EventHandler>().SetPlace(p);
			}

			if (eventName != "")
				FindObjectOfType<EventHandler>().ChangeEvent(save.currEvent + eventName);

			Debug.Log("file loaded");
		}
	}
}
