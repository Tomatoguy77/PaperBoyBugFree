using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawner : CacheMB 
{
	public GameObject[] Obstacles;

	public float ObstacleSpawnRate = 2F;
	private float CurrentObstacleSpawnRate = 0;

	public float HouseSpawnRate = 1F;
	private float CurrentHouseSpawnRate = 0;

	public float PickupSpawnRate = 10F;
	private float CurrentPickupSpawnRate = 0;

	public Vector3 InitialSpawnPos;

	private float MinSpawnX, MaxSpawnX;

	private List<GameObject> Objects = new List<GameObject>();

	void Start()
	{
		MinSpawnX = -3;
		MaxSpawnX = 3;
	}

	public void UpdateSpawner()
	{
		CurrentObstacleSpawnRate += Time.deltaTime;
		if(CurrentObstacleSpawnRate >= ObstacleSpawnRate)
		{
			SpawnObstacle();

			CurrentObstacleSpawnRate = 0;

			ObstacleSpawnRate = 2F - (0.2F * Global.Instance.Speed - 3);
		}

		CurrentHouseSpawnRate += Time.deltaTime;
		if(CurrentHouseSpawnRate >= HouseSpawnRate)
		{
			SpawnHouse(MinSpawnX - 3F, false);
			SpawnHouse(MaxSpawnX + 3F, true);
			
			CurrentHouseSpawnRate = 0;

			if(Global.Instance.DistanceScore >= 1000)
			{
				HouseSpawnRate = 0.5F;
			}

			ObstacleSpawnRate = 1F - (0.1F * Global.Instance.Speed - 3);
		}

		CurrentPickupSpawnRate += Time.deltaTime;
		if(CurrentPickupSpawnRate >= PickupSpawnRate)
		{
			SpawnPickup();

			CurrentPickupSpawnRate = 0;
		}
	}

	private void FixedUpdate()
	{
		CleanList(Objects);

		if(Objects.Count != 0)
		{
			foreach(GameObject obj in Objects)
			{
				if(obj)
				{
					if(obj.transform.position.y < -InitialSpawnPos.y)
						RemoveObject(obj);
				}
			}
		}
	}

	private void SpawnObstacle()
	{
		Vector3 InitialPlusSpawnPos = InitialSpawnPos + new Vector3(Random.Range(MinSpawnX, MaxSpawnX), 0, 0);

		int ObstacleToSpawn = Random.Range(0, Obstacles.Length);
		if(ObstacleToSpawn == 1 || ObstacleToSpawn == 2 || ObstacleToSpawn == 3)
			ObstacleToSpawn = Random.Range(0, Obstacles.Length);

		GameObject NewObstacle = Spawner.Spawn(Obstacles[ObstacleToSpawn], InitialPlusSpawnPos, Quaternion.identity);

		switch(ObstacleToSpawn)
		{
			case 1:
				NewObstacle.GetComponent<PatrollingObstacleBehaviour>().SetPatrol(MinSpawnX, MaxSpawnX);
			break;
			case 2:
				NewObstacle.GetComponent<PatrollingObstacleBehaviour>().SetPatrol(MinSpawnX, MaxSpawnX);
			break;
			case 3:
				NewObstacle.GetComponent<PatrollingObstacleBehaviour>().SetPatrol(MinSpawnX, MaxSpawnX);
			break;

			case 5:			
				int RandomNumber = Random.Range(0, 2);
				if(RandomNumber == 0)
				{
					NewObstacle.transform.position = InitialSpawnPos + new Vector3(MaxSpawnX, 0, 0);
					NewObstacle.transform.localScale = new Vector3(-NewObstacle.transform.localScale.x, NewObstacle.transform.localScale.y, NewObstacle.transform.localScale.z);
				}
				else if(RandomNumber == 1)
				{
					NewObstacle.transform.position = InitialSpawnPos + new Vector3(MinSpawnX, 0, 0);
				}
			break;

			default: break;
		}

		Objects.Add(NewObstacle);
	}
	private void SpawnHouse(float X, bool IsRight)
	{
		Vector3 InitialPlusSpawnPos = InitialSpawnPos + new Vector3(X, Random.Range(0F, 6F), 0);
		GameObject NewHouse = Spawner.Spawn((GameObject)Resources.Load("Houses/House"), InitialPlusSpawnPos, Quaternion.identity);

		HouseBehaviour housie = NewHouse.GetComponent<HouseBehaviour>();
		housie.SetRight(IsRight);

		if(IsRight)
		{
			NewHouse.transform.localScale = new Vector3(3, 3, 1);
		}
		else
		{
			NewHouse.transform.localScale = new Vector3(-3, 3, 1);
		}
		
		Objects.Add(NewHouse);

		HouseSpawnRate = Random.Range(1.5F, 2.5F);
	}
	private void SpawnPickup()
	{		
		Vector3 InitialPlusSpawnPos = InitialSpawnPos + new Vector3(Random.Range(MinSpawnX, MaxSpawnX), 0, 0);

		int Pickup = Random.Range(0, 4);
		
		GameObject NewPickup = (GameObject)Instantiate(Resources.Load("Pickups/Pickup" + Pickup), InitialPlusSpawnPos, Quaternion.identity);
		
		Objects.Add(NewPickup);
	}

	#region List Functions
	private void CleanList(List<GameObject> ListToClean)
	{
		for(int i = 0; i < ListToClean.Count; ++i)
		{
			if(ListToClean[i] == null)
				ListToClean.Remove(ListToClean[i]);
		}
	}
	private void RemoveObject(GameObject ThisObject)
	{
		Spawner.CacheDestroy(ThisObject);
	}
	#endregion
}
