using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyUI : MonoBehaviour {
	[SerializeField] LayoutElement charContainer = null;
	[SerializeField] Transform content = null;
	[SerializeField] LayoutElement equipContainer = null;
	[SerializeField] LayoutElement equipmentPlaceholder = null;
	PartyManager partyManager;
	EquipmentBank bank;
	PartyMap party;

	bool isActivated;
	int armoryIndex = int.MaxValue;

	private void Awake() {
		isActivated = false;
	}

	private void Start() {
		partyManager = FindObjectOfType<PartyManager>();
		party = FindObjectOfType<PartyMap>();
		bank = FindObjectOfType<EquipmentBank>();

		gameObject.SetActive(isActivated);
	}

	public void Switch() {
		if (!isActivated) {
			QuestLogUI log = FindObjectOfType<QuestLogUI>();
			if (log)
				log.Desactivate();

			foreach (Transform child in content)
				Destroy(child.gameObject);
			
			Character leader = partyManager.GetLeader();
			LayoutElement leaderContainer = Instantiate(charContainer, content);
			leaderContainer.GetComponentInChildren<TextMeshProUGUI>().text = "<color=red>Leader\n</color>" + leader.GetName() + ": \n" +
				"Health: " + leader.GetHealth() + "\n" +
				"Speed: " + leader.GetRadius();
			foreach (Image i in leaderContainer.GetComponentsInChildren<Image>()) if (i.transform.parent != content.transform) {
					if (i.tag == "Fire Button") {
						i.GetComponent<Image>().color = new Color(66, 70, 94);
						i.transform.localScale = new Vector3(1, 1, 1);
					}
					else if (i.tag == "Character Icon")
						i.sprite = Resources.Load<Sprite>("Sprites/NPCs/" + leader.GetName());
					else if (i.tag == "Equipment Icon") foreach (Image im in i.GetComponentsInChildren<Image>()) if (im.tag != "Equipment Icon")
								im.sprite = Resources.Load<Sprite>("Sprites/Equipments/" + leader.GetEquipment().GetID());
			}
			foreach (Character c in partyManager.GetParty()) {
				LayoutElement container = Instantiate(charContainer, content);
				container.GetComponentInChildren<TextMeshProUGUI>().text = c.GetName() + ": \n" +
					"Health: " + c.GetHealth() + "\n" +
					"Speed: " + c.GetRadius();
				container.GetComponent<CharacterFirer>().SetChara(c);
				foreach (Image i in container.GetComponentsInChildren<Image>()) {
						if (i.tag == "Fire Button")
							i.transform.localScale = new Vector3(1, 1, 1);
						else if (i.tag == "Character Icon")
							i.sprite = Resources.Load<Sprite>("Sprites/NPCs/" + c.GetName());
						else if (i.tag == "Equipment Icon") {
							foreach (Image im in i.GetComponentsInChildren<Image>()) if (im.tag != "Equipment Icon") {
									im.sprite = Resources.Load<Sprite>("Sprites/Equipments/" + c.GetEquipment().GetID());
								}
						}
					}

			}

			isActivated = true;
		}
		else
			isActivated = false;

		party.SetBusy(isActivated);
		gameObject.SetActive(isActivated);
	}

	public void Desactivate() {
		isActivated = false;

		party.SetBusy(false);
		gameObject.SetActive(false);
	}

	public void DisplayArmors(int charIndex) {
		foreach (Transform child in content) if (child.tag == "Armory Tab")
					Destroy(child.gameObject);
		
		LayoutElement container = Instantiate(equipContainer, content);
		for (int i = 0; i < EquipmentBank.NB_EQUIPMENT; ++i) {
			LayoutElement equipment = Instantiate(equipmentPlaceholder, container.transform);
			string ID = i.ToString("D4");
			foreach (Image im in equipment.GetComponentsInChildren<Image>()) if (im.tag != "Equipment Icon") {
					im.sprite = Resources.Load<Sprite>("Sprites/Equipments/" + ID);
					im.color = bank.GetUnlockedEquipment()[i] ? Color.white : Color.black;
				}
			equipment.name = ID;
			if (!bank.GetUnlockedEquipment()[i])
				equipment.GetComponent<Button>().enabled = false;
		}
		container.transform.SetSiblingIndex(charIndex+1);
		armoryIndex = armoryIndex < charIndex ? charIndex - 1 : charIndex;
		Debug.Log(armoryIndex);
	}

	public void SelectArmor(string armorID) {
		if (armoryIndex == 0)
			partyManager.GetLeader().Equip(bank.GetEquipment(armorID));
		else
			partyManager.GetParty()[armoryIndex - 1].Equip(bank.GetEquipment(armorID));

		armoryIndex = int.MaxValue;
		Switch();
		Switch();
	}
}
