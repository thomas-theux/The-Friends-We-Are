using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour {

	public Text levelTimeText;
	private float levelTime = 24;
	public static bool levelEnd;

	public KillAllPoints killAllPointsScript;


	private void Start() {
		levelTimeText = GameObject.Find("LevelTime").GetComponent<Text>();
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


	private void LevelEnd() {

		// Load Story Overview screen
		SceneManager.LoadScene("6 Summary");

	}


	IEnumerator LastSeconds() {
		// End the level
		levelEnd = true;

		// Destroy all remaining points
		killAllPointsScript.DestroyRemainingPoints();

		// Wait another 2 seconds before going to the Story Overview screen
		yield return new WaitForSeconds(2);

		LevelEnd();
	}
	
}
