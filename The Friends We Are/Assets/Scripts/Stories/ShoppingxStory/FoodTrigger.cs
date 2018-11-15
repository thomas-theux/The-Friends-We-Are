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
			} else {
				// Subtract points for picking up the wrong food
			}
		}
	}

}
