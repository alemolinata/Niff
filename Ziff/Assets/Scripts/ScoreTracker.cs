using UnityEngine;
using System.Collections;

public class ScoreTracker : MonoBehaviour {

	public ZiffCharacter player;
	private int score;
	private TextMesh scoreText;

	void Awake () {
		scoreText = GetComponent<TextMesh>();
		
	}

	void Update () {
		scoreText.text = player.score.ToString("D3");
	}
}
