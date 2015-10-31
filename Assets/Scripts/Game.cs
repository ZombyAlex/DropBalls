using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
	public GameObject prefabBall;
	private float timeNextBall;
	private float timeGameOver;

	private List<Ball> balls = new List<Ball>();
	
	static Game mInstance;

	private GameObject board;


	public static Game Instance
	{
		get
		{
			return mInstance;
		}
	}

	void Awake()
	{
		mInstance = this;
	}
	
	void Start ()
	{
		Data.score = 0;
		Data.timeGame = 0;
		Data.isGameOver = false;
		timeNextBall = 1.0f;

		if (Data.server)
		{
			bool useNat = !Network.HavePublicAddress();
			Network.InitializeServer(16, 25111, useNat);
			SendScore();
		}
		else
		{
			NetworkConnectionError aError = Network.Connect(Data.textIP, 25111);
			if (aError != NetworkConnectionError.NoError)
			{
				Debug.LogError("Error connection");
			}
		}
		board = GameObject.Find("Board");
	}
	
	void Update ()
	{
		if (Data.server)
		{
			Data.timeGame += Time.deltaTime;
			timeNextBall -= Time.deltaTime;
			if (timeNextBall <= 0)
			{
				timeNextBall = Random.Range(2.0f, 3.0f);

				GameObject obj =
					Instantiate(prefabBall, new Vector3(Random.Range(-100, 100), 110, 0), Quaternion.identity) as GameObject;
				Ball ball = obj.GetComponent<Ball>();
				ball.Id = Data.CurBallId;
				balls.Add(ball);
			}
		}

		if (Data.isGameOver)
		{
			timeGameOver += Time.deltaTime;
			if (timeGameOver > 3)
			{
				Application.LoadLevel("MainMenu");
			}
		}

	}

	void OnServerInitialized()
	{
		Debug.Log("Server initialized and ready");
	}

	void OnConnectedToServer()
	{
		Debug.Log("Connected to server");

		NetworkViewID viewID = Network.AllocateViewID();

		GetComponent<NetworkView>().RPC("GetMessage", RPCMode.AllBuffered, viewID, "hello");
	}

	void OnFailedToConnect(NetworkConnectionError error)
	{
		Debug.Log("Could not connect to server: " + error);
		Application.LoadLevel("MainMenu");
	}

	[RPC]
	void GetMessage(NetworkViewID inViewId, string inMessage)
	{
		Debug.Log("Net message = " + inMessage);
	}

	[RPC]
	private void GetMessageBallPos(NetworkViewID inViewId, int inBallId, Vector3 inPos)
	{
		if (Data.server)
			return;
		Ball ball = balls.Find((x) => x.Id == inBallId);
		if (ball != null)
		{
			ball.transform.position = inPos;
		}
		else
		{
			GameObject obj = Instantiate(prefabBall, inPos, Quaternion.identity) as GameObject;
			Ball newBall = obj.GetComponent<Ball>();
			newBall.Id = inBallId;
			balls.Add(newBall);
		}
	}

	[RPC]
	private void GetMessageDestroyBall(NetworkViewID inViewId, int inBallId)
	{
		if (Data.server)
			return;
		Ball ball = balls.Find((x) => x.Id == inBallId);
		if (ball != null)
		{
			DestroyObject(ball.gameObject);
		}
	}

	[RPC]
	private void GetMessageBoardPos(NetworkViewID inViewId, Vector3 inPos)
	{
		if (Data.server)
			return;
		if (board != null)
		{
			board.transform.position = inPos;
		}
	}

	[RPC]
	private void GetMessageGameOver(NetworkViewID inViewId, int inGameOver)
	{
		if (Data.server)
			return;
		Data.isGameOver = inGameOver != 0;
	}

	[RPC]
	private void GetMessageScore(NetworkViewID inViewId, int inScore)
	{
		if (Data.server)
			return;
		Data.score = inScore;
	}

	public void SendPosBall(int inBallId, Vector3 inPos)
	{
		NetworkViewID viewID = Network.AllocateViewID();
		GetComponent<NetworkView>().RPC("GetMessageBallPos", RPCMode.AllBuffered, viewID, inBallId, inPos);
	}

	public void SendDestroyBall(int inBallId)
	{
		NetworkViewID viewID = Network.AllocateViewID();
		GetComponent<NetworkView>().RPC("GetMessageDestroyBall", RPCMode.AllBuffered, viewID, inBallId);
	}

	public void SendBoardPos(Vector3 inPos)
	{
		NetworkViewID viewID = Network.AllocateViewID();
		GetComponent<NetworkView>().RPC("GetMessageBoardPos", RPCMode.AllBuffered, viewID, inPos);
	}

	public void SendGameOver()
	{
		NetworkViewID viewID = Network.AllocateViewID();
		GetComponent<NetworkView>().RPC("GetMessageGameOver", RPCMode.AllBuffered, viewID, Data.isGameOver ? 1 : 0);
	}

	public void SendScore()
	{
		NetworkViewID viewID = Network.AllocateViewID();
		GetComponent<NetworkView>().RPC("GetMessageScore", RPCMode.AllBuffered, viewID, Data.score);
	}
}
