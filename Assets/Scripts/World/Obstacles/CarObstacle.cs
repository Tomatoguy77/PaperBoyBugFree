using UnityEngine;
using System.Collections;

public class CarObstacle : BasicObstacleBehaviour 
{
	float CarSpeed;

	void Start()
	{		
		Anim = GetComponent<Animator>();
	}

	void OnEnable()
	{		
		transform.FindChild("Sprite").FindChild("Sprite").GetComponent<SpriteRenderer>().color = new Color(Random.Range(0F, 1F), Random.Range(0F, 1F), Random.Range(0F, 1F), 1);

		CarSpeed = Random.Range(0F, 2.5F);
	}

	protected override void Move ()
	{
		MoveDirection = new Vector2(0, -Global.Instance.Speed - CarSpeed);
		
		transform.Translate(MoveDirection * Time.deltaTime);
	}
}
