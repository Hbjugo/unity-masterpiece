using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class CharInfobull : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI text = null;
	[SerializeField] GameObject infobull = null;

	bool isActivated = false;

	private void Start() {
		Character chara = GetComponent<Player>().GetCharacter();
		StringBuilder sb = new StringBuilder();
		sb.Append(chara.GetName() + ": \n");
		sb.Append("Health: " + chara.GetHealth() + "\n"); // TODO add current hp
		sb.Append("Movement: " + chara.GetRadius() + "\n");
		text.text = sb.ToString();

		infobull.SetActive(isActivated);
	}

	private void Update() {
		if (isActivated && Input.GetMouseButtonUp(1))
			Switch();
	}

	public void Switch() {
		infobull.SetActive(isActivated = !isActivated);
	}
}
