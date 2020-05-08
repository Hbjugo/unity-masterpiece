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
 * TODO : add datas that are not here to be able to save the game and enter battles and stuff
 * 
 * 
 **/
public class GameStatus : MonoBehaviour {
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

		SaveState();
		StartCoroutine("LoadWorld");
	}

	/**
	 * Called at the beginning of a battle
	 * Just call the scene loader to change the scene
	 * TODO : should save the state of the game, so when the player comes back, the world is restored
	 * input: battleCoords -> the coordinates of the player on the world, juste before he entered the battle
	 **/
	public void EnterBattle(string sceneName) {
		SaveState();
		SceneManager.LoadScene(sceneName);
	}

	// TODO handle differently the fact that the battle is won or not (should inform the event handler)
	public void BattleWon() {
		SceneManager.LoadScene("World");
		StartCoroutine("LoadWorld");
		StartCoroutine("InformEventHandler", "battleWon");
	}

	public void BattleLost() {
		SceneManager.LoadScene("World");
		StartCoroutine("LoadWorld");
		StartCoroutine("InformEventHandler", "battleLost");
	}

	IEnumerator InformEventHandler(string eventName) {
		while (SceneManager.GetActiveScene().name != "World")
			yield return null;

		if (SceneManager.GetActiveScene().name == "World")
			FindObjectOfType<EventHandler>().ChangeEvent(eventName);
	}

	private void SaveState() {
		Save save = new Save();

		Vector3Int currCell = FindObjectOfType<PartyMap>().GetCurrCell();
		save.partyCellX = currCell.x;
		save.partyCellY = currCell.y;

		Place currPlace = FindObjectOfType<EventHandler>().GetPlace();
		save.currPlace = currPlace ? currPlace.GetID() : "";

		save.money = FindObjectOfType<Wallet>().GetMoney();

		QuestLog log = FindObjectOfType<QuestLog>();
		save.log = log.GetLog();
		save.pendingQuestID = log.getPendingQuestID();

		PartyManager party = FindObjectOfType<PartyManager>();
		Character leader = party.GetLeader();
		save.charNames.Add(leader.GetName());
		save.charHealth.Add(leader.GetBaseHealth());
		save.charRadius.Add(leader.GetBaseRadius());
		save.charEquipments.Add(leader.GetEquipment().GetID());
		foreach (Character c in party.GetParty()) {
			save.charNames.Add(c.GetName());
			save.charHealth.Add(c.GetBaseHealth());
			save.charRadius.Add(c.GetBaseRadius());
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

	IEnumerator LoadWorld() {
		while (SceneManager.GetActiveScene().name != "World")
			yield return null;

		if (SceneManager.GetActiveScene().name == "World") {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
			Save save = (Save)bf.Deserialize(file);
			file.Close();

			FindObjectOfType<PartyMap>().Load(save);

			FindObjectOfType<QuestLog>().Load(save);

			FindObjectOfType<PartyManager>().Load(save);

			FindObjectOfType<Wallet>().Load(save);

			foreach (Place p in FindObjectsOfType<Place>()) {
				p.Load(save);
				if (p.GetID() == save.currPlace)
					FindObjectOfType<EventHandler>().SetPlace(p);
			}


			Debug.Log("file loaded");
		}
	}
}
