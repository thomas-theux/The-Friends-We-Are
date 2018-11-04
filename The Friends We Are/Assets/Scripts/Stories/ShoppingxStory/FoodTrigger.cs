using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTrigger : MonoBehaviour {

	private void OnTriggerStay(Collider other) {
		if (other.GetComponent<PlayerController>().pickup) {
			print("TUT");
		}
	}

}
