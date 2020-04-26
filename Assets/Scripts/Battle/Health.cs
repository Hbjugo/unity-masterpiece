using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
	float startingHp;
	float currHp;

    public void Initialize(float healthPoints) {
		startingHp = healthPoints;
	}

    public void GetHit(float damage) {
		currHp -= damage;
		if (currHp <= 0)
			Destroy(gameObject);
	}

	public float getCurrHp() {
		return currHp;
	}
}
