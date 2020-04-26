using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestLogUI : MonoBehaviour {
	[SerializeField] TextMeshProUGUI text;
	QuestLog log;

	bool isActivated;

	private void Awake() {
		isActivated = false;
	}

	private void Start() {
		log = FindObjectOfType<QuestLog>();

		gameObject.SetActive(isActivated);
	}

	public void Switch() {
		if (!isActivated) {
			text.text = log.LogToString();
			isActivated = true;
		}
		else 
			isActivated = false;

		gameObject.SetActive(isActivated);
	}
}
