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
public class Criss : City {
	public override string ProcessEvent(string id) {

		string city = GetText(id);

		if (city != null)
			return city;

		if ((city = base.ProcessEvent(id)) != null)
			return city;

		if (id[0] == '0' && id.Length > 4)
			return ProcessEvent("Triv" + id.Substring(4));

		Debug.LogError("Text not found for " + name);
		return null;
	}

	public override string GetID() {
		return "01";
	}


	public string GetText(string name) {
		switch (name) {
			case "0002TurnIn":
			case "0103Accomplished":
			case "Criss":
				string s = "You arrived in the great city of Criss. Everything here is charming: there are cute houses on the riverside, the people passing by are all smiling, walking at a slow pace, as if they didn't have anything to care about in the world. It feels good, here. \n\n " +
					"<color=#cc3300><link=\"inn\">Go to the nearby inn</link></color> \n\n";
				if (GetObjQuest().Contains("0002"))
					s += "<color=#cc3300><link=\"quest0002\">Deliver the package as requested</link></color> \n\n";
				if (GetObjQuest().Contains("0103"))
					s += "<color=#cc3300><link=\"quest0103\">You begin searching for the pickpocket</link></color> \n\n";
				s += "<color=#cc3300><link=\"exit\">Leave the city</link></color>";
				return s;

			case "0002AlreadyGiven":
				return "You arrive at the address given by the agent. You knock at the door of a large mansion. There, a domestic opens the door. He looks at you, and asks you what business you have with his master.\n\n" +
					"<color=#cc3300><link=\"questAccomplished\">You explain why you've come and hand him the package</link></color>";
			case "0002Accomplished":
				return "The man takes the object and leaves. After a short moment, he comes back with your award. \n\n" +
					"<color=#cc3300><link=\"questTurnIn\">You thank him and go back to the city</link></color>";

			default: return null;
		}
	}
}
