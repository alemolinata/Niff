using UnityEngine;
using System.Collections;

public class NoLegsMode : MonoBehaviour {
	private ZiffCharacter ziff;
	Rigidbody2D r;

	void Awake () {
		r = GetComponent<Rigidbody2D>();
		ziff = GetComponent<ZiffCharacter>();
	}
	void OnEnable(){
		ziff.bodyCollider.offset = new Vector2 (0f, 0f);
		ziff.legsCollider.offset = new Vector2 (-0.16f, 0f);
	}
}
