using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour {
	const int MAX_CHAR = 5;
	List<Character> party;
	CharacterBank charBank;
	Character partyLeader;

	private void Awake() {
		PartyManager[] gss = FindObjectsOfType<PartyManager>();
		if (gss.Length > 1)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		charBank = FindObjectOfType<CharacterBank>();
		party = new List<Character>();
		partyLeader = charBank.GetCharacter("Arthur");
	}

	public void Load(Save save) {
		// resetting
		party = new List<Character>();
		partyLeader = null;
		EquipmentBank bank = FindObjectOfType<EquipmentBank>();
		partyLeader = new Character(save.charNames[0], save.charHealth[0], save.charRadius[0], bank.GetEquipment(save.charEquipments[0]));
		for (int i = 1; i < save.charNames.Count; ++i)
			Recruit(new Character(save.charNames[i], save.charHealth[i], save.charRadius[i], bank.GetEquipment(save.charEquipments[i])));
	}

	public void Recruit(Character chara) {
		if (party.Count < MAX_CHAR)
			party.Add(chara);

		// TODO handle case where the limit of the party size has been reached
	}

	public void Fire(Character chara) {
		charBank.AddWaitingChar(chara);
		party.Remove(chara);
	}
	public void Kill(Character chara) {
		party.Remove(chara);
	}

	public List<Character> GetParty() {
		return party;
	}

	public Character GetLeader() {
		return partyLeader;
	}
}
