using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : Mover {
	Character chara;

	bool isAttacking = false;
	bool hasFinished = false;
	bool initialized = false;
	Tilemap map;
	Vector3Int currCell;

	[SerializeField] TileBase[] invalidTiles = new TileBase[0];
	int movementRad = 1;

	HashSet<Vector3Int> currNeighbours;

	float damage = 1f;
	Enemy currTarget;

	BattleStatus bs;

	// equivalent to start, called by the BattleStatus
	public bool Initialize(Vector3Int cell, Character chara) {
		base.Start();

		if (TileIsValid(cell, invalidTiles)) {
			bs = FindObjectOfType<BattleStatus>();

			map = grid.GetComponentInChildren<Tilemap>();

			this.chara = chara;

			GetComponent<Health>().Initialize(chara.GetHealth());
			movementRad = chara.GetRadius();
			gameObject.GetComponent<SpriteRenderer>().sprite = chara.GetSprite();

			transform.position = grid.CellToWorld(cell);
			Move(cell, invalidTiles);
			currCell = cell;
			currNeighbours = ComputeNeighbours(cell, movementRad);

			initialized = true;

			return true;
		}

		return false;
	}

	// Update is called once per frame
	protected override void Update() {
		if (bs.IsItMyTurn(this) && !hasFinished)
			ColorNeighbours();
		else if (bs.IsItMyTurn(this))
			DecolorNeighbours();

		// if we're attacking, we have to go back to our cell -> once we've reached the enemy, we go back to our original cell
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
				// get the cell pointed by the mouse
				Vector3Int mousePos = MouseGrid();

				// if the mouse is clicked on a neighbour cell
				if (Input.GetMouseButtonDown(0) && IsNextTo(mousePos, currNeighbours)) {
					// if there's currently an enemy in this cell, attack it
					Enemy enemy = GetEnemy(mousePos);
					if (enemy)
						Attack(enemy);

					// else move in it
					else if (!GetPlayer(mousePos)) {
						bool hasMoved = Move(mousePos, invalidTiles);
						UpdatePos(mousePos, hasMoved);
						hasFinished = true;
					}

				}
				else if ((Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(1)) && mousePos == currCell) 
					GetComponent<CharInfobull>().Switch(); // TODO store the component
			}
		} 

		base.Update();
	}


	/**
	 * Gets the current cell pointed by the mouse
	 * returns: the coordinates of the cell the mouse is in
	 **/
	Vector3Int MouseGrid() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return grid.WorldToCell(mousePos);
	}



	/**
	 * Check if there is currently an enemy in the given cell
	 * If there is one, set an enemy target
	 * returns: true iff the cell designated by the given coordinates currently contains an enemy
	 **/
	Enemy GetEnemy(Vector3Int cell) {
		Enemy[] enemies = FindObjectsOfType<Enemy>();
		foreach (Enemy e in enemies) {
			if (e.GetCell() == cell) {
				currTarget = e;
				return e;
			}
		}
		return null;
	}

	/**
	 * Check if there is currently a player in the given cell
	 * If there is one, set an player target
	 * returns: true iff the cell designated by the given coordinates currently contains a player
	 **/
	Player GetPlayer(Vector3Int cell) {
		Player[] players = FindObjectsOfType<Player>();
		foreach (Player p in players) {
			if (p.GetCell() == cell)
				return p;
		}
		return null;
	}

	void Attack(Enemy enemy) {
		isAttacking = true;
		Move(enemy.GetCell(), invalidTiles);
	}

	private void UpdatePos(Vector3Int nextPos, bool hasMoved) {
		if (hasMoved) {
			DecolorNeighbours();

			currCell = nextPos;
			currNeighbours = ComputeNeighbours(currCell, movementRad);
		}
	}

	private void ColorNeighbours() {
		foreach (Vector3Int c in currNeighbours) {
			if (TileIsValid(c, invalidTiles) && !GetPlayer(c)) {
				map.SetTileFlags(c, TileFlags.LockTransform);
				map.SetColor(c, Color.yellow);
			}
		}
	}

	private void DecolorNeighbours() {
		foreach (Vector3Int c in currNeighbours) {
			map.SetColor(c, Color.white);
			map.SetTileFlags(c, TileFlags.LockAll);
		}
	}

	public Vector3Int GetCell() {
		return currCell;
	}

	public Character GetCharacter() {
		return chara;
	}

	private void OnDestroy() {
		if (initialized)
			bs.PlayerDies();
	}

	
}