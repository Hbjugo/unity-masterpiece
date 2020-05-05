using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a place on the map
 * It could be a city, a bandit's lair, a dungeon, and so on
 * It can be the home to objectives of quests, or to receivers of it
 **/
public abstract class Place : MonoBehaviour {
	// the name of the place
	[SerializeField] string placeName;
	[SerializeField] bool isActivated;

	PartyMap party;
	EventHandler events;

	GameStatus gameStatus;

	List<string> objQuests;
	List<string> recQuests;
	bool hasGivenATriv;

	bool hasEntered;

	private void Awake() {
		objQuests = new List<string>();
		recQuests = new List<string>();

		hasGivenATriv = false;
	}

	private void Start() {
		events = FindObjectOfType<EventHandler>();

		party = FindObjectOfType<PartyMap>();

		gameStatus = FindObjectOfType<GameStatus>();
	}

	public void Load(Save save) {
		Grid grid = FindObjectOfType<Grid>();
		transform.position = grid.CellToWorld(grid.WorldToCell(transform.position));

		Vector3Int currCell = grid.WorldToCell(transform.position);
		hasEntered = save.partyCellX == currCell.x && save.partyCellY == currCell.y;

		save.placesObjQuests.TryGetValue(placeName, out objQuests);
		save.placesRecQuests.TryGetValue(placeName, out recQuests);

		save.activatedPlaces.TryGetValue(placeName, out isActivated);
		hasGivenATriv = save.trivGiven.ContainsKey(placeName);
		
		GetComponent<SpriteRenderer>().enabled = isActivated;
	}

    // Update is called once per frame
    void Update() {
		if (!isActivated)
			return;

		// Check if the player's party has entered the city (ie if its position is the same as the place's)
		if (party.transform.position == transform.position && !hasEntered) {
			Enter();
			hasEntered = true;
		}
		else if (party.transform.position != transform.position)
			hasEntered = false;
	}

	// Abstract methods to be redefined by subclasses
	public abstract void Enter();

	public abstract string ProcessEvent(string id);

	public void Activate(bool activation) {
		isActivated = activation;
		GetComponent<SpriteRenderer>().enabled = activation;
	}

	// Getters
	protected EventHandler GetEventHandler() {
		return events;
	}
	public string GetName() {
		return placeName;
	}
	protected GameStatus GetGameStatus() {
		return gameStatus;

	}
	protected bool HasEntered() {
		return hasEntered;
	}
	public bool IsActivated() {
		return isActivated;
	}

	public List<string> GetObjQuest() {
		return objQuests;
	}
	public List<string> GetRecQuest() {
		return recQuests;
	}
	public bool HasGivenATriv() {
		return hasGivenATriv;
	}

	public void SetHasGivenATriv(bool value) {
		hasGivenATriv = value;
	}


	// Related to quests
	public void AddQuestObjective(string quest) {
		objQuests.Add(quest);
	}

	public void AddQuestReceiver(string quest) {
		recQuests.Add(quest);
	}

	// Multiple same type trivial quests could be logged in the QL at the same time -> ask the place to accomplish it, because the QL is not able to differentiate them
	public virtual void AccomplishQuest(string ID) {
		objQuests.Remove(ID);
	}

	public virtual void TurnInQuest(string ID) {
		recQuests.Remove(ID);
	}
}
