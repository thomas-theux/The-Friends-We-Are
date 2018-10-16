using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public GameObject character;
	private Vector3 offset;


	private void Start() {
		offset = new Vector3(-20, -20, -8);
	}

	private void Update() {
		this.gameObject.transform.position = character.transform.position - offset;
	}

}
