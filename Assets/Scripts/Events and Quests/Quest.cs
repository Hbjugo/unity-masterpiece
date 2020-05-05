using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A class that represents a quest
 * Each quest has an ID, corresponding to its nature -> 
 * for now, it is as such
 * > 0XXX : trivial quest
 * > 1XXX : primary quest
 * > 2XXX : secondary quest
 * 
 * The quest can have sub-objectives (not yet implemented)
 * 
 * It has one receiver and can have multiple objectives. 
 **/
public class Quest {
	string ID;
	bool isAccomplished;

	public Quest(string ID, Place receiverPlace, params Place[] objectivesPlaces) {
		this.ID = ID;
		// announce to its receiver's place it should be able to handle this quest
		receiverPlace.AddQuestReceiver(ID);

		// if there is no objective, then the quest must only be turned in
		if (objectivesPlaces == null)
			isAccomplished = true;
		else {
			// announce to each of its objectives it should be able to handle this quest
			foreach (Place place in objectivesPlaces)
				place.AddQuestObjective(ID);

			isAccomplished = false;
		}
	}

	// An accomplished quest can be returned to its receiver
	public void AccomplishQuest(string ID) {
		isAccomplished = true;
	}

	// Getters and setters
	public bool IsAccomplished() {
		return isAccomplished;
	}

	public string GetID() {
		return ID;
	}
}
