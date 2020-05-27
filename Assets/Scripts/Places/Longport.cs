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
public class Longport : City {
	Character smith;
	Character mehdouche;

	protected override void Start() {
		base.Start();
		CharacterBank bank = FindObjectOfType<CharacterBank>();
		smith = bank.GetCharacter("Smith");
		mehdouche = bank.GetCharacter("Mehdouche");
		bank.AddWaitingChar(smith);
	}

	public override string ProcessEvent(string id) {
		if (id == "thiefBattle") {
			FindObjectOfType<GameStatus>().EnterBattle("Thief Battle");
			return "";
		}

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
		Wallet wallet = FindObjectOfType<Wallet>();
		EquipmentBank equipBank = FindObjectOfType<EquipmentBank>();
		CharacterBank charBank = FindObjectOfType<CharacterBank>();
		QuestBank questBank = FindObjectOfType<QuestBank>();
		PartyManager party = FindObjectOfType<PartyManager>();
		QuestLog log = FindObjectOfType<QuestLog>();

		switch (name) {
			case "0102TurnIn":
			case "2000Accomplished":
			case "2000TurnIn":
			case "Longport":
				string s = "You arrived in the great city of Longport. It is one of the main city of the Known World. From what you've gathered, it is run by merchants. They have the grip on the biggest port of the Longlake, and use it to assess their power on all the villages surrounding the lake. \n\n" +
					"The city may be all rich and powerful, but the people don't seem really happy here. They all go by quickly, not giving you or anyone of your group a single look. Coming from the main gate, you and your party begin to wander through the streets. \n\n" +
					"You stop a passer to ask him directions. He's a sailor, working for the Merchants' Guild. He seems busy, and asks you not to take too much of his time.\n\n" +
					"<color=#cc3300><link=\"inn\">Ask the direction to the inn</link></color> \n\n" +
					"<color=#cc3300><link=\"market\">Ask the direction to the local market place</link></color> \n\n";
				if (GetObjQuest().Contains("0102"))
					s += "<color=#cc3300><link=\"quest0102\">Ask the direction to the address where you are supposed to deliver the package.</link></color> \n\n";
				s += "<color=#cc3300><link=\"merchantsHQ\">Ask the direction to the Merchants' Guild Headquarters</link></color> \n\n" +
					"<color=#cc3300><link=\"exit\">Leave the city</link></color>";
				return s;

			case "market":
				return "The market is located next to the harbour, the busiest place of the city. Many of the seafarers trade their goods to the merchants, who can then sell them on the market.\n\n" +
					"You are overwhelmed by the noise of the market. Everywhere, people are screaming, trying to get the attention of people passing by and to get them to buy their products. As Longport has the biggest harbour of the region, the market is appropriately sized. You can't see the end of the market. \n\n" +
					"You give the rest of your party a place and an hour to meet, before beginning your stroll. You should be able to find everything an adventurer needs, here.\n\n" +
					(equipBank.GetUnlockedEquipment()[1] ? "<color=#858585>Buy an armor (health = 1, radius = 2) for 50 golds (you've already bought it)</color>" : "<color=#cc3300><link=\"buyEquipment0001\">Buy an armor (health = 1, radius = 2) for 50 golds</link></color>\n\n") +
					(GetObjQuest().Contains("2000") ? "<color=#cc3300><link=\"quest2000\">You begin to search for the pickpocket</link></color> \n\n" : "") +
					"<color=#cc3300><link=\"city\">Go back to the city</link></color>";

			case "buyEquipment0001":
				if (wallet.SpendMoney(50)) {
					equipBank.Unlock("0001");
					return "You successfully bought a new armor.\n\n" +
						"<color=#cc3300><link=\"market\">Go back to the market</link></color>";
				}
				return "You don't have enough money. Come back when you have more! \n\n" +
					"<color=#cc3300><link=\"market\">Go back to the market</link></color>";

			case "inn":
				return "You enter the inn. Inside, a few townmen are drinking beers, in silence. The atmosphere here is as gloomy as the rest of the city. People don't seem to be enjoying very much their drinks. All the conversations you overhear are about work. They all seem to be willing to go back to it as soon as they can.\n\n" +
					"You find an agent of the adventurers' Guild in a corner of the inn. In a city as big as this one, the Guild surely has jobs for your kind. " + (!questBank.IsSideQuestActivated("2000") ? "At his table is a woman, speaking to him. On her chest, a broach indicates that she works for the Merchants' Guild. When they are finished talking, she gives a file to the agent, greets him and leaves." : "") + "\n\n" +
					(charBank.IsCharWaiting(smith) ? "There are also a few adventurers sitting around the inn. One of them doesn't seem to be in a group. Maybe you could recruit him for your party.\n\n" : "") +
					(charBank.IsCharWaiting(mehdouche) ? "The inn is pretty big, so at first you don't recognize anyone else. But after a little while, you notice that Mehdouche, the pickpocket you stopped earlier, is here, drinking with sailors. Maybe he would still be willing to come with you in your adventures." : "") +
					"<color=#cc3300><link=\"questTriv\">Go see the agent</link></color> \n\n" +
					(!questBank.IsSideQuestActivated("2000") ? "<color=#cc3300><link=\"quest2000\">You ask the agent what the Merchants' messenger wanted.</link></color>\n\n" : "") +
					(charBank.IsCharWaiting(smith) ? "<color=#cc3300><link=\"smith\">Propose to the adventurer to join your party </link></color> \n\n" : "") +
					(charBank.IsCharWaiting(mehdouche) ? "<color=#cc3300><link=\"mehdouche\">Greet the pickpocket and ask him if he still wants to join your party </link></color> \n\n" : "") +
					"<color=#cc3300><link=\"city\">Go back to the city</link></color>";

			case "smith":
				return "You sit at the bar, next to the adventurer. After a quick chat, it seems like he is searching for a party to gain fame and gold. He hates his city, and is willing to give up his former life to get out of here. You look at him. \n\n" +
					"His name is Smith. He was a worker for the city's merchants. He got in a few fights in his life, but really he doesn't have any experience with battle and such (health = 1, speed = 1). He also didn't have much money spared, so he has no perticular equipment.\n\n" +
					"<color=#cc3300><link=\"recruitSmith\">You welcome him in your party.</link></color> \n\n" +
					"<color=#cc3300><link=\"inn\">You refuse his demand, and leave him be.</link></color>";
			case "recruitSmith":
				party.Recruit(smith);
				charBank.RemoveWaitingChar(smith);
				return "So it is decided, Smith is now a member of your party. He just asks you for a little bit of time, so he can gather his stuff, resign from his job and say his farewells to his friends.\n\n" +
					"<color=#cc3300><link=\"inn\">You go back to the inn.</link></color>";

			case "mehdouche":
				party.Recruit(mehdouche);
				charBank.RemoveWaitingChar(mehdouche);
				return "His eyes widen as you approach him. He leaves his companions and embraces you firmly. \"My savior ! My friend, I will never be able to thank you enough for what you did.\". He offers to join him for a drink or two.\n\n" +
					"When you finally make your proposal, he accepts without any hesitation. \"I'm still willing to go with you. Please, just give me enough time to gather my things and to say goodbye to my sister, and I'll be on my way with you!\"\n\n" +
					"<color=#cc3300><link=\"inn\">You accept his request and go back to the inn.</link></color>";

			case "merchantsHQ":
				return "You arrive at the Headquarter of the Merchants' Guild. It is the tallest building of the town, located just next to the port. There is a constant flux of people entering and leaving the place, running to some unattended business they have. You walk through the door. \n\n" +
					"Inside, a secretary asks what is it you are doing here.\n\n" +
					(GetRecQuest().Contains("2000") && log.GetLog()["2000"] ? "<color=#cc3300><link=\"quest2000\">You say you are here for the reward promised by the Guild for stopping the pickpocket</link></color>\n\n" : "") +
					"<color=#cc3300><link=\"city\">You inform her that you have no business here. She looks rather annoyed that you wasted three minutes of her life, and asks you to leave.</link></color>";

			case "2000AlreadyAccomplished":
				return "She looks at you with a bit of suspicion. Apparently, nobody in this city seems to trust your capacities. She asks you to wait here, then leaves through a door behind her desk. Three minutes later, she comes back, with the messenger you had seen with the Adventurers' agent back in the inn. \n\n" +
					"\"My name is Lona, I work for the Merchants' Guild. Is it true you've rid us of that thief who eluded all our guards ?\" " +
					(party.GetParty().Exists(chara => chara.GetName() == "Mehdouche") ? "She takes a long look at Mehdouche, as if she knew he was the one who had caused her Guild so much trouble. But she takes her attention back to you, without mentionning anything about your companion.\n\n" : "\n\n") +
					"\"Here is your reward. Our Guild is grateful for your services. I'm sure that if we ever need some adventurers help in the future, we will call for you.\"" +
					"<color=#cc3300><link=\"questTurnIn\">You take the reward and make your way back to the city.</link></color>";

			// Quests
			case "0102AlreadyGiven":
				return "You arrive at the address given by the agent. You knock at the door of a large mansion. There, a domestic opens the door. He looks at you, and asks you what business you have with his master.\n\n" +
					"<color=#cc3300><link=\"questAccomplished\">You explain why you've come and hand him the package</link></color>";
			case "0102Accomplished":
				return "The man takes the object, tells you to wait a minute and leaves. After a short moment, he comes back with your reward. \n\n" +
					"<color=#cc3300><link=\"questTurnIn\">You thank him and go back to the city</link></color>";

			



			case ("0001"):
				return "You quietly sit at the table the man occupies, and explain what brings you here. He gives you a quick look, and begins to search in his files. \n\n"
					+ "\"We've had some reports that a group of icabres has established their lair near the city. Alone, they pose no threats, but a group could harm townfolks and villagers alike. That's bad for the merchant's business, so they asked us to send someone. If you would wipe them for us, we would pay you 50 golds\" \n\n" +
					"<color=#cc3300><link=\"questAccepted\">Accept the mission</link></color> \n\n<color=#cc3300><link=\"inn\">Decline the mission</link></color>";
			case ("0001Accepted"):
				return "\"I will be waiting for you here. Please be quick about it. The Merchant's Guild is generous, but they don't like it when business takes too much time. You will find the lair south of here. Follow the south coast when you exit the city, you should find it pretty quickly.\" \n\n" +
					"<color=#cc3300><link=\"inn\">You greet the agent and go back to the inn</link></color>";
			case ("0002"):
				return "The agent looks at you. \n\n\" Do you like traveling ? A merchant from their Guild asked this package to be delivered in the city of Criss. It is located not really far away from here. You only have to take the high road to the east, until you come accross the river. There, cross the bridge and you will arrive to the great city of Criss. The client will pay you 25 gold for your efforts.\"\n\n" +
					"<color=#cc3300><link=\"questAccepted\">Accept the mission</link></color> \n\n<color=#cc3300><link=\"inn\">Decline the mission</link></color>";
			case ("0002Accepted"):
				return "\"The reward is given by the client, so you won't have to come back to me once you will have delivered the package. Here is the address where you should go, once you're in Criss. Good luck, be sure that our Guild will hear about your services. \"\n\n" +
					"<color=#cc3300><link=\"inn\">You greet the agent and go back to the inn</link></color>";
			case ("2000"):
				return "\"Oh, she came in to inform me a new job is up. Apparently, a pickpocket has made his way into our city. Normally, the Guild's guards would be the ones to take care of the problem, but it seems like the merchants' puppies are having trouble dealing with this one. So the Merchants' Guild gave us this bounty. 100 golds if you manage to find the thief and stop him for good.\"\n\n" +
					"<color=#cc3300><link=\"questAccepted\">Accept the mission</link></color> \n\n<color=#cc3300><link=\"inn\">Decline the mission</link></color>";
			case ("2000Accepted"):
				return "\"According to our clients, the thief acts in the market. I think you would have a good chance of catching him if you search for him there. But hey, proceeds as you want. I'm not the one being paid for stopping him. \n\n" +
					"Once you've dealt with him, go to the Merchants' Guild Headquarter. They will give you the reward. It is located between the port and the market, you shouldn't be able to miss it.\"\n\n" +
					"<color=#cc3300><link=\"inn\">You greet the agent and go back to the inn</link></color>";

			case ("TrivAlreadyAccepted"):
				return "The man looks at you with a bit of surprise in his eyes. \"You are already finished with the task I've given you ? That was rather quick.\" You responds that you are not, and that you still have to deal with his job. \n\n" +
					"\"Oh, and here I was, thinking that for once I had a good adventurer under my hand, that would be able to finish jobs more quickly than all these other shitty sellswords. But no, seems like you're not better than those.\" He gives you a last look full of disdain and goes back to his drink.\n\n" +
					"<color=#cc3300><link=\"inn\">You go back to your business</link></color>";
			case "TrivAccomplished":
				return "The man lifts his head as you arrive. \"So, are you finally finished with this job I gave you ? Or do you need even more time ?\" \n\n" +
					"<color=#cc3300><link=\"questTurnIn\">Show him proof of your exploits</link></color>";
			case "TrivTurnIn":
				return "The man looks like he didn't expect you to fulfill your job. \"Ok, very good. You have the Guild's gratitude. Here's your reward.\". \n\n" +
					"<color=#cc3300><link=\"questTriv\">You ask for another job</link></color>\n\n <color=#cc3300><link=\"inn\">You go back to your business</link></color>";



			// 2000 quest
			case ("2000AlreadyGiven"):
				return "You wander through the market. Keeping an eye on your purse, you begin to act carelessly, in order to bring the pickpocket to make a move. \n\n" +
					"After an hour or two of this, you notice that a suspicious individual is following you.\n\n" +
					"<color=#cc3300><link=\"confrontThief\">You wait for him to act, then confront him.</link></color>";
			case "confrontThief":
				return "The pickpocket begins to run. He clearly knows the streets better than you do, but your amazing physical condition allows you to outrun him enough to jump on him. \n\n" +
					"You both are out of breath. He does not look afraid at all. \"I don't do this for the sole sake of robbing people, you know. I have a sister and a child to feed. I won't let you bring me to the guards. I'd rather kill you here and now\". He gets two daggers out of his boots. It seems like he's not bluffing. \n\n" +
					"<color=#cc3300><link=\"thiefBattle\">You're clearly not going to let him do as he wants.</link></color>";
			case "thiefBattleWon":
				return "The thief is on the ground, at your mercy. His eyes are getting wet, just like his pants.\n\n" +
					"\"Please sir... I have a family! You can't kill me! My sister... Her child... They won't be able to survive without me! I will find a real job, I promise, but please, please, don't kill me !\"\n\n" +
					"<color=#cc3300><link=\"questAccomplished>\"No mercy for the scum\", you say, before dealing the final blow the him</link></color> \n\n<color=#cc3300><link=\"pickpocketted>\"Just get the fuck outta here before I change my mind\"\n\n</link></color>" +
					(wallet.GetMoney() >= 100 ? "<color=#cc3300><link=\"newFriend\">\"Here\", you say as you throw him a purse full of gold. \"This is the reward for your head. Don't make me regret this.\"</link></color>" : "");
			case "pickpocketted":
				wallet.LoseMoney(15);
				return "He thanks you greatly, swearing that he will never be seen doing bad deeds again. But you've hardly turned your back, that you realize your purse has disappeared. He must have gotten away with it. At least, you've damaged him enough that he won't be able to bother the merchants for a long time.\n\n" +
					"<color=#cc3300><link=\"questAccomplished>You make your way back to the city</link></color>";
			case "newFriend":
				wallet.SpendMoney(100);
				return "His eyes widen. His gaze alternates between you and the purse. \"I don't have enough words to thank you.\" If his eyes were wet, now they are flowing with tears. He jumps to your feet. \n\n\"Take me with you ! Please ! I know how to fight, and this way, I will never have to be a thief ever again !\" You look at him, considering his offer.\n\n" +
					"He is a thief with great speed (radius = 4), but can not sustain many hits (hp = 2).\n\n" +
					"<color=#cc3300><link=\"recruitThief>You accept his request</link></color> \n\n<color=#cc3300><link=\"mehdoucheWaiting>You refuse</link></color>";
			case "recruitThief":
				party.Recruit(mehdouche);
				return "He weeps his eyes, and give you a look full of gratitude. \"I will pack my things and say my goodbyes to my sister. I'm sure that thanks to you, she will never have to worry about money ever again !\"\n\n" +
					"<color=#cc3300><link=\"questAccomplished>You make your way back to the city</link></color>";
			case "mehdoucheWaiting":
				charBank.AddWaitingChar(mehdouche);
				return "He seems disappointed, but swears to you that he will find a legit job and never rob again. He adds that if you ever change your mind, you can find him at the inn.\n\n" +
					"<color=#cc3300><link=\"questAccomplished>You make your way back to the city</link></color>";


			default: return null;
		}
	}
}
