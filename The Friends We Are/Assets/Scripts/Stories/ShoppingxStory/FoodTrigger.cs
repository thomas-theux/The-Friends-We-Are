using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTrigger : MonoBehaviour {

	public int triggerID = 0;

	public AudioSource pickUpFoodSound;
	public AudioSource wrongFoodSound;


	private void Start() {
		pickUpFoodSound = GameObject.Find("PickUpFoodSound").GetComponent<AudioSource>();
		wrongFoodSound = GameObject.Find("WrongFoodSound").GetComponent<AudioSource>();
	}


	private void OnTriggerStay(Collider other) {
		if (other.GetComponent<PlayerController>().pickup) {
			if (ShoppingxStory.reqFoodArr.Contains(triggerID)) {
				pickUpFoodSound.Play();

				// Remove the food that just got picked up
				GameObject deleteBlock = GameObject.Find(SpawnTriggers.departmentNames[triggerID]);
				Destroy(deleteBlock);
				ShoppingxStory.reqFoodArr.Remove(triggerID);

				// Check for streak stat
				ShoppingxStory.currentStreak++;
				if (ShoppingxStory.maxStreak < ShoppingxStory.currentStreak) {
					ShoppingxStory.maxStreak = ShoppingxStory.currentStreak;
				}

				// Add one point to the picked up food array
				ShoppingxStory.foodCount[other.GetComponent<PlayerController>().playerID] += 1;
			} else {
				wrongFoodSound.Play();

				// Reset streak counter
				ShoppingxStory.currentStreak = 0;
			}
		}
	}

}
