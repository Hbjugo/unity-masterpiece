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
	// SceneLoader
	SceneLoader sl;

	// Party
	[SerializeField] List<Character> party;
	[SerializeField] Character partyLeader;
	const int MAX_CHAR_IN_PARTY = 5;


	// Singleton object
	private void Awake() {
		GameStatus[] gss = FindObjectsOfType<GameStatus>();
		if (gss.Length > 1) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);

		party = new List<Character>();
		partyLeader = new Character("Arthur", 4, 2);

		//TODO remove this
		RecruitChar(new Character("Smith", 2, 1));
		RecruitChar(new Character("Arnold", 5, 3));
		RecruitChar(new Character("Smith", 5, 2));
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
	public void EnterBattle() {
		SaveState();
		SceneManager.LoadScene("Battle Scene");
	}

	// TODO handle differently the fact that the battle is won or not (should inform the event handler)
	public void BattleWon() {
		SceneManager.LoadScene("World");
			StartCoroutine("LoadWorld");
			StartCoroutine("InformEventHandler", "battleWon");
		

		
	}

	public void BattleLost() {
		Debug.Log("Perdu");
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
		save.currPlace = currPlace ? currPlace.GetName() : "";

		QuestLog log = FindObjectOfType<QuestLog>();
		save.log = log.GetLog();
		save.trivGiven = log.GetTrivGiven();
		save.pendingQuestID = log.getPendingQuestID();

		foreach (Place p in FindObjectsOfType<Place>()) {
			save.placesObjQuests.Add(p.GetName(), p.GetObjQuest());
			save.placesRecQuests.Add(p.GetName(), p.GetRecQuest());
			save.activatedPlaces.Add(p.GetName(), p.IsActivated());
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

			foreach (Place p in FindObjectsOfType<Place>()) {
				p.Load(save);
				if (p.GetName() == save.currPlace)
					FindObjectOfType<EventHandler>().SetPlace(p);
			}


			Debug.Log("file loaded");
		}
	}

	// Party 
	// TODO maybe add a party manager class ?
	public void RecruitChar(Character character) {
		if (party.Count < MAX_CHAR_IN_PARTY)
			party.Add(character);
	}

	public void FireChar(Character character) {
		party.Remove(character);
	}

	public List<Character> GetParty() {
		return party;
	}

	public Character GetLeader() {
		return partyLeader;
	}

	public void GetPlaceCell(Place place, out Vector3 cell) {
		cell = new Vector3();
	}
}
