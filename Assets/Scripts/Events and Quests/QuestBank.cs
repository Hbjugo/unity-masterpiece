using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBank : MonoBehaviour {
    public void CreateQuest(string ID) {
		switch (ID) {
			case "0001":
				CreateMonsterHuntInTest();
				break;
			default:  break;
		}
	}

	private void CreateMonsterHuntInTest() {
		MonsterPlace monsterHunt = null;
		foreach (MonsterPlace p in FindObjectsOfType<MonsterPlace>()) if (p.GetCityName() == "Test") monsterHunt = p;
		if (!monsterHunt)
			Debug.LogError("Did not find the Monster place for the quest 0001");

		monsterHunt.Activate(true);
		ActivateQuest("0001", FindPlace("Test"), monsterHunt);
	}

	private Place FindPlace(string placeName) {
		Place[] places = FindObjectsOfType<Place>();
		foreach (Place p in places)
			if (p.GetName() == placeName) return p;

		Debug.LogError("No place of name " + placeName + " found.");
		return null;
	}

	private void ActivateQuest(string ID, Place receiverPlace, params Place[] objectivesPlaces) {
		// announce to its receiver's place it should be able to handle this quest
		receiverPlace.AddQuestReceiver(ID);

		
		// announce to each of its objectives it should be able to handle this quest
		foreach (Place place in objectivesPlaces)
			place.AddQuestObjective(ID);
		
	}
}
