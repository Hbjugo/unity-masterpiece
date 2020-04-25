using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mover : MonoBehaviour {

	protected GameStatus gs;

	// map related
	protected Grid grid;
	Vector3 cellTarget;

    // Start is called before the first frame update
    protected virtual void Start() {
		grid = FindObjectOfType<Grid>();
		gs = FindObjectOfType<GameStatus>();
	}

    // Update is called once per frame
    protected virtual void Update() {
		// if the current target hasn't been reached, continue to move toward it
		if (IsMoving())
			transform.position = Vector3.MoveTowards(transform.position, cellTarget, 0.1f);
		
	}

	protected bool IsMoving() {
		return transform.position != cellTarget;
	}

	/**
	 * IsNextTo is used to check if the cell targeted by pos is a neighbour of the mover
	 * pos : the coordinates in the grid of the cell we want to check
	 * returns : true iff the cell is one of the six surrounding the player
	 * 
	 **/
	public static bool IsNextTo(Vector3Int pos, HashSet<Vector3Int> neighbours) {
		foreach (Vector3Int cell in neighbours) {
			if (cell == pos)
				return true;
		}
		return false;
	}


	/**
	 * ComputeNexts is used to compute all the neighbouring cells and to put them in the neighbours attribute
	 * It puts automatically all the surrounding cells, 
	 * independantly from the fact that those cells are valid cells for the player to cross or not
	 **/
	public static HashSet<Vector3Int> ComputeNeighbours(Vector3Int center, int rad) {
		HashSet<Vector3Int> neighbours = AuxComputeNeighbours(center, rad, new HashSet<Vector3Int>());
		neighbours.Remove(center);
		return neighbours;
	}

	private static HashSet<Vector3Int> AuxComputeNeighbours(Vector3Int center, int rad, HashSet<Vector3Int> alreadyComputed) {
		if (rad == 0)
			return new HashSet<Vector3Int>();
		int x = center.x;
		int y = center.y;

		HashSet<Vector3Int> neighbours = new HashSet<Vector3Int>();

		neighbours.Add(new Vector3Int(x - 1, y, 0));
		neighbours.Add(new Vector3Int(x + 1, y, 0));
		if (y % 2 == 0) {
			neighbours.Add(new Vector3Int(x - 1, y + 1, 0));
			neighbours.Add(new Vector3Int(x, y + 1, 0));
			neighbours.Add(new Vector3Int(x - 1, y - 1, 0));
			neighbours.Add(new Vector3Int(x, y - 1, 0));
		}
		else {
			neighbours.Add(new Vector3Int(x + 1, y + 1, 0));
			neighbours.Add(new Vector3Int(x, y + 1, 0));
			neighbours.Add(new Vector3Int(x + 1, y - 1, 0));
			neighbours.Add(new Vector3Int(x, y - 1, 0));
		}

		HashSet<Vector3Int> copyNeighbours1 = new HashSet<Vector3Int>(neighbours);
		HashSet<Vector3Int> copyNeighbours2 = new HashSet<Vector3Int>(neighbours);
		copyNeighbours1.ExceptWith(alreadyComputed);
		foreach (Vector3Int neighbour in copyNeighbours1) {
			neighbours.UnionWith(AuxComputeNeighbours(neighbour, rad - 1, copyNeighbours2));
		}

		return neighbours;
	}


	/**
	 * Check if the cell pointed by the vector 3 is a cell that the player can move to or not
	 * returns : true iff the cell is a valid one (i.e., is not contained by the invalidCells attribute)
	 * 
	 **/
	protected bool TileIsValid(Vector3Int cell, TileBase[] invalidTiles) {
		Tilemap map = grid.GetComponentInChildren<Tilemap>();
		TileBase tile = map.GetTile(cell);
		foreach (TileBase c in invalidTiles) {
			if (tile == c)
				return false;
		}
		return tile != null;
	}



	/**
	 * update the position of the player to nextPos
	 * the method only updates cellTarget and currCell, the Update method is the one that actually make the player move
	 * This method also compute the new neighbours by calling ComputeNexts
	 **/
	protected virtual bool Move(Vector3Int nextPos, TileBase[] invalidTiles) {
		if (TileIsValid(nextPos, invalidTiles)) {
			cellTarget = grid.CellToWorld(nextPos);
			return true;
		}
		return false;
	}



}
