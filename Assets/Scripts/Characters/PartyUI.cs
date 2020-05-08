using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyUI : MonoBehaviour {
	[SerializeField] LayoutElement charContainer = null;
	[SerializeField] Transform content = null;
	PartyManager partyManager;
	PartyMap party;

	bool isActivated;

	private void Awake() {
		isActivated = false;
	}

	private void Start() {
		partyManager = FindObjectOfType<PartyManager>();
		party = FindObjectOfType<PartyMap>();

		gameObject.SetActive(isActivated);
	}

	public void Switch() {
		if (!isActivated) {
			foreach (Transform child in content)
				Destroy(child.gameObject);

			Character leader = partyManager.GetLeader();
			LayoutElement leaderContainer = Instantiate(charContainer, content);
			leaderContainer.GetComponentInChildren<TextMeshProUGUI>().text = "<color=red>Leader: </color>" + leader.GetName() + ": \n" +
				"Health: " + leader.GetHealth() + "\n" +
				"Speed: " + leader.GetRadius();
			foreach (Image i in leaderContainer.GetComponentsInChildren<Image>()) if (i.transform.parent != content.transform) {
					if (i.GetComponent<Button>()) {
						i.GetComponent<Image>().color = new Color(66, 70, 94);
						i.transform.localScale = new Vector3(1, 1, 1);
					}
					else
						i.sprite = Resources.Load<Sprite>("Sprites/NPCs/" + leader.GetName());
			}
			foreach (Character c in partyManager.GetParty()) {
				LayoutElement container = Instantiate(charContainer, content);
				container.GetComponentInChildren<TextMeshProUGUI>().text = c.GetName() + ": \n" +
					"Health: " + c.GetHealth() + "\n" +
					"Speed: " + c.GetRadius();
				container.GetComponent<CharacterFirer>().SetChara(c);
				foreach (Image i in container.GetComponentsInChildren<Image>()) if (i.transform.parent != content.transform) {
					if (i.GetComponent<Button>())
						i.transform.localScale = new Vector3(1, 1, 1);
					else
						i.sprite = Resources.Load<Sprite>("Sprites/NPCs/" + c.GetName());
				}

			}
			isActivated = true;
		}
		else
			isActivated = false;

		party.SetBusy(isActivated);
		gameObject.SetActive(isActivated);
	}
}
