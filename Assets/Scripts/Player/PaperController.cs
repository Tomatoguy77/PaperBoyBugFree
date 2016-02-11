using UnityEngine;
using System.Collections;

public class PaperController : CacheMB
{
	public float RotationSpeed = 700; // Set to 700 by default.
	public float Speed = 10;

	GameObject Target;

	bool Steel;

	void Update()
	{
		if(Global.Instance.IsPlaying)
		{
			transform.Rotate(new Vector3(0, 0, RotationSpeed * Time.deltaTime));
			
			if(Target != null)
			{
				Vector3 MoveDirection = Target.transform.position - transform.position; 

				GetComponent<Rigidbody2D>().velocity = MoveDirection.normalized * (Global.Instance.Speed * 3F);
			}
		}
	}

	public void StartPaper(GameObject Target, bool Steel)
	{
		this.Target = Target;

		this.Steel = Steel;
	}

	void OnTriggerEnter2D(Collider2D Coll)
	{
		if(Coll.gameObject == Target)
		{
			if(Steel)
				Spawner.Destroy(Coll.gameObject);

			Destroy (gameObject);
		}
	}
}
