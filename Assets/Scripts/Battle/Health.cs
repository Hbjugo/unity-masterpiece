using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
	[SerializeField] float startingHp = 10f;
	float currHp;

    // Start is called before the first frame update
    void Start() {
		currHp = startingHp;
    }

    public void GetHit(float damage) {
		currHp -= damage;
		if (currHp <= 0)
			Destroy(gameObject);
	}
}
