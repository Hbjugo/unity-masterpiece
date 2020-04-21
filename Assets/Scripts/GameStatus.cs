using UnityEngine;
using UnityEngine.SceneManagement;


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

	// Singleton object
	private void Awake() {
		GameStatus[] gss = FindObjectsOfType<GameStatus>();
		if (gss.Length > 1)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
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

	// Getters
	public Vector3Int GetCurrCell() {
		return currCell;
	}
}
