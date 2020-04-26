using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestLogUI : MonoBehaviour {
	[SerializeField] TextMeshProUGUI text;
	QuestLog log;
	PartyMap party;

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
			text.text = log.LogToString();
			isActivated = true;
		}
		else 
			isActivated = false;

		party.SetBusy(isActivated);
		gameObject.SetActive(isActivated);
	}
}
