using UnityEngine;
using System.Collections;

public class ZiffCharacter : MonoBehaviour {

	public enum Mode {Walking, Flying, Swimming, WalkFly, WalkSwim, FlySwim, NoLegs};
	public static Mode mode = Mode.NoLegs;
	WalkingMode walkingMode;
	FlyingMode flyingMode;
	SwimmingMode swimmingMode;
	WalkFlyMode walkFlyMode;
	WalkSwimMode walkSwimMode;
	FlySwimMode flySwimMode;
	NoLegsMode noLegsMode;

	public bool shouldDie;
	public Animator anim;
	public bool win = false;

	public static  bool dead = false;
	public int lives;

	private Rigidbody2D r;
	private Transform t;

	public Collider2D bodyCollider;
	public Collider2D legsCollider;

	public bool grounded = false; // Whether or not the player is grounded.
	public bool ceilingHit = false;
	public bool inWater = false;

	public bool hasShield = false;

	public int score = 0;
	private int lifeLostTimer =0;

	public GameObject shield;

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

		noLegsMode = GetComponent<NoLegsMode> ();

		anim = GetComponent<Animator> ();
		r = GetComponent<Rigidbody2D> ();
		t = GetComponent<Transform> ();

	}

	void Start(){
		bodyCollider.offset = new Vector2 (0f, 0f);
		legsCollider.offset = new Vector2 (-0.16f, -0.7f);
	}

	void FixedUpdate(){
		if (win) {

		}
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundedRadius, whatIsGround);
		ceilingHit = Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround);
		inWater = Physics2D.OverlapCircle(t.position, 1f, whatIsWater);
		anim.SetBool ("Grounded", grounded);
		anim.SetBool ("InWater", inWater);

		if (lives <= 0) {
			dead = true;
			anim.SetBool("Dead", true);
			walkingMode.enabled = false;
			flyingMode.enabled = false;
			swimmingMode.enabled = false;
			walkFlyMode.enabled = false;
			walkSwimMode.enabled = false;
			flySwimMode.enabled = false;
			Destroy (gameObject, 2);
		}
		if (lifeLostTimer > 40) {
			lifeLostTimer = 0;
		} else if (lifeLostTimer > 0) {
			lifeLostTimer ++;
		}
		if (win) {
			Destroy (gameObject);
		}

	}

	void Update () {
		if (mode == Mode.Walking) {
			walkingMode.enabled = true;
			flyingMode.enabled = false;
			swimmingMode.enabled = false;
			walkFlyMode.enabled = false;
			walkSwimMode.enabled = false;
			flySwimMode.enabled = false;
			noLegsMode.enabled = false;

		} else if (mode == Mode.Flying) {
			walkingMode.enabled = false;
			flyingMode.enabled = true;
			swimmingMode.enabled = false;
			walkFlyMode.enabled = false;
			walkSwimMode.enabled = false;
			flySwimMode.enabled = false;
			noLegsMode.enabled = false;

		} else if (mode == Mode.Swimming) {
			walkingMode.enabled = false;
			flyingMode.enabled = false;
			swimmingMode.enabled = true;
			walkFlyMode.enabled = false;
			walkSwimMode.enabled = false;
			flySwimMode.enabled = false;
			noLegsMode.enabled = false;

		} else if (mode == Mode.WalkFly) {
			walkingMode.enabled = false;
			flyingMode.enabled = false;
			swimmingMode.enabled = false;
			walkFlyMode.enabled = true;
			walkSwimMode.enabled = false;
			flySwimMode.enabled = false;
			noLegsMode.enabled = false;

		} else if (mode == Mode.WalkSwim) {
			walkingMode.enabled = false;
			flyingMode.enabled = false;
			swimmingMode.enabled = false;
			walkFlyMode.enabled = false;
			walkSwimMode.enabled = true;
			flySwimMode.enabled = false;
			noLegsMode.enabled = false;
			
		} else if (mode == Mode.FlySwim) {
			walkingMode.enabled = false;
			flyingMode.enabled = false;
			swimmingMode.enabled = false;
			walkFlyMode.enabled = false;
			walkSwimMode.enabled = false;
			flySwimMode.enabled = true;
			noLegsMode.enabled = false;
		} else {
			walkingMode.enabled = false;
			flyingMode.enabled = false;
			swimmingMode.enabled = false;
			walkFlyMode.enabled = false;
			walkSwimMode.enabled = false;
			flySwimMode.enabled = false;
			noLegsMode.enabled = true;
		}
		if (ZiffUserControl.missingLeg || dead || win) {
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
		if(ZiffUserControl.missingLeg || win)
			r.velocity = new Vector2(0, 0);
		shield.GetComponent<SpriteRenderer> ().color = (hasShield) ? new Color (1f, 1f, 1f, 1f) : new Color (1f, 1f, 1f, 0f);
	}


	void OnTriggerEnter2D(Collider2D trig){
		if (trig.gameObject.tag == "Coins") {
			Destroy (trig.gameObject);
			score += 10;
		}
		if (trig.gameObject.tag == "ShieldPowerUp") {
			Destroy (trig.gameObject);
			hasShield = true;
		}

	}

	void OnCollisionEnter2D (Collision2D coll){
		if (coll.gameObject.tag == "Friend"){
			win = true;
		}
		if (shouldDie) {
			//if (lifeLostTimer == 0 && coll.gameObject.tag == "Killer"){
			if (coll.gameObject.tag == "Killer"){
				if(hasShield){
					Destroy (coll.gameObject);
					hasShield = false;
				}
				else{
					lifeLostTimer = 1;
					Destroy (coll.gameObject);
					lives --;
				}
			}
		}
	}
}