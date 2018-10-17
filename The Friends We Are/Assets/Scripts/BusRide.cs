using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusRide : MonoBehaviour {

	private Rigidbody rb;


	private void Start() {
		rb = GetComponent<Rigidbody>();
	}


	private void Update() {
		Vector3 movement = new Vector3(0, 0, 24);
		rb.AddForce(movement);
	}

}
