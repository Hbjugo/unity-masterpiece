using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStatus : MonoBehaviour {


	public Mover[] movers;
	int currMover;

	int playerCount;
	int enemyCount;

	GameStatus gs;

	// Start is called before the first frame update
	void Start() {
		gs = FindObjectOfType<GameStatus>();

		movers = FindObjectsOfType<Mover>();
		Shuffle(movers);
		currMover = 0;

		playerCount = FindObjectsOfType<Player>().Length;
		enemyCount = FindObjectsOfType<Enemy>().Length;
	}

	/**
	 * Used to advance in the current order: each mover calls it once it has done an action
	 **/
	public void Next() {
		do {
			currMover = (currMover + 1) % movers.Length;
		} while (!movers[currMover]);
	}

	/**
	 * Called by movers to check if it's currently their turn to play
	 * input: mover -> the mover trying to know if it's their turn
	 **/
	public bool IsItMyTurn(Mover mover) {
		return movers[currMover] == mover;
	}


	/**
	 * Fisher-Yates shuffle algorithm
	 * input: array -> the array to be shuffled. The shuffling is done in place
	 **/
	public void Shuffle(Mover[] array) {
		int n = array.Length;
		while (n > 1) {
			n--;
			int i = Random.Range(0, n + 1);
			Mover temp = array[i];
			array[i] = array[n];
			array[n] = temp;
		}
	}

	/**
	 *  Called by a player when it is destroyed to decrement the players count
	 *  If the players count is below or equal to 0, the battle is lost
	 **/
	public void PlayerDies() {
		playerCount -= 1;
		if (playerCount <= 0)
			gs.BattleLost();
	}

	/**
	 *  Called by an enemy when it is destroyed to decrement the enemies count
	 *  If the enemies count is below or equal to 0, the battle is won
	 **/
	public void EnemyDies() {
		enemyCount -= 1;
		if (enemyCount <= 0)
			gs.BattleWon();
	}
}
