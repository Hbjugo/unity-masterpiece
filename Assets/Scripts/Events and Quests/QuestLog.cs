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
	QuestTexts texts;
	QuestBank bank;

	const int NB_TRIV_QUESTS = 1;

	// The last generated quest ID, before it is accepted, must be stored
	string pendingQuestID;
	
	// grid related stuff, to be able to generate quests
	Tilemap map;
	PartyMap party;

	EventHandler events;

	// The log consists of a dictionnary. Each key is the ID of a quest, and the boolean says if the quest is already accomplished or not.
	Dictionary<string, bool> log = new Dictionary<string, bool>();
	// The log isn't able to differentiate two same type triv quests (they have the same ID), so it stores the places that have already given a trivial quest 
	// TODO maybe a same place gould give multiple trivial quests ?
	Dictionary<string, string> trivGiven = new Dictionary<string, string>();

	// Places for triv quest
	[SerializeField] MonsterPlace monsters = null;
	[SerializeField] TileBase[] validMonsters = null;


	// Start is called before the first frame update
	void Start() {
		map = FindObjectOfType<Tilemap>();
		party = FindObjectOfType<PartyMap>();

		events = FindObjectOfType<EventHandler>();
		bank = FindObjectOfType<QuestBank>();
    }

	public void Load(Save save) {
		log = save.log;
		trivGiven = save.trivGiven;
		pendingQuestID = save.pendingQuestID;

		texts = new QuestTexts();
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
		if (!log.ContainsKey(quest))
			return pendingQuestID;

		bool questAlreadyAccomplished;
		log.TryGetValue(pendingQuestID, out questAlreadyAccomplished);
		if (questAlreadyAccomplished)
			return quest + "Accomplished";

		return quest + "AlreadyGiven";
	}


	/**
	 * Handles the case where the given keyword was "Triv"
	 * A same place can not give multiple trivial quests at the same time.
	 * So this method first checks if there is an unresolved quest currently given by the place we are currently in.
	 * If so, it checks if the quest has already been accomplished or not. It informs the EH of that information.
	 * Else, it generates a new trivial quest ID.
	 **/
	string HandleTriv() {
		if (!trivGiven.ContainsKey(events.GetPlace().GetName())) {
			pendingQuestID = Random.Range(1, NB_TRIV_QUESTS + 1).ToString("D4");
			return pendingQuestID;
		}
		
		trivGiven.TryGetValue(events.GetPlace().GetName(), out pendingQuestID);
		bool isCurrTrivAccomplished;
		log.TryGetValue(pendingQuestID, out isCurrTrivAccomplished);
		if (isCurrTrivAccomplished)
			return "TrivAccomplished";

		return "TrivAlreadyGiven";
	}


	public string LogToString() {
		StringBuilder sb = new StringBuilder();
		sb.Append("Current quests : \n\n\n");
		foreach (string id in log.Keys)
			sb.Append(texts.GetText(id) + "\n\n");

		return sb.ToString();
	}




	// Auxiliary methods
	string CreateQuest() {
		bank.CreateQuest(pendingQuestID);
		log.Add(pendingQuestID, false);

		// Add the quest to the trivial given if it is a trivial quest
		if (pendingQuestID[0] == '0')
			trivGiven.Add(events.GetPlace().GetName(), pendingQuestID);

		return "Good";
	}

	// Announces to the current place that one of its quests has been accomplished
	string AccomplishQuest(string questID) {
		events.GetPlace().AccomplishQuest(questID);
		log[questID] = true;
		return "Good";
	}

	// removes the quest from the log and announces to the current place that this quest has been turned in
	// TODO make sure that there can be only one same type triv at the same time
	string TurnInQuest(string questID) {
		events.GetPlace().TurnInQuest(questID);
		log.Remove(questID);
		// if triv, remove it from the map
		if (questID[0] == '0')
			trivGiven.Remove(events.GetPlace().GetName());
		return "Good";
	}

	public Dictionary<string, bool> GetLog() {
		return log;
	}
	public Dictionary<string, string> GetTrivGiven() {
		return trivGiven;
	}
	public string getPendingQuestID() {
		return pendingQuestID;
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
