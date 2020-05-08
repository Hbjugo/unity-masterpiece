using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character {
	string name;
	Sprite battleSprite;

	int health;
	int battleRadius;

	Equipment equipment;

	public Character(string name, int health, int battleRadius, Equipment equipment) {
		this.name = name;
		this.battleSprite = Resources.Load<Sprite>("Sprites/Characters/" + name);
		this.health = health;
		this.battleRadius = battleRadius;
		this.equipment = equipment;
	}

	// getters
	public string GetName() {
		return name;
	}

	public int GetHealth() {
		return health + (equipment != null ? equipment.GetHealth() : 0);
	}

	public int GetBaseHealth() {
		return health;
	}

	public int GetRadius() {
		return battleRadius + (equipment != null ? equipment.GetRadius() : 0);
	}

	public int GetBaseRadius() {
		return battleRadius;
	}

	public Sprite GetSprite() {
		return battleSprite;
	}

	public Equipment GetEquipment() {
		return equipment;
	}

	public void Equip(Equipment equipment) {
		this.equipment = equipment;
	}

}
