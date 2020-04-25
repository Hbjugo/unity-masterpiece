using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleStatus : MonoBehaviour {
	[SerializeField] Player player;

	public Mover[] movers;
	int currMover;

	int playerCount;
	int enemyCount;

	const int HEIGHT = 7;
	const int WIDTH  = 3;

	GameStatus gs;

	// Start is called before the first frame update
	void Start() {
		gs = FindObjectOfType<GameStatus>();

		PlacePlayers();

		movers = FindObjectsOfType<Mover>();
		Shuffle(movers);
		currMover = 0;

		playerCount = FindObjectsOfType<Player>().Length;
		enemyCount = FindObjectsOfType<Enemy>().Length;
	}

	private void PlacePlayers() {
		Vector3Int pos = new Vector3Int(-WIDTH, -HEIGHT, 0);

		pos = PlaceChara(gs.GetLeader(), pos);
		foreach (Character chara in gs.GetParty()) {
			pos = PlaceChara(chara, pos);
		}
	}

	private Vector3Int PlaceChara(Character c, Vector3Int pos) {
		bool placed = false;
		Grid grid = FindObjectOfType<Grid>();
		while (!placed) {
			Player p = Instantiate(player);

			if (p.Initialize(pos, c))
				placed = true;
			else 
				Destroy(p.gameObject);

			int x = pos.x;
			int y = pos.y;

			x += 1;
			if (x >= WIDTH) {
				x = -WIDTH;
				y += 1;
			}

			pos = new Vector3Int(x, y, 0);
			
		}

		return pos;
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
		// TODO find a better way to do it, as of right now, the scene world is loaded every time we stop the battle scene
		if (enemyCount <= 0)
			gs.BattleWon();
	}
}
