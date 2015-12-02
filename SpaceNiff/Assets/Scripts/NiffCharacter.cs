using UnityEngine;
using System.Collections;

public class NiffCharacter : MonoBehaviour {

	public enum Mode {Walking, Flying, Swimming};
	public static Mode mode = Mode.Walking;
	WalkingMode walkingMode;
	FlyingMode flyingMode;
	SwimmingMode swimmingMode;
	public Animator anim;

	private Rigidbody2D r;
	private Transform t;

	public Collider2D bodyCollider;
	public Collider2D legsCollider;

	public bool grounded = false; // Whether or not the player is grounded.
	public bool ceilingHit = false;
	public bool inWater = false;

	[SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
	[SerializeField] private LayerMask whatIsWater; // A mask determining what is water to the character
	public Transform groundCheck; // A position marking where to check if the player is grounded.
	private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public Transform ceilingCheck; // A position marking where to check for ceilings
	private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up

	public float waterLevel =  -3.16f;
	float floatHeight = 2f;
	float bounceDamp  = 0.05f;
	Vector3 buoyancyCentreOffset;
	
	
	private float forceFactor;
	private Vector3 actionPoint;
	private Vector3 uplift;

	private void Awake() {
		walkingMode = GetComponent<WalkingMode> ();
		flyingMode = GetComponent<FlyingMode> ();
		swimmingMode = GetComponent<SwimmingMode> ();
		anim = GetComponent<Animator> ();
		r = GetComponent<Rigidbody2D> ();
		t = GetComponent<Transform> ();
	}

	void FixedUpdate(){
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		ceilingHit = Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround);
		inWater = Physics2D.OverlapCircle(t.position, 1f, whatIsWater);
	}

	void Update () {
		if (mode == Mode.Walking) {
			walkingMode.enabled = true;
			flyingMode.enabled = false;
			swimmingMode.enabled = false;
		} else if (mode == Mode.Flying) {
			walkingMode.enabled = false;
			flyingMode.enabled = true;
			swimmingMode.enabled = false;
		} else {
			walkingMode.enabled = false;
			flyingMode.enabled = false;
			swimmingMode.enabled = true;
		}

		// Floating in Water
		actionPoint = t.position;
		forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
		
		if (forceFactor > 0f) 
		{
			uplift = -Physics.gravity * (forceFactor - r.velocity.y * bounceDamp);
			r.AddForceAtPosition(uplift, actionPoint);
		}
	}
}