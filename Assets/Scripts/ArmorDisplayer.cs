using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorDisplayer : MonoBehaviour {
	PartyUI UI;

	private void Start() {
		UI = FindObjectOfType<PartyUI>();
	}

	public void DisplayArmors() {
		UI.DisplayArmors(transform.parent.GetSiblingIndex());
	}
}
