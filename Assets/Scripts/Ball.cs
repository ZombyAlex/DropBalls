using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
	private int id;
	private const float startSpeedBall = 30;
	private float speedBall = 30;
	private float timeNextOffset;
	private const float maxOffsetX = 20.0f;
	private float offsetX;

	public int Id
	{
		get { return id; }
		set { id = value; }
	}

	void Start ()
	{
		timeNextOffset = Random.Range(0.5f, 1.0f);
	}
	
	void Update ()
	{
		if (!Data.server)
			return;
		if (Data.isGameOver)
			return;
		int period = (int) Data.timeGame/30;
		speedBall = startSpeedBall + period*10;
		Vector3 pos = transform.position;
		pos+=new Vector3(offsetX*Time.deltaTime, -speedBall*Time.deltaTime, 0);
		pos.x = Mathf.Clamp(pos.x, -100, 100);
		transform.position = pos;

		Game.Instance.SendPosBall(id, pos);

		timeNextOffset -= Time.deltaTime;
		if (timeNextOffset <= 0)
		{
			offsetX = Random.Range(-maxOffsetX, maxOffsetX);
			timeNextOffset = Random.Range(0.2f, 1.5f);
		}

		if (transform.position.y < -100)
		{
			Data.isGameOver = true;
			Game.Instance.SendGameOver();
			Game.Instance.SendDestroyBall(id);
			DestroyObject(gameObject);
		}
	}
}
