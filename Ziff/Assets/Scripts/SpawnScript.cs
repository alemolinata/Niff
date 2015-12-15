using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	public GameObject[] obj;
	public float spawnMin = 1f;
	public float spawnMax = 2f;
	// Use this for initialization
	void Start () {
		Spawn ();
	}

	void FixedUpdate(){

	}

	void Spawn () {
		Instantiate (obj [Random.Range (0, obj.Length)], transform.position, Quaternion.identity);

		if(spawnMin!=spawnMax)
			Invoke ("Spawn", Random.Range(spawnMin, spawnMax));
		else
			Invoke ("Spawn", spawnMin);
	}
}
