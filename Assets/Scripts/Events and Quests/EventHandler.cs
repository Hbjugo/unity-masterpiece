using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/**
 * This is the class that handles events.
 * By event, I mean the texts where players can make various choices (engaging a battle, recruiting a new companion for their party, accepting a new quest, so on)
 * This class is informed by the ClickableText method what new event the player is trying to get
 * 
 * This works as follows: 
 * An event ID is given to the ChangeEvent method
 * For now, the two unique keywords are "exit" and "quest"
 * if it is "exit", the EH exits the current place and gives back the player his mobility. Nothing else is made
 * if it is "quest", then it transmits the rest of the ID to the QL. The QL, after having processed it, eventually gives a new event ID, or let it be.
 * Then, the EH gives the event ID (eventually changed by the QL) to the current place, who will pick the text to be next showed. The EH also eventually changes the background image based on the event ID.
 * 
 **/
public class EventHandler : MonoBehaviour {
	[SerializeField] Image eventImage = null;
	[SerializeField] Image NPCImage = null;
	[SerializeField] TextMeshProUGUI eventText = null;
	[SerializeField] Text title = null;
	EventTexts texts = new EventTexts();

	PartyMap party;

	QuestLog log;
	Place currPlace;

	private void Start() {
		party = FindObjectOfType<PartyMap>();
		log = FindObjectOfType<QuestLog>();
	}

	public void ChangeEvent(string newEvent) {
		// exiting the current place/ event
		if (newEvent == "exit") {
			gameObject.SetActive(false);
			party.SetBusy(false);
			currPlace = null;
			// TODO eventually add an exit method to Place ?
			return;
		}


		// changes the background image
		party.SetBusy(true);
		gameObject.SetActive(true);


		Sprite newBackground = Resources.Load<Sprite>("Sprites/Events/" + currPlace.GetName() + "/" + newEvent);
		if (newBackground != null)
			eventImage.sprite = newBackground;

		Sprite newNPC = Resources.Load<Sprite>("Sprites/NPCs/" + currPlace.GetName() + "/" + newEvent);
		if (newNPC) {
			NPCImage.sprite = newNPC;
			NPCImage.color = new Color(1, 1, 1, 1);
		}
		else {
			NPCImage.sprite = null;
			NPCImage.color = new Color(1, 1, 1, 0);
		}


		// Case where the quest logger should handle the event
		if (newEvent.Length >= 5 && newEvent.Substring(0, 5) == "quest") {
			string questProcessed = log.processQuest(newEvent.Substring(5));
			if (questProcessed == "Error")
				Debug.LogError("Error during quest processing");
			if (questProcessed != "Good")
				newEvent = questProcessed;
		}


		eventText.text = currPlace.ProcessEvent(newEvent);
	}


	// Setters and Getters
	public void SetPlace(Place newPlace) {
		title.text = newPlace.GetName();
		currPlace = newPlace;
	}

	public Place GetPlace() {
		return currPlace;
	}
}
