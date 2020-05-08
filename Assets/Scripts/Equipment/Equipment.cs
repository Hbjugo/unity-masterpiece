using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment {
	string ID;
	int health;
	int radius;

	public Equipment(string ID, int health, int radius) {
		this.ID = ID;
		this.health = health;
		this.radius = radius;
	}

	public string GetID() {
		return ID;
	}
	public int GetHealth() {
		return health;
	}
	public int GetRadius() {
		return radius;
	}
}
