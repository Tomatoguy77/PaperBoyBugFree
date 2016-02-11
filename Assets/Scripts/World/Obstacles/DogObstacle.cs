using UnityEngine;
using System.Collections;

public class DogObstacle : BasicObstacleBehaviour
{
	public bool IsRunningLeft = true;

	void Start()
	{
		Anim = GetComponent<Animator>();

		if(IsRunningLeft)
			transform.localScale = new Vector3(2, 2, 1);
		else
			transform.localScale = new Vector3(-2, 2, 1);
	}

	protected override void Move ()
	{
		if(IsRunningLeft)
		{			
			MoveDirection = new Vector2(-4, -Global.Instance.Speed);
		}
		else
		{
			MoveDirection = new Vector2(4, -Global.Instance.Speed);
		}

		transform.Translate(MoveDirection * Time.deltaTime);
	}
}
