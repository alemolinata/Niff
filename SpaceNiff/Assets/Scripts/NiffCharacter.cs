using UnityEngine;
using System.Collections;

public class NiffCharacter : MonoBehaviour {

	public enum Mode {Walking, Flying, Swimming, WalkFly, WalkSwim, FlySwim};
	public static Mode mode = Mode.Walking;
	WalkingMode walkingMode;
	FlyingMode flyingMode;
	SwimmingMode swimmingMode;
	WalkFlyMode walkFlyMode;
	WalkSwimMode walkSwimMode;
	FlySwimMode flySwimMode;

	public bool shouldDie;
	public Animator anim;

	private Rigidbody2D r;
	private Transform t;

	public Collider2D bodyCollider;
	public Collider2D legsCollider;

	public bool grounded = false; // Whether or not the player is grounded.
	public bool ceilingHit = false;
	public bool inWater = false;

	int score = 0;

	[SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
	[SerializeField] private LayerMask whatIsWater; // A mask determining what is water to the character
	public Transform groundCheck; // A position marking where to check if the player is grounded.
	private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public Transform ceilingCheck; // A position marking where to check for ceilings
	private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up

	public float waterLevel =  -5f;
	float floatHeight = 5f;
	float bounceDamp  = 0.05f;
	Vector3 buoyancyCentreOffset;

	public OverlayController overlayController;

	private float forceFactor;
	private Vector3 actionPoint;
	private Vector3 uplift;

	private void Awake() {
		walkingMode = GetComponent<WalkingMode> ();
		flyingMode = GetComponent<FlyingMode> ();
		swimmingMode = GetComponent<SwimmingMode> ();

		walkFlyMode = GetComponent<WalkFlyMode> ();
		walkSwimMode = GetComponent<WalkSwimMode> ();
		flySwimMode = GetComponent<FlySwimMode> ();

		anim = GetComponent<Animator> ();
		r = GetComponent<Rigidbody2D> ();
		t = GetComponent<Transform> ();
		//groundOverlay = GameObject.Find("MainCamera/GroundOverlay");
	}

	void FixedUpdate(){
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundedRadius, whatIsGround);
		ceilingHit = Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround);
		inWater = Physics2D.OverlapCircle(t.position, 1f, whatIsWater);
		anim.SetBool ("Grounded", grounded);
		anim.SetBool ("InWater", inWater);
	}

	void Update () {
		if (mode == Mode.Walking) {
			walkingMode.enabled = true;
			flyingMode.enabled = false;
			swimmingMode.enabled = false;
			walkFlyMode.enabled = false;
			walkSwimMode.enabled = false;
			flySwimMode.enabled = false;

		} else if (mode == Mode.Flying) {
			walkingMode.enabled = false;
			flyingMode.enabled = true;
			swimmingMode.enabled = false;
			walkFlyMode.enabled = false;
			walkSwimMode.enabled = false;
			flySwimMode.enabled = false;

		} else if (mode == Mode.Swimming) {
			walkingMode.enabled = false;
			flyingMode.enabled = false;
			swimmingMode.enabled = true;
			walkFlyMode.enabled = false;
			walkSwimMode.enabled = false;
			flySwimMode.enabled = false;

		} else if (mode == Mode.WalkFly) {
			walkingMode.enabled = false;
			flyingMode.enabled = false;
			swimmingMode.enabled = false;
			walkFlyMode.enabled = true;
			walkSwimMode.enabled = false;
			flySwimMode.enabled = false;

		} else if (mode == Mode.WalkSwim) {
			walkingMode.enabled = false;
			flyingMode.enabled = false;
			swimmingMode.enabled = false;
			walkFlyMode.enabled = false;
			walkSwimMode.enabled = true;
			flySwimMode.enabled = false;
			
		} else {
			walkingMode.enabled = false;
			flyingMode.enabled = false;
			swimmingMode.enabled = false;
			walkFlyMode.enabled = false;
			walkSwimMode.enabled = false;
			flySwimMode.enabled = true;
			
		}

		if (NiffUserControl.missingLeg) {
			overlayController.enableOverlay(true);
		} else {
			overlayController.enableOverlay(false);
		}

		// Floating in Water
		actionPoint = t.position;
		forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
		float floatMultiplier = (mode == Mode.Swimming) ? 1f: 0.7f;
		
		if (forceFactor > 0f) 
		{
			uplift = -Physics.gravity * (forceFactor - r.velocity.y * bounceDamp)*floatMultiplier;
			r.AddForceAtPosition(uplift, actionPoint);
		}
	}
	void OnCollisionEnter2D (Collision2D coll){
		if (coll.gameObject.tag == "Coins") {
			Destroy (coll.gameObject);
		}
		if (shouldDie) {
			if (coll.gameObject.tag == "Killer"){
				overlayController.enableOverlay(true);
				Destroy (gameObject);
			}
		}
	}
	
	void OnGUI()
	{
		GUILayout.Label("Score = " + 200 );
	}

}