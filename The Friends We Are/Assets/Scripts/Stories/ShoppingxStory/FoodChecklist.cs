using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodChecklist : MonoBehaviour {

	public static List<int> reqFoodArr = new List<int>();

	private float waitMin = 2;
	private float waitMax = 4;
	public static int departments = 8;

	private bool isAdding;


	private void Update() {
		if (!isAdding) {
			StartCoroutine(SpawnNewReq());
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

		isAdding = false;
	}
}
