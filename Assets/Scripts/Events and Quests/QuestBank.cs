using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBank : MonoBehaviour {
	public const int NB_TRIV = 2;
	public const int NB_SIDE = 1;

	bool[] sideActivated= new bool[NB_SIDE];

    public void CreateQuest(string ID) {
		if (ID[0] == '0') switch (ID.Substring(2)) {
			case "01":
				CreateMonsterHunt(ID.Substring(0, 2));
				break;
			case "02":
				CreateDeliveringQuestToOtherCity(ID.Substring(0, 2));
				break;
			default:  break;
		}
		else switch (ID) {
				case "2000":
					CreateFindTheThiefQuest();
					break;
			}

	}

	public void GiveAward(string ID) {
		switch (ID.Substring(2)) {
			case "01":
				AwardMonsterHunt();
				break;
			case "02":
				AwardDelivering();
				break;
			case "03":
				AwardThief();
				break;
			default: break;
		}
	}

	// 0*00
	private void CreateMonsterHunt(string placeID) {
		MonsterPlace monsterHunt = null;
		foreach (MonsterPlace p in FindObjectsOfType<MonsterPlace>()) if (p.GetCityID() == placeID) monsterHunt = p;
		if (!monsterHunt)
			Debug.LogError("Did not find the Monster place for the quest 0001");

		monsterHunt.Activate(true);
		ActivateQuest(placeID + "01", FindPlace(placeID), monsterHunt);
	}

	// 0*01
	private void CreateDeliveringQuestToOtherCity(string placeID) {
		string otherID;
		switch (placeID) {
			case "00": otherID = "01";
				break;
			case "01": otherID = "00";
				break;
			default: otherID = "XX";
				break;
		}
		Place objAndRec = FindPlace(otherID);
		Debug.Log(objAndRec);
		ActivateQuest(placeID + "02", objAndRec, objAndRec);
	}

	// 2000
	private void CreateFindTheThiefQuest() {
		Place longport = FindPlace("00");
		ActivateQuest("2000", longport, longport);
	}

	private void AwardMonsterHunt() {
		FindObjectOfType<Wallet>().AddMoney(50);
	}

	private void AwardDelivering() {
		FindObjectOfType<Wallet>().AddMoney(25);

	}

	private void AwardThief() {
		FindObjectOfType<Wallet>().AddMoney(100);
	}

	private Place FindPlace(string placeID) {
		Place[] places = FindObjectsOfType<Place>();
		foreach (Place p in places)
			if (p.GetID() == placeID) return p;

		Debug.LogError("No place of ID " + placeID + " found.");
		return null;
	}

	private void ActivateQuest(string ID, Place receiverPlace, params Place[] objectivesPlaces) {
		if (ID[0] == '2')
			sideActivated[int.Parse(ID.Substring(1))] = true;

		// announce to its receiver's place it should be able to handle this quest
		receiverPlace.AddQuestReceiver(ID);

		
		// announce to each of its objectives it should be able to handle this quest
		foreach (Place place in objectivesPlaces)
			place.AddQuestObjective(ID);
		
	}

	public bool[] GetSideActivated() {
		return sideActivated;
	}
	public bool IsSideQuestActivated(string ID) {
		return sideActivated[int.Parse(ID.Substring(1))];
	}
	public void Load(Save save) {
		sideActivated = save.sideActivated;
	}
}
