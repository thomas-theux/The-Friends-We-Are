using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPoint : MonoBehaviour {

	public GameObject collectEffect;


	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Character") {
			Instantiate(collectEffect, this.transform.position, collectEffect.transform.rotation);
			DrivingxStory.drivingScore += Mathf.Ceil(DrivingxStory.currentSpeed * 0.1f);
			Destroy(gameObject);
		}
	}
}
