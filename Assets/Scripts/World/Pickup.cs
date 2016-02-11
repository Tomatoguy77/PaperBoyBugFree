using UnityEngine;
using System.Collections;

public enum PickupType
{
	None = -1,
	Bike = 0,
	SteelPaper = 1,
	GoldenPaper = 2,
	ColaBottle = 3
}

public class Pickup : BasicObstacleBehaviour
{
	public PickupType Type;

	void Start()
	{
	}

	void OnEnable()
	{
	}

	protected override void Move ()
	{
		base.Move ();
	}

	void OnTriggerEnter2D(Collider2D Coll)
	{
		if(Coll.tag == "Player")
		{
			Coll.GetComponent<PlayerController>().SetPickup(Type);

			Destroy(gameObject);
		}
	}
}
