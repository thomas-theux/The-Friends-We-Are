using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	// private LevelFade levelFadeScript;

	public static bool isDay = false;

	public Text day;
	public Text hours;
	public Text minutes;

	public static float currentTime;
	public float currentHours;
	public float currentMinutes;

	public static float dayStartTime = 480;	//  8:00 Uhr
	private float dayEndTime = 1320;		// 22:00 Uhr

	private float multiplyTime = 14.0f;
	// Minutes in a day (awake) when using the following mutliplier
	// 02x = 7:00 min
	// 03x = 4:40 min
	// 04x = 3:30 min
	// 06x = 2:20 min
	// 07x = 2:00 min
	// 08x = 1:45 min
	// 14x = 1:00 min


	private void Awake() {
		DontDestroyOnLoad(this.gameObject);

		// Load level fader for fade animation
		// levelFadeScript = GameObject.Find("LevelFader").GetComponent<LevelFade>();

		currentTime = dayStartTime;

		if (DayDisplayer.currentDay < GameManager.tripDays) {
			if (DayDisplayer.currentDay < 10) {
				day.text = "Day 0" + DayDisplayer.currentDay;
			} else {
				day.text = "Day " + DayDisplayer.currentDay;
			}
		} else {
			day.text = "Last Day";
		}
		
	}

	private void Update() {
		if (isDay) {
			IncreaseTime();
			ShowTime();
			CheckForFeierabend();
		}
	}


	private void IncreaseTime() {
		currentTime += Time.deltaTime * multiplyTime;

		currentHours = currentTime / 60;
		currentMinutes = currentTime % 60;

		if (currentTime >= dayEndTime) {
			isDay = false;
			// levelFadeScript.FadeToLevel("5 Announcement");
			// currentTime = dayStartTime;
		}
	}


	private void ShowTime() {
		if (currentHours < 10) { hours.text = "0" + Mathf.Floor(currentHours); }
		else { hours.text = "" + Mathf.Floor(currentHours); }

		if (currentMinutes < 10) { minutes.text = "0" + Mathf.Floor(currentMinutes); }
		else { minutes.text = "" + Mathf.Floor(currentMinutes); }
	}


	private void CheckForFeierabend() {
		// Check if the day is over and end the day
	}

}
