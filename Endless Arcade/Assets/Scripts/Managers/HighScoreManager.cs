using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScoreManager : MonoBehaviour {

	public static int highscore;
	public const string HIGHSCORE_KEY = "highscore";

	Text text;

	void Awake(){
		text = GetComponent <Text> ();
		highscore = PlayerPrefs.GetInt(HIGHSCORE_KEY, 0);
		text.text = "Highscore: "+highscore;
	}

	public static void checkHighScore(){
		if(ScoreManager.score > highscore)
			PlayerPrefs.SetInt (HIGHSCORE_KEY, ScoreManager.score);
	}
}
