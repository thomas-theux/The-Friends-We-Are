using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingxStory : MonoBehaviour {

	public GameObject foodBlock;
	public GameObject foodCanvas;
	public GameObject statsDisplayerGO;

	public static List<int> reqFoodArr = new List<int>();
	public static float currentStreak = 0;
	public static float maxStreak = 0;

	private float waitMin = 1;
	private float waitMax = 2;
	private float speedIncrease = 0.1f;
	public static int departments = 8;

	private bool isAdding;

	private float startX = 20;
	private float startY = Screen.height / 4;
	private float blockDistance = 30;

	public static int[] foodCount = {0, 0};

	private bool statsSaved;


	private string showActive;


	private void Update() {
		if (StartLevelCountdown.startLevel) {
			if (!isAdding) {
				StartCoroutine(SpawnNewReq());
			}
		}
		
		if (LevelTimer.levelEnd && !statsSaved) {
			// Save the titles for the stats
			StatsHolder.transferTexts = new string[] {
				"P1 Food Collected",
				"P2 Food Collected",
				"Best Streak"
			};

			// Save the suffixes for the stats
			StatsHolder.transferSuffixes = new string[] {
				"",
				"",
				""
			};

			// Save the single values for the stats overview
			StatsHolder.transferValues = new float[] {
				foodCount[0],
				foodCount[1],
				maxStreak
			};

			// Calculate the percentage the friends score is increasing
			CalculatePercentages();

			// Enable StatsDisplayer script
			statsDisplayerGO.GetComponent<StatsDisplayer>().enabled = true;

			statsSaved = true;
		}
	}


	IEnumerator SpawnNewReq() {
		isAdding = true;

		// Random waiting time till next food requirement comes
		float wait = Random.Range(waitMin, waitMax);
		yield return new WaitForSeconds(wait);

		// Choose random department to add to the checklist
		int whatFood = Random.Range(0, departments);
		reqFoodArr.Add(whatFood);

		// Instantiate new food block
		GameObject newFoodBlock = Instantiate(foodBlock, new Vector2(startX, startY + (blockDistance * reqFoodArr.Count)), Quaternion.identity);
		newFoodBlock.transform.SetParent(foodCanvas.transform);
		newFoodBlock.transform.localScale = new Vector3(1, 1, 1);
		newFoodBlock.GetComponent<Image>().color = SpawnTriggers.colors[whatFood];
		newFoodBlock.transform.GetChild(0).GetComponent<Text>().text = SpawnTriggers.departmentNames[whatFood];
		newFoodBlock.name = SpawnTriggers.departmentNames[whatFood];


		// Print current food array
		// for (int i = 0; i < reqFoodArr.Count; i++) {
		// 	int currentActive = reqFoodArr[i];
		// 	showActive += " " + currentActive;
		// }
		// print(showActive);
		// showActive = "";


		// Increase speed of blocks coming in
		if (waitMin > 0.2f) {
			waitMin -= speedIncrease;
			waitMax = waitMin + 1;
		}

		isAdding = false;
	}


	private void CalculatePercentages() {
		// Percentage for collected food
		for (int i = 0; i < 2; i++) {
			StatsHolder.transferPercentages[i] = foodCount[i] / 2;
		}

		// Percentage for max streak
		StatsHolder.transferPercentages[2] = maxStreak / 10;
	}
}
