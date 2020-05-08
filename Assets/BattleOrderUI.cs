using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleOrderUI : MonoBehaviour {
	[SerializeField] Image head;

	public void ShowOrder(Mover[] order) {
		foreach (Transform child in transform)
			Destroy(child.gameObject);

		foreach (Mover m in order) {
			Image im = Instantiate(head, transform);
			im.sprite = m.GetCharacter().GetSprite();
		}
	}

	public void Next() {
		transform.GetChild(0).SetAsLastSibling();
	}
    
}
