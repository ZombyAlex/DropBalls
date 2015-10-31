using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	bool isWindowConnect = false;
	Rect rectWindowConnect;

	void Start () {
		rectWindowConnect = new Rect(150, 150, 170, 80);
	}
	
	void Update () {
	
	}

	public void OnEnter()
	{
		Data.server = true;
		Application.LoadLevel("Game");
	}

	public void OnExit()
	{
		Application.Quit();
	}

	public void OnConnect()
	{
		isWindowConnect = true;
	}

	void OnGUI()
	{
		if (isWindowConnect)
		{
			rectWindowConnect = GUI.ModalWindow(0, rectWindowConnect, WindowProc, "Connect");
		}
	}

	void WindowProc(int inId)
	{
		Data.textIP = GUI.TextField(new Rect(10, 20, 150, 20), Data.textIP);
		if (GUI.Button(new Rect(10, 50, 150, 25), "Connect"))
		{
			Data.server = false;
			Application.LoadLevel("Game");
		}
	}
}
