using UnityEngine;
using System.Collections;

[RequireComponent(typeof (ZiffCharacter))]
public class WalkingMode : MonoBehaviour {
	
	Rigidbody2D r;
	private ZiffCharacter ziff;
	public float moveForce = 200f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.

	private bool touchBump = false;
	private bool win;

	void Awake() {
		r = GetComponent<Rigidbody2D>();
		ziff = GetComponent<ZiffCharacter>();
		win = GetComponent<ZiffCharacter> ().win;
	}

	void OnEnable(){
		ziff.bodyCollider.offset = new Vector2 (0f, 0f);
		ziff.legsCollider.offset = new Vector2 (-0.16f, -0.7f);
		transform.rotation = Quaternion.identity;
	}

	void FixedUpdate(){
		if (!win && ZiffUserControl.powerStone != 0 && !ZiffUserControl.missingLeg && !(ziff.anim.GetCurrentAnimatorStateInfo (0).IsName ("WalkingStoneOn") || ziff.anim.GetCurrentAnimatorStateInfo (0).IsName ("WalkingNoStonIdle"))){
			if (ziff.grounded) {
				Move (1f, ZiffUserControl.buttonState);
			}
		}
		else if(ZiffUserControl.missingLeg || win)
			r.velocity = new Vector2(0, 0);
		else
			r.velocity = new Vector2(0, r.velocity.y);
	}

	public void Move(float move, bool crouch){
		if (crouch) {
			ziff.bodyCollider.offset = new Vector2 (0f, -0.6f);
			if(touchBump)
				r.velocity = new Vector2(0f, r.velocity.y);
			else
				r.velocity = new Vector2(move*maxSpeed*0.6f, r.velocity.y);
		} else {
			r.velocity = new Vector2(move*maxSpeed, r.velocity.y);
			ziff.bodyCollider.offset = new Vector2 (0f, 0f);
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