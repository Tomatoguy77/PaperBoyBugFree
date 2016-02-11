using UnityEngine;
using System.Collections;

public enum RotateSide
{
	None,
	Left,
	Right
}

public class BasicObstacleBehaviour : CacheMB 
{
	public bool RandomSprite = false;
	public Sprite[] RandomSprites;

	protected Vector2 MoveDirection = Vector3.zero;

	protected Animator Anim;

	public RotateSide SideToRotate;

	void Start()
	{
		if(RandomSprite)
		{
			transform.FindChild("Sprite").GetComponent<SpriteRenderer>().sprite = RandomSprites[Random.Range(0, RandomSprites.Length)];
		}

		Anim = GetComponent<Animator>();
	}

	void Update () 
	{
		if(Global.Instance.IsPlaying)
		{
			if(Anim != null)
				Anim.enabled = true;

			Move ();

			if(Global.Instance.IsDisco)
			{				
				switch(SideToRotate)
				{
					case RotateSide.Left:
					
					transform.Rotate(0, 0, -50 * Time.deltaTime);
					
					break;
					
					case RotateSide.Right:
					
					transform.Rotate(0, 0, 50 * Time.deltaTime);
					
					break;
					
					default: break;
				}
			}
		}
		else 
		{
			if(Anim != null)
				Anim.enabled = false;
			MoveDirection = Vector3.zero;
		}
	}

	protected virtual void Move()
	{
		MoveDirection = new Vector2(0, -Global.Instance.Speed);
		
		transform.Translate(MoveDirection * Time.deltaTime, Space.World);
	}

	protected IEnumerator WaitForColorChange()
	{
		yield return new WaitForSeconds(0.2F);		
		
		transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = new Color(Random.Range(0F, 1F), Random.Range(0F, 1F), Random.Range(0F, 1F), 1); 
		
		yield return StartCoroutine(WaitForColorChange());
	}
}
