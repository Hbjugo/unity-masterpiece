using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * Represents the party on the map.
 * Handles primarily player movement
 **/
public class PartyMap : Mover {
	// Grid stuff
	[SerializeField] TileBase[] invalidTiles = new TileBase[0];
	Tilemap map;

	HashSet<Vector3Int> currNeighbours;
	int movementRad = 1;

	Vector3Int currCell;

	bool busy = false;


	// Start is called before the first frame update
	protected override void Start() {

		base.Start();

		// compute the first neighbours
		currNeighbours = ComputeNeighbours(currCell, movementRad);

		map = grid.GetComponentInChildren<Tilemap>();
	}

    // Update is called once per frame
    protected override void Update() {
		// In case the player is busy (ie, he is handling some events), he should not be able to move (so we don't update him)
		if (busy)
			return;

		// camera should always be centered on the player
		// TODO put boundaries
		Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);

		// if we're attacking, we have to go back to our cell -> once we've reached the enemy, we go back to our original cell
		if (!IsMoving()) {
			// get the cell pointed by the mouse
			Vector3Int mousePos = MouseGrid();

			// if the mouse is clicked on a neighbour cell
			if (Input.GetMouseButtonDown(0) && IsNextTo(mousePos, currNeighbours)) {	
				bool hasMoved = Move(mousePos, invalidTiles);
				UpdatePos(mousePos, hasMoved);
			}
		} 

		base.Update();
	}

	public void Load(Save save) {
		currCell = new Vector3Int(save.partyCellX, save.partyCellY, 0);

		transform.position = grid.CellToWorld(currCell);

		// move the player to the currCell, to be sure he is on the right spot at the start
		bool hasMoved = Move(currCell, invalidTiles);
		UpdatePos(currCell, hasMoved);
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
	 * Update the position of the player
	 * Primarily, it updates the currCell and the neighbours of the party
	 * Also, it colours the new neighbours and decolours the former ones
	 **/
	private void UpdatePos(Vector3Int nextPos, bool hasMoved) {
		if (hasMoved) {
			DecolorNeighbours();

			currCell = nextPos;
			currNeighbours = ComputeNeighbours(currCell, movementRad);

			ColorNeighbours();
		}
	}


	/**
	 * Auxiliary methods to (de)colour the neighbours of the party, so the player has a visual indication of where he is able to move
	 **/
	private void DecolorNeighbours() {
		foreach (Vector3Int c in currNeighbours) {
			map.SetColor(c, Color.white);
			map.SetTileFlags(c, TileFlags.LockAll);
		}
	}

	private void ColorNeighbours() {
		foreach (Vector3Int c in currNeighbours) {
			if (TileIsValid(c, invalidTiles)) {
				map.SetTileFlags(c, TileFlags.LockTransform);
				Color neighbColor = new Color(0.8f, 0.7f, 0.1f);
				map.SetColor(c, neighbColor);
			}
		}
	}


	// Setters and getters
	public void SetBusy(bool newVal) {
		busy = newVal;
	}

	public Vector3Int GetCurrCell() {
		return currCell;
	}

	public override Character GetCharacter() {
		throw new InvalidOperationException();
	}
}