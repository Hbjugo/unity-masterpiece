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

		if (id == "battleThief") {
			FindObjectOfType<GameStatus>().EnterBattle("City Battle");
			return "";
		}
		if (id == "recruitThief")
			FindObjectOfType<PartyManager>().Recruit(new Character("Mehdouche", 2, 4, FindObjectOfType<EquipmentBank>().GetEquipment("0000")));

		if (id == "generateChar")
			 return GenerateChar();

		if (id == "recruitChar")
			return RecruitChar();

		pendingChar = null;
		return GetCityText(id);
	}

	private string GenerateChar() {
		string name = "Smith";
		int health = 3;
		int rad = 1;
		pendingChar = new Character(name, health, rad, FindObjectOfType<EquipmentBank>().GetEquipment("0000"));

		return String.Format(GetCityText("generateChar"), name, health, rad);
	}

	private string RecruitChar() {
		FindObjectOfType<PartyManager>().Recruit(pendingChar);
		string name = pendingChar.GetName();
			
		pendingChar = null;
		return String.Format(GetCityText("recruitChar"), name);
	}

	public string GetCityText(string name) {
		Wallet wallet = FindObjectOfType<Wallet>();
		EquipmentBank bank = FindObjectOfType<EquipmentBank>();
		switch (name) {
			

			case "market":
				return "You arrive on the market place. Here, numerous merchants are selling things of all sort, including all the material for adventurers like you. Surely you could find what you're looking for.\n\n" +
					(!bank.GetUnlockedEquipment()[1] ? "<color=#cc3300><link=\"buyEquipment0001\">Buy an armor (health = 1, radius = 2) for 50 golds</link></color>\n\n" : "") +
					"<color=#cc3300><link=\"city\">Go back to the city</link></color>";

			case "buyEquipment0001":
				if (wallet.SpendMoney(50)) {
					bank.Unlock("0001");
					return "You successfully bought a new armor.\n\n" +
						"<color=#cc3300><link=\"market\">Go back to the market</link></color>";
				}
				return "You don't have enough money. Come back when you have more! \n\n" +
					"<color=#cc3300><link=\"market\">Go back to the market</link></color>";

			case "TrivAccepted":
				return "\"Thanks for your help. Please do as fast as you can. And, of course, the Guild will hear about your exploits. \n\n" +
					"<color=#cc3300><link=\"inn\">Go back to the inn</link></color>";
			case "TrivAlreadyAccepted":
				return "The man looks at you. \"Are you already finished ?\" he aks, with a bit of surprise in his voice. At the negative sign you give him with your head, he tells you to come back only when you will have accomplished the job he gave you. \n\n" +
					"<color=#cc3300><link=\"inn\">You go back to your business</link></color>";
			case "TrivAccomplished":
				return "The man lifts his head as you arrive. \"So, are you finished with the job I gave you ? \n\n" +
					"<color=#cc3300><link=\"questTurnIn\">Show him proof of your exploits</link></color>";
			case "TrivTurnIn":
				return "The man looks at you with a subtle smile. \"Very good. You have the Guild's gratitude. Here's your reward.\". \n\n" +
					"<color=#cc3300><link=\"inn\">You go back to your business</link></color> \n\n <color=#cc3300><link=\"questTriv\">You ask for another job</link></color>";

			case "generateChar":
				return "You sit at the bar, next to an adventurer. After a quick chat, it seems like he is searching for a party to gain fame and gold. You look at him. \n\n" +
					"His name is {0}. He seems rather solid (hp = {1}) but not so quick (movement radius = {2}). \n\n" +
					"<color=#cc3300><link=\"recruitChar\">You welcome him in your party.</link></color> \n\n" +
					"<color=#cc3300><link=\"inn\">You refuse his demand, and leave him be.</link></color>";
			case "recruitChar":
				return "So it is decided, {0} is now a member of your party. \n\n" +
					"<color=#cc3300><link=\"inn\">You go back to the inn, with your newfound companion.</link></color>";

			case "confrontThief":
				return "The pickpocket begins to run. He clearly knows the streets better than you do, but your amazing physical condition allows you to outrun him enough to jump on him. \n\n" +
					"You both are out of breath. He does not look afraid at all. \"I don't do this for the sole sake of robbing people, you know. I have a sister and a child to feed. I won't let you bring me to the guards. I'd rather kill you here and now\". He gets two daggers out of his boots. It seems like he's not bluffing. \n\n" +
					"<color=#cc3300><link=\"battleThief\">You're clearly not going to let him do as he wants.</link></color>";
			case "battleWon":
				string s = "The thief is on the ground, at your mercy. His eyes are getting wet, just like his pants.\n\n" +
					"\"Please sir... I have a family! You can't kill me! My sister... Her child... They won't be able to survive without me! I will find a real job, I promise, but please, please, don't kill me !\"\n\n" +
					"<color=#cc3300><link=\"questAccomplished>\"No mercy for the scum\", you say, before dealing the final blow the him</link></color> \n\n<color=#cc3300><link=\"pickpocketted>\"Just get the fuck outta here before I change my mind\"\n\n</link></color>";
				if (wallet.GetMoney() > 40)
					s += "<color=#cc3300><link=\"newFriend\">\"Here\", you say as you throw him a purse full of gold. \"This is the reward for your head. Don't make me regret this.\"</link></color>";
				return s;
			case "pickpocketted":
				wallet.LoseMoney(15);
				return "He thanks you greatly, swearing that he will never be seen doing bad deeds again. But you've hardly turned your back, that you realize your purse has disappeared. He must have gotten away with it. At least, you've damaged him enough that he won't be able to bother the merchants for a long time.\n\n" +
					"<color=#cc3300><link=\"questAccomplished>You make your way back to the city</link></color>";
			case "newFriend":
				wallet.LoseMoney(40);
				return "His eyes widen. His gaze alternates between you and the purse. \"I don't have enough words to thank you.\" If his eyes were wet, now they are flowing with tears. He jumps to your feet. \n\n\"Take me with you ! Please ! I know how to fight, and this way, I will never have to be a thief ever again !\" You look at him, considering his offer.\n\n" +
					"He is a thief with great speed (radius = 4), but can not sustain many hits (hp = 2).\n\n" +
					"<color=#cc3300><link=\"recruitThief>You accept his request</link></color> \n\n<color=#cc3300><link=\"questAccomplished>You refuse, and go back to the city</link></color>";
			case "recruitThief":
				return "He weeps his eyes, and give you a look full of gratitude. \"I will pack my things and say my goodbyes to my sister. I'm sure that thanks to you, she will never have to worry about money ever again !\"\n\n" +
					"<color=#cc3300><link=\"questAccomplished>You make your way back to the city</link></color>";
		}

		// Quests

		if (GetID() + "01" == name)
			return "You quietly sit at the table the man occupies. You only need to exchange a quick glare to make him understand why you are here. \n\n"
				+ "\"Some townfolks are harassed by monsters in nearby forests. If you would wipe them for us, we would pay you 50 silvers\" \n\n" +
				"<color=#cc3300><link=\"questAccepted\">Accept the mission</link></color> \n\n<color=#cc3300><link=\"inn\">Decline the mission</link></color>";
		if (GetID() + "02" == name)
			return "The agent looks at you. \n\n\" Do you like traveling ? An important client asked this package to be delivered in a neighbour city. It is located not really far away from here. The client will pay you 25 gold for your efforts.\"\n\n" +
				"<color=#cc3300><link=\"questAccepted\">Accept the mission</link></color> \n\n<color=#cc3300><link=\"inn\">Decline the mission</link></color>";
		if (GetID() + "03" == name)
			return "\"The merchants' Guild has given us a job. Apparently, a pickpocket has made his way into our city. The guild wants you to find a way to stop this thief's misdeeds. They will award you 40 gold.\"\n\n" +
				"<color=#cc3300><link=\"questAccepted\">Accept the mission</link></color> \n\n <color=#cc3300><link=\"inn\">Decline the mission</link></color>";
		if (GetID() + "03AlreadyGiven" == name)
			return "You wander through the city's streets. Keeping an eye on your purse, you begin to act carelessly, in order to bring the pickpocket to make a move. \n\n" +
				"After an hour or two of this, you remark that a suspicious individual is following you.\n\n" +
				"<color=#cc3300><link=\"confrontThief\">You wait for him to act, then confront him.</link></color>";
		return null;
	}
}
