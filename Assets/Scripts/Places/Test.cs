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
public class Test : City {
	public override string ProcessEvent(string id) {

		string city = GetText(id);

		if (city != null)
			return city;

		if ((city = base.ProcessEvent(id)) != null)
			return city;

		if (id[0] == '0' && id.Length > 4)
			return ProcessEvent("Triv" + id.Substring(4));

		Debug.LogError("Text not found for " + id);
		return null;
	}

	public override string GetID() {
		return "00";
	}

	public string GetText(string name) {
		switch (name) {
			case "0102TurnIn":
			case "0003Accomplished":
			case "Test":
				string s = "You arrived in the great city of Test.The streets are empty, and all you can find is an inn. \n\n" +
					"<color=#cc3300><link=\"inn\">Go to the inn</link></color> \n\n" +
					"<color=#cc3300><link=\"market\">Go to the local market place</link></color> \n\n"; ;
				if (GetObjQuest().Contains("0102"))
					s += "<color=#cc3300><link=\"quest0102\">Deliver the package as requested</link></color> \n\n";
				if (GetObjQuest().Contains("0003"))
					s += "<color=#cc3300><link=\"quest0003\">You begin searching for the pickpocket</link></color> \n\n";
				s += "<color=#cc3300><link=\"exit\">Leave the city</link></color>";
				return s;

			case "inn":
				return "You enter the inn. Inside, a few townmen are drinking beers, in silence. You find an agent of the adventurers' Guild, who would surely be happy to give you a job. \n\n" +
					"There are also a few adventurers sitting around the inn. One of them doesn't seem to be in a group. Maybe you could recruit him for your party.\n\n" +
					"<color=#cc3300><link=\"questTriv\">Go see the agent</link></color> \n\n" +
					"<color=#cc3300><link=\"generateChar\">Propose to the adventurer to join your party </link></color> \n\n" +
					"<color=#cc3300><link=\"city\">Go back to the city</link></color>";

			case "0102AlreadyGiven":
				return "You arrive at the address given by the agent. You knock at the door of a large mansion. There, a domestic opens the door. He looks at you, and asks you what business you have with his master.\n\n" +
					"<color=#cc3300><link=\"questAccomplished\">You explain why you've come and hand him the package</link></color>";
			case "0102Accomplished":
				return "The man takes the object and leaves. After a short moment, he comes back with your award. \n\n" +
					"<color=#cc3300><link=\"questTurnIn\">You thank him and go back to the city</link></color>";

			default: return null;
		}
	}
}
