using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

/**
 * Represents the quest log
 * It has a certain number of current quests, stored in the log attribute
 * This class is also responsible for generating trivial quests
 * 
 * 
 **/
public class QuestLog : MonoBehaviour {
	List<Quest> log; // TODO eventually add a way to limit the number of quests in the log
	QuestTexts texts;

	const int NB_TRIV_QUESTS = 1;

	// The last generated quest ID, before it is accepted, must be stored
	string pendingQuestID;
	
	// grid related stuff, to be able to generate quests
	Tilemap map;
	PartyMap party;

	EventHandler events;

	// The log isn't able to differentiate two same type triv quests (they have the same ID), so it stores the places that have already given a trivial quest 
	// TODO maybe a same place gould give multiple trivial quests ?
	Dictionary<Place, Quest> trivGiven;

	// Places for triv quest
	[SerializeField] MonsterPlace monsters;
	[SerializeField] TileBase[] validMonsters;


	// Singleton pattern
	private void Awake() {
		if (FindObjectsOfType<QuestLog>().Length > 1)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		log = new List<Quest>();
		trivGiven = new Dictionary<Place, Quest>();
		texts = new QuestTexts();
	}


	// Start is called before the first frame update
	void Start() {
		map = FindObjectOfType<Tilemap>();
		party = FindObjectOfType<PartyMap>();

		events = FindObjectOfType<EventHandler>();
    }

	/**
	 * This is the main method of this class. It is responsible to process what the event handler is telling him
	 * 
	 * The event handler, when he receives something like "quest...", knows that this event is for the QL, so he calls this method with the "..." of "quest..."
	 * Depending of what "..." was, this methods acts differently.
	 * 
	 * Mainly, there is five cases:
	 * "Triv" -> the event handler is asking to generate a new triv
	 * "XXXX" with each X a digit -> the event is asking to store this ID, corresponding to a unique quest (in this scenario, the quest should be a primary or secondary quest, not a trivial one)
	 * In both these events, we only store the ID of the quest, because we do not know yet what should be done whith it
	 * 
	 * "Accepted" means the player has accepted the quest whose ID is the one stored. As such, the QL nows creates the quest, and adds it to the log.
	 * "Accomplished" means the player has managed to accomplish the quest whose ID is the one stored. 
	 * "TurnIn" means the player has managed to turn in an accomplished quest to its receiver.
	 * 
	 * @return either a keyword ("Good" or "Error" so the EH knows everything has been done correctly (or not) or the ID of the quest it has stored (so the EH knows what triv has been generated, in perticular).
	 **/
	public string processQuest(string quest) {
		// If the key word is "accepted", that means the player accepted the lastly given quest -> we only have to add it to the quest log and put a marker on the map
		if (quest == "Accepted")
			return CreateQuest();

		// If the key word is "accomplished", that means the player accomplished the lastly stored quest -> we have to inform the place we are currently in that the quest is accomplished
		if (quest == "Accomplished")
			return AccomplishQuest(pendingQuestID);

		// If the key word is "TurnIn", that means the player has returned the lastly stored quest to its receiver -> we have to remove it from the log and inform its receiver's place that it has been received
		if (quest == "TurnIn")
			return TurnInQuest(pendingQuestID);

		// If the keyword is something else, it means that we should just store a quest ID
		if (quest == "Triv")
			return HandleTriv();

		
		pendingQuestID = quest;
		return pendingQuestID;
	}


	/**
	 * Handles the case where the given keyword was "Triv"
	 * A same place can not give multiple trivial quests at the same time.
	 * So this method first checks if there is an unresolved quest currently given by the place we are currently in.
	 * If so, it checks if the quest has already been accomplished or not. It informs the EH of that information.
	 * Else, it generates a new trivial quest ID.
	 **/
	string HandleTriv() {
		if (!trivGiven.ContainsKey(events.GetPlace())) {
			pendingQuestID = Random.Range(1, NB_TRIV_QUESTS + 1).ToString("D4");
			return pendingQuestID;
		}
		
		Quest currTriv;
		trivGiven.TryGetValue(events.GetPlace(), out currTriv);
		pendingQuestID = currTriv.GetID();
		if (currTriv.IsAccomplished())
			return "TrivAccomplished";

		return "TrivAlreadyGiven";
	}

	/**
	 * Once a new trivial quest has been accepted, we still ought to create it. 
	 * This methods check what kind of trivial quest should be created, based on the ID that has been already generated.
	 * TODO add more kind of trivial quests
	 **/
	string GenerateTrivQuest() {
		switch (pendingQuestID) {
			case "0001":
				return GenerateMonsterHunt();
			default:
				Debug.LogError("Generated a wrong quest");
				return "Error";
		}

	}

	/**
	 * For quests types
	 * 0000 -> empty quest
	 * 0xxx -> trivial quest
	 *  -> 0001 -> 0099 : battle quest
	 *  -> 0100 -> 0199 : fedex  quest (exemples)
	 **/

	// Monster Hunt trivial quest
	/**
	 * Generate a simple trivial quest, where the party has to defeat a group of monsters
	 * The method finds a cell where it can place the monsters, then creates the quest
	 **/
	string GenerateMonsterHunt() {
		bool generated = false;
		Vector3Int coords = new Vector3Int();
		Vector3Int partyCell = map.WorldToCell(party.GetCurrCell());

		int attempts = 0;
		// TODO check no overlap with other quest
		while (!generated && attempts < 10000) {
			int randX = Random.Range(-10, 11);
			int randY = Random.Range(-10, 11);
			coords = new Vector3Int(partyCell.x + randX, partyCell.y + randY, partyCell.z);
			bool isValid = false;
			foreach (TileBase tile in validMonsters) if (tile == map.GetTile(coords))
					isValid = true;

			generated = (randX != 0 || randY != 0) && isValid;
			attempts++;
		}
		if (attempts >= 10000)
			return "Error";

		MonsterPlace obj = Instantiate(monsters) as MonsterPlace;
		obj.transform.position = map.CellToWorld(coords);

		Quest quest = new Quest(pendingQuestID, texts.GetText(pendingQuestID), events.GetPlace(), obj);
		log.Add(quest);
		trivGiven.Add(events.GetPlace(), quest);
		return "Good";
	}


	public string LogToString() {
		StringBuilder sb = new StringBuilder();
		sb.Append("Current quests : \n\n\n");
		foreach (Quest q in log)
			sb.Append(texts.GetText(q.GetID()) + "\n\n");

		return sb.ToString();
	}




	// Auxiliary methods
	string CreateQuest() {
		if (pendingQuestID[0] == '0')
			return GenerateTrivQuest();

		// TODO when we will have unique quests : log.Add(new Quest(ID, events.GetCurrPlace(), ));
		return "Error";
	}

	// Announces to the current place that one of its quests has been accomplished
	string AccomplishQuest(string questID) {
		events.GetPlace().AccomplishQuest(questID);
		return "Good";
	}

	// removes the quest from the log and announces to the current place that this quest has been turned in
	// TODO make sure that there can be only one same type triv at the same time
	string TurnInQuest(string questID) {
		Quest questToRemove = events.GetPlace().TurnInQuest(questID);
		log.Remove(questToRemove);
		// if triv, remove it from the map
		if (questID[0] == '0')
			trivGiven.Remove(events.GetPlace());
		return "Good";
	}

	private class QuestTexts {
		Dictionary<string, string> texts;

		public QuestTexts() {
			texts = new Dictionary<string, string>();

			texts.Add("0001", "The adventurers' Guild has asked you to slay a couple of monsters. They are in a forest, not far from the city where you where given this task.");
		}

		public string GetText(string name) {
			string text = "";
			if (!texts.TryGetValue(name, out text))
				Debug.LogError("text not found for " + name);
			return text;
		}
	}
	
}
