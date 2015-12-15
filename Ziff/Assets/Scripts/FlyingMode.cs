using UnityEngine;
using System.Collections;

[RequireComponent(typeof (ZiffCharacter))]
public class FlyingMode : MonoBehaviour {

	private ZiffCharacter ziff;
	Rigidbody2D r;
	public float forceY = 200f;

	public float moveForce = 200f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.

	private bool prevButtonState = false;
	private bool win;

	void Awake () {
		r = GetComponent<Rigidbody2D>();
		ziff = GetComponent<ZiffCharacter>();
		win = GetComponent<ZiffCharacter> ().win;
	}
	void OnEnable(){
		ziff.bodyCollider.offset = new Vector2 (0f, -0.48f);
		ziff.legsCollider.offset = new Vector2 (-0.16f, 0.37f);
		transform.rotation = Quaternion.identity;
	}

	void FixedUpdate() {
		if (!win && ZiffUserControl.powerStone != 0 && !ZiffUserControl.missingLeg && !(ziff.anim.GetCurrentAnimatorStateInfo (0).IsName ("FlyingStoneOn") || ziff.anim.GetCurrentAnimatorStateInfo (0).IsName ("FlyinNoStoneIdle"))) {
			if (ZiffUserControl.buttonState && !(prevButtonState == ZiffUserControl.buttonState)) {
				r.velocity = new Vector2 (maxSpeed, 0);
				r.AddForce (new Vector2 (0, forceY));
			}
			else{
				r.velocity = new Vector2(maxSpeed, r.velocity.y);
			}
			prevButtonState = ZiffUserControl.buttonState;
		}
		else if(ZiffUserControl.missingLeg || win)
			r.velocity = new Vector2(0, 0);
		else
			r.velocity = new Vector2(0, r.velocity.y);
	}
}
