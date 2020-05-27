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
	Character rumblat;
	Character remuald;

	protected override void Start() {
		base.Start();
		CharacterBank bank = FindObjectOfType<CharacterBank>();
		rumblat = bank.GetCharacter("Rumblat");
		remuald = bank.GetCharacter("Remuald");
		bank.AddWaitingChar(remuald);
	}

	public override string ProcessEvent(string id) {
		if (id == "towerBattle") {
			FindObjectOfType<GameStatus>().EnterBattle("Tower Battle");
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
		return "01";
	}


	public string GetText(string name) {
		Wallet wallet = FindObjectOfType<Wallet>();
		EquipmentBank equipBank = FindObjectOfType<EquipmentBank>();
		CharacterBank charBank = FindObjectOfType<CharacterBank>();
		QuestBank questBank = FindObjectOfType<QuestBank>();
		PartyManager party = FindObjectOfType<PartyManager>();
		QuestLog log = FindObjectOfType<QuestLog>();

		switch (name) {
			case "0002TurnIn":
			case "Criss":
				return "You arrived in the great city of Criss. The city is located on the south bank of the Mountain's Flow, Longlake's main affluent. You've heard a lot about the city's ruler, the Duke. He is a powerful man, known for his sense of justice and his revulsion for war. \n\n" +
					"Criss is located in a relatively calm part of the region, away from Orc raiders and such. For this reason, the Duke has no real army. He mainly counts on the Adventurers' Guild, which has established its Headquarters here, to defend it if need be. But the Guild has a neutrality principle, and none knows for sure if it would call adventurers to arms for the sake of the Duke. \n\n" +
					"You enter the city via the main gate, nearby the Twolands bridge joining the two banks of the river. The city was built almost vertically, from the side of the river to the top of the hills overlooking it. The higher you are on the city, the lower your rank. The richest part of the city, along with the Duke's mansion, is located all the way down on the bank. There is also a little port, which enables trade with other local cities. \n\n" +
					"Although slavery is something authorized in Criss - as long as it is not humans that you subjugate -, the Duke has forbidden slave trade. Nonetheless, you see people from all cultures going to their business around you, be it dwarves, orcs or even beastmen. You hail one of them, to get some directions. The slave obediently nodes his head, interrupting what he was doing just for you. \n\n" +
					"<color=#cc3300><link=\"inn\">Ask the direction to the inn</link></color> \n\n" +
					"<color=#cc3300><link=\"market\">Ask the direction to the local market place</link></color> \n\n" +
					(GetObjQuest().Contains("0002") ? "<color=#cc3300><link=\"quest0102\">Ask the direction to the address where you are supposed to deliver the package.</link></color> \n\n" : "") +
					"<color=#cc3300><link=\"guild\">Ask the direction to the Adventurers' Guild Headquarters</link></color> \n\n" +
					"<color=#cc3300><link=\"exit\">Leave the city</link></color>";

			// Inn
			case "inn":
				bool remWaiting = charBank.IsCharWaiting(remuald);
				return "You enter the inn. A poet is declaiming verses. A few people form a circle around him, listening to his words. The rest of the folks drink cheerily, going from one table to another.\n\n" +
					(remWaiting ? "An agent of the adventurers' Guild is talking with an adventurer. He looks like he is trying to give him a new job, but the other seems to be only wanting to party. Surely he just came back from a successful mission and is willing to spend his newfound money.\n\n" : "An agent of the Adventurers' Guild is sitting alone, sipping a beer. The agents roaming the taverns usually can propose you easy jobs, which go with easy rewards.\n\n") +
					"If you talk to the agent, you will be sure to find a job." +  (remWaiting ? "You could also try to recruit the adventurer for your party.\n\n" : "\n\n") +
					"<color=#cc3300><link=\"questTriv\">Go see the agent</link></color> \n\n" +
					(remWaiting ? "<color=#cc3300><link=\"remuald\">Propose to the adventurer to join your party </link></color> \n\n" : "") +
					"<color=#cc3300><link=\"city\">Go back to the city</link></color>";

			case "remuald":
				// TODO come back when days will be implemented
				return "You go sit at the table, and take the seat next to the adventurer. He greets you cheerfully: \"Hey mate ! Do you want to drink with me ? I just came back from an important mission for the Guild. Today is payday !\" He offers you a drink. You both toast to his success. You then explain to him why you are here.\n\n" +
					"\"So you want me to join your group ? Honestly, I just want to celebrate my victories. But I must admit I miss the old days, when I was in a party such as yours. Maybe when I'll grow tired of this inn and I'll be bored enough to be willing to go back to the adventure, come back to me. I'm sure I'll be more than happy to go with you.\"\n\n" +
					"<color=#cc3300><link=\"inn\">Go back to the inn</link></color>";

			// Market
			case "market":
				return "You arrive on the market place. It is on the low side of the city, close from the river's bank. The market is not very big, but is specialized for adventurers coming from the Guild. No doubt you will find whatever you need for your adventures here.\n\n" +
					(!equipBank.GetUnlockedEquipment()[2] ? "<color=#cc3300><link=\"buyEquipment0002\">Buy an armor (health = 2, radius = 1) for 100 golds</link></color>\n\n" : "<color=#858585>Buy an armor (health = 2, radius = 1) for 100 golds (you already bought that)</color>\n\n") +
					"<color=#cc3300><link=\"city\">Go back to the city</link></color>";

			case "buyEquipment0001":
				if (wallet.SpendMoney(50)) {
					equipBank.Unlock("0001");
					return "You successfully bought a new armor.\n\n" +
						"<color=#cc3300><link=\"market\">Go back to the market</link></color>";
				}
				return "You don't have enough money. Come back when you have more! \n\n" +
					"<color=#cc3300><link=\"market\">Go back to the market</link></color>";

			case "0002AlreadyGiven":
				return "You arrive at the address given by the agent. You knock at the door of a large mansion, on the lower sides of the city. There, a domestic opens the door. He looks at you, and asks you what business you have with his master.\n\n" +
					"<color=#cc3300><link=\"questAccomplished\">You explain why you've come and hand him the package</link></color>";
			case "0002Accomplished":
				return "The man takes the object and leaves. After a short moment, he comes back with your award. \n\n" +
					"<color=#cc3300><link=\"questTurnIn\">You thank him and go back to the city</link></color>";

			// Guild
			case "guild":
				// TODO change it when rankings will be a thing
				return "On one of the lowest part of the city, just above the Duke's mansion, the Adventurers' Guild has built its headquarters. The Guild is present in all cities of Omennis, has agent proposing various jobs in every inn of the known world. But it is in this city that they have established their base, and that the Guild's leader is commanding all its agents.\n\n" +
					"The agents waiting in the inns generally propose easy jobs, not too complex and with rewards that don't pay too much. Such missions are for C-ranked adventurers, generally unknown to the Guild. But when you climb the Guild's ranks, it is here that you can find better jobs, requiring more skills and awarding better rewards.\n\n" +
					"The building is one of the largest mansions of the city. On the outside, it is pretty calm. You don't see many people entering the building; on the streets, there are mainly other humans going by. You push the headquarters doors and enter the building\n\n" +
					"You are surprised by the contrast with the vibe the Guild was giving when you were outside. Here, people are screaming, singing bawdy songs and laughing as loud as they can. There is only one large room, with multiple tables. Waiters and waitresses run off to each adventurer, holding multiple drinks all at once, trying not to spill any of it. Every now and then, a fight erupt, and everyone stops what he or she is doing to watch the show. \n\n" +
					(!questBank.IsSideQuestActivated("2001") ? "A young woman is arguing with an older man. You recognize him as an agent of the Guild by his broach. After a while, she leaves, being noticeably upset. \n\n" : "") +
					"You approach the back of the building, next to the bar. The agents behind it act both as bartenders and job givers. They are responsible for giving quests to adventurers that come here. Behind them, on the wall, the offers put by the Guild are pinned, so every one can see them. You could ask one of the agent to register for one of those. \n\n" +
					(!questBank.IsSideQuestActivated("2001") ? "<color=#cc3300><link=\"quest2001\">You follow the woman to see why she is bothered so much</link></color> \n\n" : "") +
					"<color=#cc3300><link=\"guildAgent\">You ask one of the agent for a quest</link></color> \n\n" +
					"<color=#cc3300><link=\"city\">You go back to the city</link></color>";

			case "guildAgent":
				return "<color=#cc3300><link=\"city\">Pas encore implémenté</link></color>";

			// Quests
			case ("0101"):
				return "You sit in front of the agent, and explain what brings you here. He stops what he is doing to help you. \n\n"
					+ "\"The Guild has put up a bounty for some wolves living in the mountains in the woods, on the other side of the river. They have been more and more threatening to the folks that do the walk between Longport and here, so the Guild has decided to hire adventurers like you to get rid of them. If you accept, you will be rewarded 50 golds.\" \n\n" +
					"<color=#cc3300><link=\"questAccepted\">Accept the mission</link></color> \n\n<color=#cc3300><link=\"inn\">Decline the mission</link></color>";
			case ("0101Accepted"):
				return "\"Perfect. You will find the beasts northeast of here. Take the bridge to get to the other side of the river. Then, you should search for them in the part of the forest south to the high road. They shouldn't be too far away from here. Come back to me with trophies to me to get you reward.\" \n\n" +
					"<color=#cc3300><link=\"inn\">You greet the agent and go back to the inn</link></color>";
			case ("0102"):
				return "The agent takes a look at you, while you sit down. \n\n\"Would you be interested in seeing the world ? A client of ours wants a package to be delivered to this address, in the city of Longport. It is located not really far away from here. You only have to take the bridge, and then follow the high road. You should eventually arrive to the great city of Longport. The client will pay you 25 gold for your efforts.\"\n\n" +
					"<color=#cc3300><link=\"questAccepted\">Accept the mission</link></color> \n\n<color=#cc3300><link=\"inn\">Decline the mission</link></color>";
			case ("0102Accepted"):
				return "\"The reward is given by the client, so you won't have to come back to me once you will have delivered the package. Here is the address where you should go, once you're in Longport. Good luck during your travels. \"\n\n" +
					"<color=#cc3300><link=\"inn\">You greet the agent and go back to the inn</link></color>";

				// TODO remove premade content and customize it for Criss
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

			case "2001":
				return "You leave the building. In front of it, the woman is frothing. You approach her, and ask her what's wrong. She clearly is surprised a stranger like you is addressing her, and taking interest in her business. \"I - I don't know if I should tell you. After all, you could be on of them\", she says, with a look full of mistrust. \n\n" +
					"You try to argue that you have no idea what she is talking about, but the more you talk, the more she seems convinced she should not trust you. \"Look, I don't know who you are, and I don't know why you are trying to get in my way. But if you know what's best for you, you should leave me alone, and stay with all these idiots in the Guild!\". She leaves, as infuriated as she was when you approached her first. \n\n" +
					"<color=#cc3300><link=\"followGirl\">Despite what she told you, you wait for a bit, and begin to follow her, twenty steps behind her.</link></color>\n\n" +
					"<color=#cc3300><link=\"guild\">You asked what was wrong, and she clearly stated she did not need your help. As such, you come back inside the Guild and forget that you ever met the young woman.</link></color>";

				// TODO give choices + add a way of having a temporary party
			case "followGirl":
				return "You make sure she can't see you from where she is. You follow her for quite a bit, going up the main avenue of the city and finally entering the middle side of Criss, where the commoners live. There, the young woman still walks a bit, until she arrives in front of a tower. \n\n" +
					"The presence of this building is kind of odd in a big city like this one. It is like a mage tower, but one would assume a structure like this one would be found in nature, away from other people. \n\n" +
					"Two persons are guarding its entry. They wear hoods, and you can't quite see their face. The girl starts talking to them, but you are too far from them to hear anything. On the other hand, the way the woman is waving her arms and the two men are looking at her clearly indicates that they are no friends one from another. \n\n" +
					"You try to get closer, to get more clearly what this is all about. There is no house next to the tower, so you have to be extra careful to sneak in. You lurk above a crate, not getting to get much closer. \"Clearly you are here to bother the old man. He asked us not to harm you, unless you tried something dumb. So please be a clever girl, and leave this place for good.\" The woman seems even more angry that she was back in the Guild.\n\n" +
					"\"I just want to speak with him ! I'm his apprentice for the Omens' sake, I should be able to go in this tower as I want !\" One of the hooded men hits the floor with his stick. \"You forsook this right when you went to the Guild and brought back a man with you. Don't play dumb with us, we know what you are up to. Now leave, or else.\" The lady turns her back. You see that she doesn't seem to understand what they are implying. But you do, so you hide your head behind the crate. \n\n" +
					"\"I am tired of your excuses\", you hear her say. She must not have seen you. \"This is my final warning, you will let me see Bazeruo, whether you want it or not.\" You lift your head above the crate. The girl and the two men have now taken a fighting stance. The confrontation seems unavoidable. \n\n" +
					"<color=#cc3300><link=\"towerBattle\">You come out of your hiding spot, and engage the men along her side.</link></color>";

			case "towerBattleWon":
				return "The two men have not yet hit the ground that the tower disappears. Where three minutes ago, there was a building, there is now a little fountain. \"Gone. They must have heard us fighting and teleported elsewhere in the city.\" The girl turns back to you. \n\n" +
					"\"By the Omens, who are you and what are you doing here ?\" You explain to her that her story caught your attention, and that you followed her to know more about her story. \"Who does that ? What kind of crazy person are you to follow strangers in a city like this ?\" \n\n" +
					"She ends up thanking you for your help. She doesn't say it, but you both know that she wouldn't have made it without your help. \"My name is Rumblat. I'm an apprentice to one of the most powerful mage of this part of the world, Bazeruo. He raised me and taught me everything I know. But... \" She marks a pause. She doesn't seem quite well. \" Recently, he is working tirelessly. I haven't seen him in weeks, and now the other apprentices even refuse that I see him. They say he doesn't want to see me. \" \n\n" +
					"She raises her head and looks up to you. \"But I don't believe them. Why would he not want to see me ? At first I thought he just wanted to focus on his works, so I didn't want to bother him. But now, the apprentices even fought me so I don't enter his quarters!\" A spark appears in her eyes. \"Listen, Bazeruo is a powerful man, but he also has powerful enemies. I think... I think that he has been captured by some rivals, or worse. I don't know how they made it, I don't know how they turned the apprentices against me, but they did it, I'm sure of it.\" \n\n " +
					"You ask her why the Guild is not helping her if it is so. \"They are not believing me. I've been trying to put up a contract for a few days now, but they say that I'm making up this stuff. They say they have contacted him and he is doing well. But that doesn't make any sense! Why would he answer them, but at the same time he could not see me ? No, the Guild must be corrupted, just like the apprentices !\"\n\n" +
					"You look at her, not sure if she's onto something or just completely mad. \n\n" +
					"<color=#cc3300><link=\"questAccepted\">You ask her how you could help.</link></color>\n\n" +
					"<color=#cc3300><link=\"city\">It was funny and all, but you inform her you have other, more important things, to do. You say your goodbyes and go back to the city.</link></color>\n\n";

			case "2001Accepted":
				return "Rumblat looks at you with gratitude. \"You are the first person to believe me. Thank you. I swear, you won't regret it. When we will deliver Bazeruo from whatever evil forces are keeping him, he will reward you greatly, I assure you !\"\n\n" +
					"\"Now, I don't think we can reach the tower easily anymore. They know I've tried to contact the Guild, they won't let me approach them upfrontly like before. There are three spots where the tower can teleport to in the city. If they see us, they will immediately teleport.\" She sits on the edge of the fountain and thinks for a moment. \n\n" +
					"\"I've been watching the tower, lately. Usually, they let me go inside of it, I just wasn't allowed to go in the mage's quarters. This is the first time they have refused to let me in. When I was in here, there was a lot of people that I didn't use to see in this place. Shady people. Like, they had hoods and strange marks on their eyes.\" She stops for a second, looks at the ground before raising her eyes to you. \"I know I'm making a lot of assumptions, but I think we may be dealing with a demon.\" \n\n" +
					"She maintains the eye contact. She seems sure of her, you suspect she has given this a lot of thought. \"Demons normally live in an other world. There is not much literature about them, and we don't know much about where they come from. But they can find ways to get in our world. Generally, they are summoned by ill-willed people. But they can't stay long in Omennis before they start starving. They need human lifeforce to stay here. So they feed on those who summoned them. They sap their life, and in exchange give them great powers. But at the end, the summoners always finish dead.\" \n\n" +
					"\"I don't know why these people would summon a demon in this city, I don't know what link this has with my master. But the marks these people had, around the eyes.\" She points the two men, lying unconscious near her. \"They had this exact same marks. I've read about it. And I think they are demoniac symbols, testifying that the demon gave them its power.\" \n\n" +
					"<color=#cc3300><link=\"magePlan\">All this is fine, but what are you supposed to do against a demon which has subjugated so many people ?</link></color>\n\n";

			case "magePlan":
				return "\"Look, as I said, I don't know much more than I already told you. But I haven't wasted my time since I started noticing all those things. The other night, I saw two apprentices heading up for the higher districts. Those are the poor parts of the city. Everyone knows you should not go in these neighbourhoods. The slaves are living there, along the poor people of the city. It's where most of the city's crimes happen. I'm sure they're headed there for a reason, and if we find them and follow them, we could learn more about all this!\" \n\n" +
					"You've agreed to help her, and, not being much more advanced than her, you don't have any other plan that could help her. So, you both meet up here when the night will come. During this time, she will establish the new location of the tower, so you will now where to begin your searches when you will find each other again. \n\n" +
					"<color=#cc3300><link=\"city\">You take your leave, and head off for the city</link></color>\n\n";

			default: return null;
		}
	}
}
