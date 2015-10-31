using System.Net.Mime;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
	public Text text;
	
	void Update () 
	{
		text.text = "Score: " + Data.score;
	}
}
