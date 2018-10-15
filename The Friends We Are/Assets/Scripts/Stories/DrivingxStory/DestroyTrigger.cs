using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour {

	private void OnTriggerExit(Collider other) {
		StartCoroutine(DestructionTimer());
	}


	IEnumerator DestructionTimer() {
		yield return new WaitForSeconds(2);
		Destroy(transform.parent.gameObject);
	}

}
