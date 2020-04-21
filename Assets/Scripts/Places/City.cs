using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * A place representing a city
 * For now, there is some generic text for the city, but in the future there should be unique text for each unique city
 * The Enter() method describes how it should work for every city though
 */
public class City : Place {
	CityTexts texts = new CityTexts();

	public override void Enter() {
		EventHandler events = GetEventHandler();
		Debug.Log(this);
		events.SetPlace(this);
		events.ChangeEvent(GetName());
	}

	public override string ProcessEvent(string id) {
		return texts.GetText(id);
	}

	private class CityTexts {
		Dictionary<string, string> texts;

		public CityTexts() {
			texts = new Dictionary<string, string>();

			texts.Add("test", "You arrived in the great city of Test.The streets are empty, and all you can find is an inn. \n\n " +
			"<color=#cc3300><link=\"inn\">Go to the inn</link></color> \n\n <color=#cc3300><link=\"exit\">Leave the city</link></color>");
			texts.Add("inn", "You enter the inn. Inside, a few townmen are drinking beers, in silence. You find an agent of the adventurer guild, who would be happy to give you a job. \n\n " +
				"<color=#cc3300><link=\"questTriv\">Ask the agent for a quest</link></color> \n\n <color=#cc3300><link=\"test\">Go back to the city</link></color>");

			// Quest texts
			texts.Add("0001", "You quietly sit at the table the man occupies. You only need to exchange a quick glare to make him understand why you are here. \n\n "
				+ "\"Some townfolks are harassed by monsters in nearby forests. If you would wipe them for us, we would pay you 50 silvers\" \n\n " +
				"<color=#cc3300><link=\"questAccepted\">Accept the mission</link></color> \n\n <color=#cc3300><link=\"inn\">Decline the mission</link></color>");


			texts.Add("questAccepted", "\"Come back to me when you're finished. I will be waiting here. Of course, the Guild we hear about your exploits. \n\n " +
				"<color=#cc3300><link=\"inn\">Go back to the inn</link></color>");

			texts.Add("TrivAccomplished", "The man lifts his head as you arrive. \"So, are you finished with the job I gave you ? \n\n " +
				"<color=#cc3300><link=\"questTurnIn\">Show him proof of your exploits</link></color>");
			texts.Add("TrivAlreadyGiven", "The man looks at you. \"Are you already finished ?\" he aks, with a bit of surprise in his voice. At the negative sign you give him with your head, he tells you to come back only when you will have accomplished the job he gave you. \n\n " +
				"<color=#cc3300><link=\"inn\">You go back to your business</link></color>");
			texts.Add("questTurnIn", "The man looks at you with a subtle smile. \"Very good. You have the Guild's gratitude. Here's your reward.\". \n\n " +
				"<color=#cc3300><link=\"inn\">You go back to your business</link></color> \n\n <color=#cc3300><link=\"questTriv\">You ask for another job</link></color>");
			// TODO add a way of having too much quests texts.Add("questTrivFull", "The man tells you you have too many accepted jobs for the Guild. You should try to clear one of them before accepting an other one. \n\n " +
			//	"<color=#cc3300><link=\"inn\">Go back to the inn</link></color>");
		}

		public string GetText(string name) {
			string text = "";
			if (!texts.TryGetValue(name, out text))
				Debug.LogError("text not found for " + name);
			return text;
		}
	}
}
