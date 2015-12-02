using UnityEngine;
using System.Collections;

[RequireComponent(typeof (NiffCharacter))]
public class FlyingMode : MonoBehaviour {

	private NiffCharacter niff;
	Rigidbody2D r;
	public float forceY = 200f;

	public float moveForce = 200f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.

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
		if (NiffUserControl.powerStone != 0 && !NiffUserControl.missingLeg && !(niff.anim.GetCurrentAnimatorStateInfo (0).IsName ("FlyingStoneOn") || niff.anim.GetCurrentAnimatorStateInfo (0).IsName ("FlyinNoStoneIdle"))) {
			if (NiffUserControl.buttonState && !(prevButtonState == NiffUserControl.buttonState)) {
				r.velocity = new Vector2 (maxSpeed, 0);
				r.AddForce (new Vector2 (0, forceY));
			}
			else{
				r.velocity = new Vector2(maxSpeed, r.velocity.y);
			}
			prevButtonState = NiffUserControl.buttonState;
		}
		else
			r.velocity = new Vector2(0, r.velocity.y);
	}
}
