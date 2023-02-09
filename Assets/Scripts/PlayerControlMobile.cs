using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlMobile : MonoBehaviour
{

	public Joystick joystick;
	private float friction = 0.8f;

	void Start()
	{
		if (GetComponent<Rigidbody2D>() == null)
		{
			gameObject.AddComponent<Rigidbody2D>();
			GetComponent<Rigidbody2D>().gravityScale = 0;
		}
		if (GetComponent<BoxCollider2D>() == null)
		{
			gameObject.AddComponent<BoxCollider2D>();
		}
		gameObject.tag = "Player";
	}
	void Update()
	{
		GetComponent<Rigidbody2D>().AddForce(new Vector2(joystick.Horizontal * 5.2f, 0f) - GetComponent<Rigidbody2D>().velocity * friction);
	}

}
