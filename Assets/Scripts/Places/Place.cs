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

	PartyMap party;
	EventHandler events;

	List<Quest> objQuests;
	List<Quest> recQuests;

	bool hasEntered;

	private void Awake() {
		objQuests = new List<Quest>();
		recQuests = new List<Quest>();

		hasEntered = false;
	}

	// Start is called before the first frame update
	void Start() {
		events = FindObjectOfType<EventHandler>();
		Grid grid = FindObjectOfType<Grid>();

		// resets the position, so it is aligned with the grid
		transform.position = grid.CellToWorld(grid.WorldToCell(transform.position));
		party = FindObjectOfType<PartyMap>();
	}

    // Update is called once per frame
    void Update() {
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

	// Getters
	protected EventHandler GetEventHandler() {
		return events;
	}
	public string GetName() {
		return placeName;
	}


	// Related to quests
	public void AddQuestObjective(Quest quest) {
		objQuests.Add(quest);
	}

	public void AddQuestReceiver(Quest quest) {
		recQuests.Add(quest);
	}

	// Not sure of the use of this, I guess it is to be sure that not two objectives of the same kind of trivial quest happens in the same city
	public bool HasAlreadyObj(string objID) {
		foreach (Quest q in objQuests) {
			if (q.GetID() == objID)
				return true;
		}
		return false;
	}

	// Idem
	// TODO: if no use, remove those methods
	public bool HasAlreadyRec(string recID) {
		foreach (Quest q in recQuests) {
			if (q.GetID() == recID)
				return true;
		}
		return false;
	}

	// Multiple same type trivial quests could be logged in the QL at the same time -> ask the place to accomplish it, because the QL is not able to differentiate them
	public virtual void AccomplishQuest(string ID) {
		foreach (Quest q in objQuests) {
			if (q.GetID() == ID) {
				objQuests.Remove(q);
				q.AccomplishQuest(ID);
				return;
			}
		}
	}

	public Quest TurnInQuest(string ID) {
		foreach (Quest q in recQuests) {
			if (q.GetID() == ID) {
				recQuests.Remove(q);
				return q;
			}
		}
		Debug.LogError("Quest not found");
		return null;
	}
}
