using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentBank : MonoBehaviour {
	public const int NB_EQUIPMENT = 3;

	private bool[] unlockedEquipment = new bool[NB_EQUIPMENT];

	public void Load(Save save) {
		unlockedEquipment = save.unlockedEquipment;
	}

	public bool[] GetUnlockedEquipment() {
		return unlockedEquipment;

	}

	public void Unlock(string ID) {
		unlockedEquipment[int.Parse(ID)] = true;
	}

    public Equipment GetEquipment(string ID) {
		switch (ID) {
			case "0000":
				return new Equipment(ID, 0, 0);
			case "0001":
				return new Equipment(ID, 0, 1);
			case "0002":
				return new Equipment(ID, 1, 0);
		}
		return null;
	}
}
