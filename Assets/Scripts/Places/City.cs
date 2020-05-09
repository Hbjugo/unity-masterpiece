using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * A place representing a city
 * For now, there is some generic text for the city, but in the future there should be unique text for each unique city
 * The Enter() method describes how it should work for every city though
 */
public abstract class City : Place {

	Character pendingChar;


	public override void Enter() {
		EventHandler events = GetEventHandler();
		events.SetPlace(this);
		events.ChangeEvent("city");
	}

	public override string ProcessEvent(string id) {
		if (id == "city")
			return ProcessEvent(GetName());

		return null;
	}
}
