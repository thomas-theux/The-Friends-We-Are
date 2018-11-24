using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour {

	public GameObject statsManager;
	public GameObject levelInterface;

	public Text levelTimeText;
	private float levelTime = 24;
	public static bool levelEnd;

	// private LevelFade levelFadeScript;


	private void Start() {
		levelTimeText = GameObject.Find("LevelTime").GetComponent<Text>();
		// levelFadeScript = GameObject.Find("LevelFader").GetComponent<LevelFade>();
	}


	private void Update() {
		if (StartLevelCountdown.startLevel) {
			CountdownLevelTime();
		}
	}


	private void CountdownLevelTime() {
		if (levelTime > 0) {
			levelTime -= Time.deltaTime;
			levelTimeText.text = Mathf.Ceil(levelTime) + "";
		} else {
			StartCoroutine(LastSeconds());
			StartLevelCountdown.startLevel = false;
		}
	}


	IEnumerator LastSeconds() {
		// Wait another second before going to the Story Overview screen
		yield return new WaitForSeconds(1);

		// End the level
		LevelEnd();
	}


	private void LevelEnd() {
		// Load Story Overview screen
		levelInterface.SetActive(false);
		statsManager.SetActive(true);
		levelEnd = true;
	}
	
}
