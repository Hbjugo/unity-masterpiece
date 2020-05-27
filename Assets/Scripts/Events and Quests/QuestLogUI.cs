using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestLogUI : MonoBehaviour {
	[SerializeField] LayoutElement questContainer = null;
	[SerializeField] Transform content = null;
	QuestLog log;
	PartyMap party;
	QuestTexts texts = new QuestTexts();

	bool isActivated;

	private void Awake() {
		isActivated = false;
	}

	private void Start() {
		log = FindObjectOfType<QuestLog>();
		party = FindObjectOfType<PartyMap>();

		gameObject.SetActive(isActivated);
	}

	public void Switch() {
		if (!isActivated) {
			PartyUI party = FindObjectOfType<PartyUI>();
			if (party)
				party.Desactivate();

			foreach (Transform child in content)
				Destroy(child.gameObject);

			Dictionary<string, bool> quests = log.GetLog();
			if (quests.Count == 0) {
				LayoutElement container = Instantiate(questContainer, content);
				container.GetComponentInChildren<TextMeshProUGUI>().text = "No quest";
			}
			else foreach (string ID in quests.Keys) {
				LayoutElement container = Instantiate(questContainer, content);
				container.GetComponentInChildren<TextMeshProUGUI>().text = texts.GetText(ID);
			}

			foreach (Transform child in transform.parent)
				child.gameObject.SetActive(false);

			isActivated = true;

		}
		else 
			isActivated = false;

		party.SetBusy(isActivated);
		gameObject.SetActive(isActivated);
	}

	public void Desactivate() {
		isActivated = false;

		party.SetBusy(false);
		gameObject.SetActive(false);
	}

	private class QuestTexts {
		Dictionary<string, string> texts;

		public QuestTexts() {
			texts = new Dictionary<string, string>();

			texts.Add("0001", "The adventurers' Guild has asked you to slay a couple of monsters. They are in a forest, not far from the city of Test where you were given this task.");
			texts.Add("0101", "The adventurers' Guild has asked you to slay a couple of monsters. They are in a forest, not far from the city of Criss where you were given this task.");

			texts.Add("0002", "A client of the adventurers' Guild has asked for a package to be delivered in the city of Criss. It is southwest from the city of Test, where you were given this task.");
			texts.Add("0102", "A client of the adventurers' Guild has asked for a package to be delivered in the city of Test. It is northeast from the city of Criss, where you were given this task.");

			texts.Add("2000", "The Merchants' Guild has hired you to get rid of a thief in the city of Longport. Make sure he will never rob them again and they will reward you. \n\n" +
				"According to the Adventurers' Guild agent who gave you the job, you should look in the marketplace to find your target.");

			texts.Add("2001", "You met Rumblat in Criss. She is an apprentice to an old mage, whom, according to her, has been kidnapped by mysterious forces. You've told her you would help her find her master. \n\n" +
				"To reach this goal, you've decided to follow some of the mage's apprentices in the higher districts of Criss. You are to meet her up where you left her, in a place with a fountain in its center, where the tower once stood.");
		}

		public string GetText(string name) {
			string text = "";
			if (!texts.TryGetValue(name, out text))
				Debug.LogError("text not found for " + name);
			return text;
		}
	}
}
