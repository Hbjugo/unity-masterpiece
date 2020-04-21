using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
	int currScene;

	private void Awake() {
		SceneLoader[] loaders = FindObjectsOfType<SceneLoader>();
		if (loaders.Length > 1)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	public void LoadScene(string name) {
		SceneManager.LoadScene(name);
		currScene = SceneManager.GetActiveScene().buildIndex;
	}

	public void LoadNext() {
		currScene = (currScene + 1) % SceneManager.sceneCountInBuildSettings;
		SceneManager.LoadScene(currScene);
	}
}
