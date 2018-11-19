using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodChecklist : MonoBehaviour {

	public GameObject foodBlock;
	public GameObject foodCanvas;

	public static List<int> reqFoodArr = new List<int>();

	private float waitMin = 4;
	private float waitMax = 6;
	private float speedIncrease = 0.1f;
	public static int departments = 8;

	private bool isAdding;

	private float startX = 20;
	private float startY = Screen.height / 4;
	private float blockDistance = 30;


	private string showActive;


	private void Update() {
		if (StartLevelCountdown.startLevel) {
			if (!isAdding) {
				StartCoroutine(SpawnNewReq());
			}
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
}
