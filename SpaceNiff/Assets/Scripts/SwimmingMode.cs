using UnityEngine;
using System.Collections;

[RequireComponent(typeof (NiffCharacter))]
public class SwimmingMode : MonoBehaviour {
	private NiffCharacter niff;
	Rigidbody2D niffRB;
	public float forceX = 250;
	public float forceY = -250;
	private bool prevButtonState = false;

	void Awake () {
		niffRB = GetComponent<Rigidbody2D>();
		niff = GetComponent<NiffCharacter>();
	}



	void OnEnable(){
		niff.bodyCollider.offset = new Vector2 (0.39f, 0.31f);
		niff.legsCollider.offset = new Vector2 (-0.64f, -0.06f);
	}

	void FixedUpdate () {
		niffRB.velocity = new Vector2 (niffRB.velocity.x, Mathf.Clamp (niffRB.velocity.y, -10, 10));
		if (niff.inWater) {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis( niffRB.velocity.y*7, Vector3.forward),Time.deltaTime*4);
		} else {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis( 0, Vector3.forward),Time.deltaTime*4);
		}

		if (NiffUserControl.powerStone != 0 && !NiffUserControl.missingLeg && !(niff.anim.GetCurrentAnimatorStateInfo (0).IsName ("SwimmingStoneOn") || niff.anim.GetCurrentAnimatorStateInfo (0).IsName ("SwimmingNoStoneIdle"))) {
			if (NiffUserControl.buttonState && !(prevButtonState == NiffUserControl.buttonState)) {
				//niffRB.velocity = new Vector2 (niffRB.velocity.x, 0);
				niffRB.AddForce (new Vector2 (forceX, forceY));
				//transform.rotation = Quaternion.AngleAxis(-60, Vector3.forward);
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis( -60, Vector3.forward),Time.deltaTime*10);
			}
			prevButtonState = NiffUserControl.buttonState;
		}
	}
}
