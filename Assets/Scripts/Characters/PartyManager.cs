using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour {
	const int MAX_CHAR = 5;
	List<Character> party;
	Character partyLeader;

	private void Awake() {
		PartyManager[] gss = FindObjectsOfType<PartyManager>();
		if (gss.Length > 1)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		party = new List<Character>();
		partyLeader = new Character("Arthur", 4, 2);

		//TODO remove this
		Recruit(new Character("Smith", 2, 1));
		Recruit(new Character("Arnold", 5, 3));
		Recruit(new Character("Smith", 5, 2));
	}

	public void Load(Save save) {
		// resetting
		party = new List<Character>();
		partyLeader = null;

		partyLeader = new Character(save.charNames[0], save.charHealth[0], save.charRadius[0]);
		for (int i = 1; i < save.charNames.Count; ++i)
			Recruit(new Character(save.charNames[i], save.charHealth[i], save.charRadius[i]));
	}

	public void Recruit(Character chara) {
		if (party.Count < MAX_CHAR)
			party.Add(chara);

		// TODO handle case where the limit of the party size has been reached
	}

	public void Fire(Character chara) {
		party.Remove(chara);
	}

	public List<Character> GetParty() {
		return party;
	}

	public Character GetLeader() {
		return partyLeader;
	}
}
