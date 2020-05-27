using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBank : MonoBehaviour {
	List<string> waitingChars = new List<string>();

    public Character GetCharacter(string charName) {
		EquipmentBank bank = FindObjectOfType<EquipmentBank>();
		switch (charName) {
			case "Arthur": return new Character(charName, 1000, 3, bank.GetEquipment("0000"));
			case "Smith": return new Character(charName, 1, 1, bank.GetEquipment("0000"));
			case "Mehdouche": return new Character(charName, 2, 1, bank.GetEquipment("0001"));
			case "Arnold": return new Character(charName, 1, 2, bank.GetEquipment("0000"));
			case "Nethonal": return new Character(charName, 1, 1, bank.GetEquipment("0001"));
			case "Remuald": return new Character(charName, 1, 1, bank.GetEquipment("0000"));
			case "Rumblat": return new Character(charName, 2, 2, bank.GetEquipment("0000"));
			default: return new Character(charName, 1, 1, bank.GetEquipment("0000"));
		}
	}

	public List<string> GetWaitingChars() {
		return waitingChars;
	}
	public void Load(Save save) {
		waitingChars = save.waitingChars;
	}
	public void AddWaitingChar(Character chara) {
		waitingChars.Add(chara.GetName());
	}
	public void RemoveWaitingChar(Character chara) {
		waitingChars.Remove(chara.GetName());
	}
	public bool IsCharWaiting(Character chara) {
		return waitingChars.Contains(chara.GetName());
	}


}
