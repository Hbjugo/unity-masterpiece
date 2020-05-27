using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
	// at what point of the game the party is currently
	public string currEvent;

	// Party position on the map
	public int partyCellX;
	public int partyCellY;
	public string currPlace;

	// Current state of the wallet
	public int money;

	// Current state of the Quest log
	public Dictionary<string, bool> log = new Dictionary<string, bool>();
	public string pendingQuestID;

	// Current state of the progression (how much has been unlocked)
	public bool[] unlockedEquipment;
	public List<string> waitingChars = new List<string>();
	public bool[] sideActivated;

	// Current state of the party
	public List<string> party = new List<string>();
	public List<string> charEquipments = new List<string>();

	// Current state of the cities
	public Dictionary<string, List<string>> placesObjQuests = new Dictionary<string, List<string>>();
	public Dictionary<string, List<string>> placesRecQuests = new Dictionary<string, List<string>>();
	public Dictionary<string, bool> activatedPlaces = new Dictionary<string, bool>();

}
