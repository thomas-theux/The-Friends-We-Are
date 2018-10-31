using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayDisplayer : MonoBehaviour {

	private LevelFade levelFadeScript;

	public Text currentDayText;
	public static int currentDay;


	private void OnEnable() {
		// Load level fader for fade animation
		levelFadeScript = GameObject.Find("LevelFader").GetComponent<LevelFade>();

		// Count the days up
		currentDay++;

		// Display the appropriate day number
		CurrentDay();

		// Load next level after the wait time
		StartCoroutine(WaitTillNextLevel());
	}


	private void CurrentDay() {
		// Check if the current day is the last one or not
		if (currentDay < GameManager.tripDays) {
			if (currentDay < 10) {
				currentDayText.text = "Day 0" + currentDay;
			} else {
				currentDayText.text = "Day " + currentDay;
			}
		} else {
			currentDayText.text = "Last Day";
		}
	}


	IEnumerator WaitTillNextLevel() {
		yield return new WaitForSeconds(4.0f);

		// Check which day it is and load the appropriate scene
		if (currentDay == 1) {
			levelFadeScript.FadeToLevel("4 DrivexStory");
		} else {
			levelFadeScript.FadeToLevel("5 Announcement");
		}
	}

}
