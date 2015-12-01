using UnityEngine;
using System.Collections;

public class WaterTracker : MonoBehaviour {

	public Transform player;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) {
			transform.position = new Vector3(player.position.x + 5f, -9.61f, 0f);
		}
	}
}
