using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
	public float maxY;		// The maximum x and y coordinates the camera can have.
	public float minY;		// The minimum x and y coordinates the camera can have.
	public Transform player;

	public GUIStyle textOverlay;
	public GUIStyle scoreOverlay;
	private int score;
	private int lives;
	private bool win;

	public Texture2D heart;

	bool CheckYMargin()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
	}
	

	void FixedUpdate(){
		if (player != null) {
			TrackPlayer ();
			score = player.GetComponent<ZiffCharacter>().score;
			lives = player.GetComponent<ZiffCharacter>().lives;
			win = player.GetComponent<ZiffCharacter>().win;
		}
	}

	void Update(){
		if (Input.GetKeyDown ("p")) {
			Application.LoadLevel("Level1");
		}
	}

	void TrackPlayer ()
	{
		float targetY = transform.position.y;
		// If the player has moved beyond the y margin...
		if(CheckYMargin())
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);
		targetY = Mathf.Clamp(targetY, minY, maxY);
		
		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(player.position.x + 5, targetY, -5);
	}

	void OnGUI () {
		GUI.Label (new Rect (Screen.width - 120, 20, 100, 30), score.ToString("D3"), scoreOverlay);
		if (lives > 2) {
			GUI.Label(new Rect(Screen.width - 220, 20, 40, 40), heart);
		}
		if (lives > 1) {
			GUI.Label(new Rect(Screen.width - 180, 20, 40, 40), heart);
		}
		if (lives > 0) {
			GUI.Label(new Rect(Screen.width - 140, 20, 40, 40), heart);
		}
		else
			GUI.Label (new Rect (Screen.width/2, Screen.height/2,100,50), "You Lost!", textOverlay);
		if (win) {
			GUI.Label (new Rect (Screen.width/2, Screen.height/2,100,50), "You Win!", textOverlay);
		}
	}
}