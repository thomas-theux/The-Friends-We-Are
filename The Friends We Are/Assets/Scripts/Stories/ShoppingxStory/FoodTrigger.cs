using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTrigger : MonoBehaviour {

	public int triggerID = 0;


	private void OnTriggerStay(Collider other) {
		if (other.GetComponent<PlayerController>().pickup) {
			if (FoodChecklist.reqFoodArr.Contains(triggerID)) {
				// Add points for picking up the right food

				// Remove the food that just got picked up
				GameObject deleteBlock = GameObject.Find(SpawnTriggers.departmentNames[triggerID]);
				Destroy(deleteBlock);
				FoodChecklist.reqFoodArr.Remove(triggerID);

				// Check for streak stat
				FoodChecklist.currentStreak++;
				if (FoodChecklist.maxStreak < FoodChecklist.currentStreak) {
					FoodChecklist.maxStreak = FoodChecklist.currentStreak;
				}

				// Add one point to the picked up food array
				FoodChecklist.foodCount[other.GetComponent<PlayerController>().playerID] += 1;
			} else {
				// Subtract points for picking up the wrong food

				// Reset streak counter
				FoodChecklist.currentStreak = 0;
			}
		}
	}

}
