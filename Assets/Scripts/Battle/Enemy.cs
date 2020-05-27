using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : Mover {
	Vector3Int currCell;

	[SerializeField] string charName;
	[SerializeField] int movementRad = 1;
	[SerializeField] int health = 1;
	[SerializeField] TileBase[] invalidTiles = new TileBase[0];

	float damage = 1f;
	Player currTarget;

	bool isAttacking = false;
	bool hasFinished = false;

	BattleStatus bs;
	Infobull infobull;

	// Start is called before the first frame update
	protected override void Start() {
		base.Start();

		bs = FindObjectOfType<BattleStatus>();

		infobull = FindObjectOfType<Infobull>();

		currCell = grid.WorldToCell(transform.position);

		Move(currCell, invalidTiles);

		GetComponent<Health>().Initialize(health);
	}

    // Update is called once per frame
    protected override void Update() {
		if (Input.GetMouseButtonDown(1) && MouseGrid() == currCell)
			infobull.Display(GetCharacter(), Input.mousePosition);

		if (!IsMoving()) {

			if (hasFinished) {
				hasFinished = false;
				bs.Next();
			}

			if (!bs.IsItMyTurn(this))
				return;


			if (isAttacking) {
				isAttacking = false;
				currTarget.GetComponent<Health>().GetHit(damage);
				Move(currCell, invalidTiles);
				hasFinished = true;
			}

			else {
				Player[] players = FindObjectsOfType<Player>();
				Vector3Int[] targets = new Vector3Int[players.Length];
				for (int i = 0; i < players.Length; ++i)
					targets[i] = players[i].GetCell();

				Vector3Int target = ShortestPath(targets);
				Player p = GetPlayer(target);
				if (p)
					Attack(p);
				else {
					currCell = target;
					Move(currCell, invalidTiles);
					hasFinished = true;
				}
			}
		}
		base.Update();
    }

	Vector3Int ShortestPath(Vector3Int[] targets) {
		if (targets.Length == 0)
			return currCell;

		Dictionary<Vector3Int, Vector3Int> explored = new Dictionary<Vector3Int, Vector3Int>();
		Queue<Vector3Int> queue = new Queue<Vector3Int>();

		explored.Add(currCell, currCell);
		queue.Enqueue(currCell);
		bool targetFound = false;
		Vector3Int targ = new Vector3Int();
		while (queue.Count != 0 && !targetFound) {
			Vector3Int currExpl = queue.Dequeue();
			foreach (Vector3Int n in ComputeNeighbours(currExpl, movementRad, invalidTiles)) {
				if (!explored.ContainsKey(n) && TileIsValid(n, invalidTiles) && !GetEnemy(n)) {
					queue.Enqueue(n);
					explored.Add(n, currExpl);
				}
				foreach (Vector3Int p in targets) {
					if (p == n) {
						targetFound = true;
						targ = n;
					}
				}
			}
		}

		List<Vector3Int> path = new List<Vector3Int>();
		path.Add(targ);
		while (targ != currCell) {
			explored.TryGetValue(targ, out targ);
			path.Add(targ);
		}
		path.Reverse();

		return path[1];
		
	}

	/**
	 * Check if there is currently an enemy in the given cell
	 * If there is one, set an enemy target
	 * returns: true iff the cell designated by the given coordinates currently contains an enemy
	 **/
	Player GetPlayer(Vector3Int cell) {
		Player[] players = FindObjectsOfType<Player>();
		foreach (Player p in players) {
			if (p.GetCell() == cell) {
				currTarget = p;
				return p;
			}
		}
		return null;
	}

	/**
	 * Check if there is currently an enemy in the given cell
	 * If there is one, set an enemy target
	 * returns: true iff the cell designated by the given coordinates currently contains an enemy
	 **/
	Enemy GetEnemy(Vector3Int cell) {
		Enemy[] enemies = FindObjectsOfType<Enemy>();
		foreach (Enemy e in enemies) {
			if (e.GetCell() == cell)
				return e;
		}
		return null;
	}

	void Attack(Player player) {
		isAttacking = true;
		Move(player.GetCell(), invalidTiles);
	}

	public Vector3Int GetCell() {
		return currCell;
	}
	
	public override Character GetCharacter() {
		return new Character(charName, health, movementRad, new Equipment("", 0, 0));
	}

	private void OnDestroy() {
		bs.EnemyDies();
	}
}
