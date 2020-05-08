using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wallet : MonoBehaviour {
	[SerializeField] TextMeshProUGUI moneyText = null;
	int money = 0;

	private void Start() {
		moneyText.text = money.ToString();
	}

	public void AddMoney(int amount) {
		money += amount;
		moneyText.text = money.ToString();
	}

	public bool SpendMoney(int amount) {
		if (amount > money)
			return false;

		money -= amount;
		moneyText.text = money.ToString();
		return true;
	}

	public int GetMoney() {
		return money;
	}

	public void Load(Save save) {
		money = save.money;
	}
}
