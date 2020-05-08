using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBank : MonoBehaviour {
    public void CreateQuest(string ID) {
		switch (ID.Substring(2)) {
			case "01":
				CreateMonsterHuntInTest(ID.Substring(0, 2));
				break;
			case "02":
				CreateDeliveringQuestToOtherCity(ID.Substring(0, 2));
				break;
			case "03":
				CreateFindTheThiefQuest(ID.Substring(0, 2));
				break;
			default:  break;
		}
	}

	private void CreateMonsterHuntInTest(string placeID) {
		MonsterPlace monsterHunt = null;
		foreach (MonsterPlace p in FindObjectsOfType<MonsterPlace>()) if (p.GetCityID() == placeID) monsterHunt = p;
		if (!monsterHunt)
			Debug.LogError("Did not find the Monster place for the quest 0001");

		monsterHunt.Activate(true);
		ActivateQuest(placeID + "01", FindPlace(placeID), monsterHunt);
	}

	private void CreateDeliveringQuestToOtherCity(string placeID) {
		string otherID;
		int citiesCount = FindObjectsOfType<City>().Length;
		while ((otherID = UnityEngine.Random.Range(0, citiesCount).ToString("D2")) == placeID) ;
		Place objAndRec = FindPlace(otherID);
		ActivateQuest(placeID + "02", objAndRec, objAndRec);
	}

	private void CreateFindTheThiefQuest(string placeID) {
		Debug.Log(placeID);
		Place objAndRec = FindPlace(placeID);
		ActivateQuest(placeID + "03", objAndRec, objAndRec);
	}

	private Place FindPlace(string placeID) {
		Place[] places = FindObjectsOfType<Place>();
		foreach (Place p in places)
			if (p.GetID() == placeID) return p;

		Debug.LogError("No place of ID " + placeID + " found.");
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
