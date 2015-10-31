using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
	public Text text;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Data.isGameOver)
			text.enabled = true;
		else
			text.enabled = false;
	}
}
