using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour {

	private void Awake() {
		StartCoroutine(DestroySelf());
	}


	IEnumerator DestroySelf() {
		yield return new WaitForSeconds(1);
		Destroy(this.gameObject);
	}
	
}
