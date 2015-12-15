using UnityEngine;
using System.Collections;

public class HorizontalTracker : MonoBehaviour {

	public Transform player;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) {
			transform.position = new Vector3(player.position.x + 5f, transform.position.y, transform.position.z);
		}
	}

}
