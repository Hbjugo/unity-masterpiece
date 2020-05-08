using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseArmor : MonoBehaviour {
	PartyUI UI;

	private void Start() {
		UI = FindObjectOfType<PartyUI>();
	}

	public void SelectArmor() {
		UI.SelectArmor(gameObject.name);
	}
}
