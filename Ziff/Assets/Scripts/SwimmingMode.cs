using UnityEngine;
using System.Collections;

[RequireComponent(typeof (ZiffCharacter))]
public class SwimmingMode : MonoBehaviour {
	private ZiffCharacter ziff;
	Rigidbody2D ziffRB;
	public float forceX = 20;
	public float forceY = -100;
	private bool prevButtonState = false;
	private bool win;

	void Awake () {
		ziffRB = GetComponent<Rigidbody2D>();
		ziff = GetComponent<ZiffCharacter>();
		win = GetComponent<ZiffCharacter> ().win;
	}



	void OnEnable(){
		ziff.bodyCollider.offset = new Vector2 (0.39f, 0.31f);
		ziff.legsCollider.offset = new Vector2 (-0.64f, -0.06f);
	}

	void FixedUpdate () {
		ziffRB.velocity = new Vector2 (Mathf.Clamp (ziffRB.velocity.x, 0, 8), Mathf.Clamp (ziffRB.velocity.y, -8, 6));
		if (ziff.inWater) {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis( ziffRB.velocity.y*7, Vector3.forward),Time.deltaTime*4);
		} else {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis( 0, Vector3.forward),Time.deltaTime*4);
		}

		if (!win && ZiffUserControl.powerStone != 0 && !ZiffUserControl.missingLeg && !(ziff.anim.GetCurrentAnimatorStateInfo (0).IsName ("SwimmingStoneOn") || ziff.anim.GetCurrentAnimatorStateInfo (0).IsName ("SwimmingNoStoneIdle"))) {
			if (ZiffUserControl.buttonState && !(prevButtonState == ZiffUserControl.buttonState)) {
				//ziffRB.velocity = new Vector2 (ziffRB.velocity.x, 0);
				ziffRB.AddForce (new Vector2 (forceX, forceY));
				//transform.rotation = Quaternion.AngleAxis(-60, Vector3.forward);
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis( -60, Vector3.forward),Time.deltaTime*10);
			}
			prevButtonState = ZiffUserControl.buttonState;
		}
		else if(ZiffUserControl.missingLeg || win)
			ziffRB.velocity = new Vector2(0, 0);
		else
			if(ziffRB.velocity.x > 0)
				ziffRB.velocity = new Vector2(0f, ziffRB.velocity.y);
	}
}
