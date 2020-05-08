using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlace : Place {

	public override void Enter() {
		GetGameStatus().EnterBattle("Battle Scene");
	}

	public override string ProcessEvent(string id) {
		return "";
	}

	public override string GetID() {
		return null;
	}
}
