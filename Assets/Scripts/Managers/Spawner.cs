using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour 
{
	public static Spawner spawner;

	[SerializeField]
	ObjectCache[] Caches;

	Hashtable ActiveCachedObjects;

	[System.Serializable]
	class ObjectCache
	{
		public GameObject Prefab;
		public int CacheSize = 10;

		private GameObject[] Objects;
		private int CacheIndex = 0;

		public void Initialize()
		{
			Objects = new GameObject[CacheSize];

			for(int i = 0; i < CacheSize; ++i)
			{
				Objects[i] = MonoBehaviour.Instantiate(Prefab) as GameObject;
				Objects[i].SetActive(false);
				Objects[i].name = Objects[i].name + i;
			}
		}

		public GameObject GetNextObjectInCache()
		{
			GameObject obj = null;

			for(int i = 0; i < CacheSize; ++i)
			{
				obj = Objects[CacheIndex];

				if(!obj.activeSelf)  
					break;

				CacheIndex = (CacheIndex + 1) % CacheSize;
			}

			if(obj.activeSelf)
			{
				Debug.LogWarning ("Spawn of " + Prefab.name + " exceeds cache size of " + CacheSize + "! Reusing already active object.", obj);
				Spawner.Destroy (obj);
			}

			CacheIndex = (CacheIndex + 1) % CacheSize;

			return obj;
		}
	}

	void Awake()
	{
		spawner = this;

		int Amount = 0;

		for(int i = 0; i < Caches.Length; ++i)
		{
			Caches[i].Initialize();

			Amount += Caches[i].CacheSize;
		}

		ActiveCachedObjects = new Hashtable(Amount);
	}

	public static GameObject Spawn(GameObject Prefab, Vector3 Position, Quaternion Rotation)
	{
		ObjectCache Cache = null;

		if(spawner)
		{
			for(int i = 0; i < spawner.Caches.Length; ++i)
			{
				if(spawner.Caches[i].Prefab == Prefab)
				{
					Cache = spawner.Caches[i];
				}
			}
		}
		
		if (Cache == null)
		{
			return Instantiate (Prefab, Position, Rotation) as GameObject;
		}

		GameObject obj = Cache.GetNextObjectInCache();

		obj.transform.position = Position;
		obj.transform.rotation = Rotation;

		obj.SetActive(true);
		spawner.ActiveCachedObjects[obj] = true;

		return obj;
	}

	public static void CacheDestroy(GameObject obj)
	{
		if(spawner && spawner.ActiveCachedObjects.ContainsKey(obj))
		{
			obj.SetActive(false);
			spawner.ActiveCachedObjects[obj] = false;
		}
		else
		{
			Destroy(obj);
		}
	}
}
