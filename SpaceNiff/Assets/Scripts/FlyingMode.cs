using UnityEngine;
using System.Collections;

[RequireComponent(typeof (NiffCharacter))]
public class FlyingMode : MonoBehaviour {

	private NiffCharacter niff;
	Rigidbody2D r;
	public float forceX = 250;
	public float forceY = 250;
	private bool prevButtonState = false;

	void Awake () {
		r = GetComponent<Rigidbody2D>();
		niff = GetComponent<NiffCharacter>();
	}
	void OnEnable(){
		niff.bodyCollider.offset = new Vector2 (0f, -0.48f);
		niff.legsCollider.offset = new Vector2 (-0.16f, 0.37f);
		transform.rotation = Quaternion.identity;
	}

	void FixedUpdate() {
		if (NiffUserControl.powerStoneOn && !(niff.anim.GetCurrentAnimatorStateInfo (0).IsName ("FlyingStoneOn") || niff.anim.GetCurrentAnimatorStateInfo (0).IsName ("FlyinNoStoneIdle"))) {
			if (NiffUserControl.buttonState && !(prevButtonState == NiffUserControl.buttonState)) {
				r.velocity = new Vector2 (r.velocity.x, 0);
				r.AddForce (new Vector2 (forceX, forceY));
			}
			prevButtonState = NiffUserControl.buttonState;
		}
	}
}
