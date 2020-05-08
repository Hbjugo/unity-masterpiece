using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A simple place for the Monster Hunt trivial quest
 * The place should work as follows:
 * The party arrives. There is a quick text, where the player can choose to engage the monsters or to go back
 * If he wants to fight them, a battle should take place (TODO)
 * After the battle, if the battle is won, the quest is accomplished and the player can continue its business
 **/
public class MonsterPlace : Place {
	// The name of the city who gave you the quest
	[SerializeField] string cityID = "";

	MonsterTexts texts;

	protected override void Start() {
		texts = new MonsterTexts(this);
		base.Start();
	}

	public override void Enter() {
		EventHandler events = GetEventHandler();
		events.SetPlace(this);
		events.ChangeEvent("monsterHunt");
	}

	public override string ProcessEvent(string id) {
		if (id == cityID + "01AlreadyGiven") {
			FindObjectOfType<GameStatus>().EnterBattle("Battle Scene");
			return "";
		}

		return texts.GetText(id);
	}

	// Once the quest has been accomplished, this place has no further use
	override public void AccomplishQuest(string ID) {
		base.AccomplishQuest(ID);
		Activate(false);
	}

	public string GetCityID() {
		return cityID;
	}

	public override string GetID() {
		return GetName() + cityID;
	}

	private class MonsterTexts {
		Dictionary<string, string> texts;

		public MonsterTexts(MonsterPlace place) {
			texts = new Dictionary<string, string>();
			
			texts.Add("monsterHunt", "You arrived in the forest. Quickly, you find the monsters you were looking for. \n\n " +
			"<color=#cc3300><link=\"quest" + place.cityID + "01\">Engage them</link></color> \n\n <color=#cc3300><link=\"exit\">Go back</link></color>");

			texts.Add("battleWon", "The beasts are slain. You should collect a few trophies, so you can prove your deeds to the Guild \n\n " +
				"<color=#cc3300><link=\"questAccomplished\">Collect some trophies</link></color>");

			texts.Add("battleLost", "The monster have won the battle. But the gods have been kind to you, because no one in your party has been hurt. Maybe you could try again to vainquish the monsters. \n\n" +
				"<color=#cc3300><link=\"quest" + place.cityID + "01\">Try again</link></color> \n\n <color=#cc3300><link=\"exit\">Flee</link></color>");

			texts.Add(place.cityID + "01Accomplished", "You can now go back to the city and collect your rewards. \n\n " +
				"<color=#cc3300><link=\"exit\">Go back</link></color>");
		}

		public string GetText(string name) {
			string text = "";
			if (!texts.TryGetValue(name, out text))
				Debug.LogError("text not found for " + name);
			return text;
		}
	}
}
