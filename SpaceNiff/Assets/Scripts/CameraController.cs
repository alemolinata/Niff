using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
	public float maxY;		// The maximum x and y coordinates the camera can have.
	public float minY;		// The minimum x and y coordinates the camera can have.
	public Transform player;
	
	bool CheckYMargin()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
	}

	void FixedUpdate(){
		if (player != null) {
			TrackPlayer ();
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
}