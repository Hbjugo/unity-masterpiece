using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFirer : MonoBehaviour
{
	Character chara = null;
	PartyManager party;
	PartyUI UI;

	private void Start() {
		party = FindObjectOfType<PartyManager>();
		UI = FindObjectOfType<PartyUI>();
	}

	public void SetChara(Character chara) {
		this.chara = chara;
	}

	public void FireChara() {
		if (chara == null)
			return;

		party.Fire(chara);
		UI.Switch();
		UI.Switch();
	}
}
