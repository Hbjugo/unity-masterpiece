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
			isActivated = true;
		}
		else 
			isActivated = false;

		party.SetBusy(isActivated);
		gameObject.SetActive(isActivated);
	}

	private class QuestTexts {
		Dictionary<string, string> texts;

		public QuestTexts() {
			texts = new Dictionary<string, string>();

			texts.Add("0001", "The adventurers' Guild has asked you to slay a couple of monsters. They are in a forest, not far from the city of Test where you were given this task.");
			texts.Add("0101", "The adventurers' Guild has asked you to slay a couple of monsters. They are in a forest, not far from the city of Criss where you were given this task.");

			texts.Add("0002", "A client of the adventurers' Guild has asked for a package to be delivered in the city of Criss. It is southwest from the city of Test, where you were given this task.");
			texts.Add("0102", "A client of the adventurers' Guild has asked for a package to be delivered in the city of Test. It is northeast from the city of Criss, where you were given this task.");

			texts.Add("0003", "The merchants' Guild has hired you to get rid of a thief in the city of Test. Make sure he will never rob them again and they will reward you.");
			texts.Add("0103", "The merchants' Guild has hired you to get rid of a thief in the city of Criss. Make sure he will never rob them again and they will reward you.");
		}

		public string GetText(string name) {
			string text = "";
			if (!texts.TryGetValue(name, out text))
				Debug.LogError("text not found for " + name);
			return text;
		}
	}
}
