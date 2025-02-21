﻿using UnityEngine;
using System.Collections;

public class DestroyerScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "player") {
			Debug.Break ();
			return;
		}
		if (other.gameObject.transform.parent) {
			Destroy (other.gameObject.transform.parent.gameObject);
		} else {
			Destroy (other.gameObject);
		}
	}
}
