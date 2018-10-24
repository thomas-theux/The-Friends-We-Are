using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndDestroy : MonoBehaviour {

	private void Start() {
		Destroy(this.gameObject, 4);
	}

	private void Update() {
		transform.position += new Vector3(0, 0, 0.1f);
	}

}
