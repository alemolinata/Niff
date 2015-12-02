using UnityEngine;
using System.Collections;

[RequireComponent(typeof (NiffCharacter))]
public class WalkingMode : MonoBehaviour {
	
	Rigidbody2D r;
	private NiffCharacter niff;
	public float moveForce = 200f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.

	private bool touchBump = false;

	void Awake() {
		r = GetComponent<Rigidbody2D>();
		niff = GetComponent<NiffCharacter>();
	}

	void OnEnable(){
		niff.bodyCollider.offset = new Vector2 (0f, 0f);
		niff.legsCollider.offset = new Vector2 (-0.16f, -0.7f);
		transform.rotation = Quaternion.identity;
	}

	void FixedUpdate(){
		if (NiffUserControl.powerStone != 0 && !(niff.anim.GetCurrentAnimatorStateInfo(0).IsName("WalkingStoneOn") || niff.anim.GetCurrentAnimatorStateInfo(0).IsName("WalkingNoStonIdle") ))
			Move (1f, NiffUserControl.buttonState);
		else
			Move (0f, false);
	}

	public void Move(float move, bool crouch){

		if (crouch) {
			niff.bodyCollider.offset = new Vector2 (0f, -0.6f);
			if(touchBump)
				r.velocity = new Vector2(0f, r.velocity.y);
			else
				r.velocity = new Vector2(move*maxSpeed*0.6f, r.velocity.y);
		} else {
			r.velocity = new Vector2(move*maxSpeed, r.velocity.y);
			niff.bodyCollider.offset = new Vector2 (0f, 0f);
		}
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "BumpPink")
			touchBump = true;
	}
	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == "BumpPink")
			touchBump = false;
	}
}