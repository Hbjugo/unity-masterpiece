using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Bank of texts. 
 * TODO : to be removed and put in each class that needs it (Place and its subclasses mainly)
 **/
public class Texts {
	Dictionary<string, string> nameToText;

	public Texts() {
		nameToText = new Dictionary<string, string>();

		// Town text
		nameToText.Add("city", "You arrived in the great city of {0}.The streets are empty, and all you can find is an inn. \n\n " +
			"<color=#cc3300><link=\"inn\">Go to the inn</link></color> \n\n <color=#cc3300><link=\"exit\">Leave the city</link></color>");
		nameToText.Add("placeInn", "You enter the inn. Inside, a few townmen are drinking beers, in silence. You find an agent of the adventurer guild, who would be happy to give you a job. \n\n " +
			"<color=#cc3300><link=\"questTriv\">Ask the agent for a quest</link></color> \n\n <color=#cc3300><link=\"city\">Go back to the city</link></color>");

		// Quest texts
		nameToText.Add("questTriv", "You quietly sit at the table the man occupies. You only need to exchange a quick glare to make him understand why you are here. \n\n {0} \n\n " +
			"<color=#cc3300><link=\"questAccepted\">Accept the mission</link></color> \n\n <color=#cc3300><link=\"inn\">Decline the mission</link></color>");



		nameToText.Add("questTrivAccepted", "\"Come back to me when you're finished. I will be waiting here. Of course, the Guild we hear about your exploits. \n\n " +
			"<color=#cc3300><link=\"inn\">Go back to the inn</link></color>");
		nameToText.Add("questTrivFull", "The man tells you you have too many accepted jobs for the Guild. You should try to clear one of them before accepting an other one. \n\n " +
			"<color=#cc3300><link=\"inn\">Go back to the inn</link></color>");

	}

	public string GetText(string name) {
		string text = "";
		if (!nameToText.TryGetValue(name, out text))
			Debug.LogError("text not found for " + name);
		return text;
	}

}
