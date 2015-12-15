using UnityEngine;
using System.Collections;

public class OverlayController : MonoBehaviour {

	public SpriteRenderer rend;
	public Transform player;
	
	void Awake () {
		rend = GetComponent<SpriteRenderer>();
		rend.color = new Color(1f, 1f, 1f, 0f);
	}
	// Update is called once per frame
	void Update () {
		if (player != null) {
			transform.position = new Vector3(player.position.x + 5f, player.position.y, transform.position.z);
		}
	}

	public void enableOverlay (bool enable){
		rend.color = (enable) ? new Color(1f, 1f, 1f, 0.5f): new Color(1f, 1f, 1f, 0f);
	}
}
