using UnityEngine;
using System.Collections;

public class PatrollingObstacleBehaviour : BasicObstacleBehaviour 
{	
	public float PatrolSpeed = 5F;

	private float MinimalPatrol, MaximalPatrol;

	private PatrolSide Side;
	private enum PatrolSide
	{
		PatrollingRight,
		PatrollingLeft
	}

	void Start()
	{
		if(RandomSprite)
		{
			transform.FindChild("Sprite").GetComponent<SpriteRenderer>().sprite = RandomSprites[Random.Range(0, RandomSprites.Length)];
		}
		
		Anim = GetComponent<Animator>();
		
		transform.localScale = new Vector2(-1, 1);
	}

	protected override void Move ()
	{
		switch(Side)
		{
			case PatrolSide.PatrollingRight:
			
				MoveDirection = new Vector2(PatrolSpeed, -Global.Instance.Speed);
				if(transform.position.x >= MaximalPatrol)
				{
					transform.localScale = new Vector2(1, 1);
					Side = PatrolSide.PatrollingLeft;
				}
			
			break;
			
			case PatrolSide.PatrollingLeft:
			
				MoveDirection = new Vector2(-PatrolSpeed, -Global.Instance.Speed);
				if(transform.position.x <= MinimalPatrol)
				{
					transform.localScale = new Vector2(-1, 1);
					Side = PatrolSide.PatrollingRight;
				}

			break;
		}
		
		transform.Translate(MoveDirection * Time.deltaTime);
	}

	public void SetPatrol(float Minx, float MaxX)
	{
		MinimalPatrol = Minx;
		MaximalPatrol = MaxX;
	}
}
