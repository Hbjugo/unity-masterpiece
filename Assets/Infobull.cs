using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Infobull : MonoBehaviour {
	[SerializeField] Image avatar;
	[SerializeField] TextMeshProUGUI charName;
	[SerializeField] TextMeshProUGUI description;
	Grid grid;

	bool init = false;

	private void Start() {
		grid = FindObjectOfType<Grid>();
	}
	private void Update() {
		if (!init) {
			init = true;
			gameObject.SetActive(false);
		}
		if (Input.GetMouseButtonUp(1))
			gameObject.SetActive(false);
	}

	public void Display(Character chara, Vector3 mousePos) {
		Rect canvas = transform.parent.GetComponent<RectTransform>().rect;

		Vector3 newPos = new Vector3();
		Debug.Log(canvas.width / 2);
		if (mousePos.x < canvas.width / 2) {
			newPos.x = canvas.width / 4;
		}
		else {
			newPos.x = - canvas.width / 4;
		}

		if (mousePos.y < canvas.width / 2) {
			newPos.y = - canvas.height / 4;
		}
		else {
			newPos.y = canvas.height / 4;
		}

		transform.localPosition = newPos;
		charName.text = chara.GetName();
		description.text = "Health: " + chara.GetHealth() + "\nRadius: " + chara.GetRadius();
		avatar.sprite = Resources.Load<Sprite>("Sprites/NPCs/" + chara.GetName());
		gameObject.SetActive(true);
	}
}
