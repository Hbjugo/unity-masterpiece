using UnityEngine;
using System.Collections.Generic;


/**
 * The current status of the game
 * All data for the game should be stored here
 * TODO : add datas that are not here to be able to save the game and enter battles and stuff
 * 
 * 
 **/
public class GameStatus : MonoBehaviour {
	// SceneLoader
	SceneLoader sl;

	// In World
	Vector3Int currCell;

	// Party
	[SerializeField] List<Character> party;
	[SerializeField] Character partyLeader;
	const int MAX_CHAR_IN_PARTY = 5;

	// Singleton object
	private void Awake() {
		GameStatus[] gss = FindObjectsOfType<GameStatus>();
		if (gss.Length > 1)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		party = new List<Character>();
		partyLeader = new Character("Arthur", 4, 2);

		//TODO remove this
		RecruitChar(new Character("Smith", 2, 1));
		RecruitChar(new Character("Arnold", 5, 3));
	}

	// Start is called before the first frame update
	void Start() {
		sl = FindObjectOfType<SceneLoader>();
		currCell = new Vector3Int(0, 0, 0);
	}

	/**
	 * Called at the beginning of a battle
	 * Just call the scene loader to change the scene
	 * TODO : should save the state of the game, so when the player comes back, the world is restored
	 * input: battleCoords -> the coordinates of the player on the world, juste before he entered the battle
	 **/
	public void EnterBattle(Vector3Int battleCoords) {
		sl.LoadScene("Battle Scene");
		currCell = battleCoords;
	}


	// TODO handle differently the fact that the battle is won or not (should inform the event handler)
	public void BattleWon() {
		sl.LoadScene("World");
	}



	public void BattleLost() {
		Debug.Log("Perdu");
	}

	// Party 
	// TODO maybe add a party manager class ?
	public void RecruitChar(Character character) {
		if (party.Count < MAX_CHAR_IN_PARTY)
			party.Add(character);
	}

	public void FireChar(Character character) {
		party.Remove(character);
	}

	// Getters
	public Vector3Int GetCurrCell() {
		return currCell;
	}

	public List<Character> GetParty() {
		return party;
	}

	public Character GetLeader() {
		return partyLeader;
	}
}
