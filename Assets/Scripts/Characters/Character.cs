using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character {
	string name;

	Sprite battleSprite;
	int health;

	int battleRadius;

	public Character(string name, int health, int battleRadius) {
		this.name = name;
		this.battleSprite = Resources.Load<Sprite>("Sprites/Characters/" + name);
		this.health = health;
		this.battleRadius = battleRadius;
	}

	// getters
	public string GetName() {
		return name;
	}

	public int GetHealth() {
		return health;
	}

	public int GetRadius() {
		return battleRadius;

	}

	public Sprite GetSprite() {
		return battleSprite;
	}

}
