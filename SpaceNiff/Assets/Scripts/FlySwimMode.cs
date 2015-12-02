using UnityEngine;
using System.Collections;

public class FlySwimMode : MonoBehaviour {
	private NiffCharacter niff;
	Rigidbody2D r;
	
	void Awake () {
		r = GetComponent<Rigidbody2D>();
		niff = GetComponent<NiffCharacter>();
	}

	void OnEnable(){
		niff.bodyCollider.offset = new Vector2 (0.39f, 0.31f);
		niff.legsCollider.offset = new Vector2 (-0.64f, -0.06f);
	}
}
