using UnityEngine;
using System.Collections;

public class FlySwimMode : MonoBehaviour {
	private ZiffCharacter ziff;
	Rigidbody2D r;
	
	void Awake () {
		r = GetComponent<Rigidbody2D>();
		ziff = GetComponent<ZiffCharacter>();
	}

	void OnEnable(){
		ziff.bodyCollider.offset = new Vector2 (0.39f, 0.31f);
		ziff.legsCollider.offset = new Vector2 (-0.64f, -0.06f);
	}
}
