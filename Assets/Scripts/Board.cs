using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{
	private const float speedBoard = 60;

	void Start () 
	{
	}
	
	void Update () 
	{

		if (!Data.server)
			return;
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			Vector3 pos = transform.position;
			pos.x -= speedBoard * Time.deltaTime;
			transform.position = pos;
			Game.Instance.SendBoardPos(transform.position);
		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			Vector3 pos = transform.position;
			pos.x += speedBoard * Time.deltaTime;
			transform.position = pos;
			Game.Instance.SendBoardPos(transform.position);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		Ball ball = collider.gameObject.GetComponent<Ball>();
		if(ball != null)
			Game.Instance.SendDestroyBall(ball.Id);
		DestroyObject(collider.gameObject);
		Data.score++;
		Game.Instance.SendScore();
	}
	
	void Test1()
	{
		int a=10;
		
	}
	
}