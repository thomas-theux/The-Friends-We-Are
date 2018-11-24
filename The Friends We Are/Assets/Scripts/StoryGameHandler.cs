using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryGameHandler : MonoBehaviour {

	private GameObject gameManagerGO;

	// public static bool storyGameIsActive;
	// public bool activateStoryGame;


	private void Awake() {
		// storyGameIsActive = activateStoryGame;

		gameManagerGO = GameObject.Find("GameManager");

		LevelTimer.levelEnd = false;

		gameManagerGO.GetComponent<LevelTimer>().enabled = true;
		gameManagerGO.GetComponent<StartLevelCountdown>().enabled = true;
	}

}