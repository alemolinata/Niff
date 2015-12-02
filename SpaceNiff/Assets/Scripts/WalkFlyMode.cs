using UnityEngine;
using System.Collections;

public class WalkFlyMode : MonoBehaviour {
	private NiffCharacter niff;
	Rigidbody2D r;
	
	void Awake () {
		r = GetComponent<Rigidbody2D>();
		niff = GetComponent<NiffCharacter>();
	}
	
	void OnEnable(){
		niff.bodyCollider.offset = new Vector2 (0f, 0f);
		niff.legsCollider.offset = new Vector2 (-0.16f, -0.7f);
	}
}
