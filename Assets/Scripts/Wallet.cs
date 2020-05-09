using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wallet : MonoBehaviour {
	[SerializeField] TextMeshProUGUI moneyText = null;
	int money = 100;

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

	public void LoseMoney(int amount) {
		money -= amount;
		if (money < 0)
			money = 0;

		moneyText.text = money.ToString();
	}

	public int GetMoney() {
		return money;
	}

	public void Load(Save save) {
		money = save.money;
		moneyText.text = money.ToString();
	}
}
