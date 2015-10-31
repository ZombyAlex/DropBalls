using UnityEngine;
using System.Collections;

public static class Data
{
	public static int score;
	public static float timeGame;
	public static bool isGameOver = false;
	public static string textIP = "127.0.0.1";
	public static bool server = false;
	private static int curBallId = 0;
	public static int CurBallId
	{
		get { return curBallId++; }
	}
}
