using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPoint : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Character") {
			DrivingxStory.drivingScore += 10;
			Destroy(gameObject);
		}
	}
}
