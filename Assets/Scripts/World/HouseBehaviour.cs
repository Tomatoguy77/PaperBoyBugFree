using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HouseBehaviour : BasicObstacleBehaviour
{	
	private bool HasDelivered = false;

	public bool NoDelivery = true;

	public Sprite[] GoodHouseSprites;
	public Sprite[] BadHouseSprites;

	private bool IsGoodHouse = true;

	public GameObject[] Trees;

	public bool IsRight;

	private float MinX = -1F, MinY = -3F, MaxX = 1F, MaxY = -0.3F;

	private RotateSide Side;

	List<GameObject> SpawnedTrees;

	private GameObject TreeContainer;

	public void SetRight(bool IsRight)
	{
		this.IsRight = IsRight;
	}

	private void SpawnTrees(float Count)
	{
		for(int i = 0; i < Count; ++i)
		{
			GameObject NewTree = (GameObject)Instantiate(Trees[Random.Range(0, Trees.Length)], transform.position, Quaternion.identity);

			Vector3 RandomLocation = Vector3.zero;
			RandomLocation.x = Random.Range(-0.9F, 0.6F);
			RandomLocation.y = Random.Range(0, 2) == 0 ? Random.Range(-3F, -3.5F) : Random.Range(0.5F, 3.5F);			
			
			NewTree.transform.localPosition = TreeContainer.transform.position + RandomLocation;
			NewTree.transform.parent = TreeContainer.transform;

			SpawnedTrees.Add(NewTree);
		}
	}

	public void Deliver()
	{
		if(HasDelivered)
			return;

		if(IsGoodHouse)
		{
			Global.Instance.Dollars++;

			Global.Instance.ComboMultiplier += 0.01F;
		}
		else
		{
			Global.Instance.ComboMultiplier = 1F;

			GameObject Dog = (GameObject)Instantiate (Resources.Load("Obstacles/DogObstacle"), transform.position, Quaternion.identity);
			Dog.GetComponent<DogObstacle>().IsRunningLeft = IsRight;
		}

		GetComponent<Animator>().SetTrigger("Deliver");
		transform.FindChild("Particle System").GetComponent<ParticleSystem>().Play();

		HasDelivered = true;
	}

	void OnEnable()
	{
		SpawnedTrees = new List<GameObject>();

		TreeContainer = new GameObject("TreeContainer");
		TreeContainer.AddComponent<BasicObstacleBehaviour>();

		TreeContainer.transform.position = transform.position;
		TreeContainer.transform.localScale = new Vector3(1, 1, 1);

		IsGoodHouse = Random.Range(0, 2) == 0? true : false;
		if(IsGoodHouse)
		{
			transform.FindChild("Sprite").GetComponent<SpriteRenderer>().sprite = GoodHouseSprites[Random.Range(0, GoodHouseSprites.Length)];
			transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = Color.white;
		}
		else
		{
			transform.FindChild("Sprite").GetComponent<SpriteRenderer>().sprite = BadHouseSprites[Random.Range(0, BadHouseSprites.Length)];
			transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = new Color(0.47F, 0.47F, 0.47F);
		}
		
		int RandomTreeCount = Random.Range(2, 6);
		SpawnTrees(RandomTreeCount);

		if(IsRight)
		{
			transform.localScale = new Vector3(3, 3, 1);
		}
		else
		{
			transform.localScale = new Vector3(-3, 3, 1);
		}

		if(Global.Instance.IsDisco) 
		{
			SideToRotate = Random.Range(0, 2) == 0 ? RotateSide.Left : RotateSide.Right;
			StartCoroutine(WaitForColorChange());
		}
	}	

	void OnDisable()
	{
		if(IsGoodHouse && !HasDelivered)
			Global.Instance.ComboMultiplier = 1;

		HasDelivered = false;
		NoDelivery = true;

		Destroy(TreeContainer);

		StopAllCoroutines();
	}
}
