using UnityEngine;
using System.Collections;

public class PlayerController : CacheMB 
{
	public float Speed = 5.0F;

	public bool IsDeath;

	private Animator Anim;

    private Rigidbody2D rb;

	private PickupType CurrentPickup = PickupType.None;

	private float GlobalInitSpeed;

	private float ColaTimeout = 0;

	public ParticleSystem ColaBottleParticles;

    private float regularSpeed = 5;
    private float lerpTime = 1f;
    private float currentLerpTime;

    private bool _colaPickupActive;
    void Start()
	{
		Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    void Update () 
	{

        if (Global.Instance.IsPlaying)
		{
            //cola bottle
            ColaPickup();
            //cola
            PlayerMovement();
			#region Movement
			
			#if UNITY_STANDALONE || UNITY_EDITOR		
         
			#elif UNITY_ANDROID || UNITY_IOS
			
			Speed = 20F;		
			rigidbody2D.velocity = new Vector2(Input.acceleration.x * Speed, 0);
			
			#endif

			transform.rotation = Quaternion.Euler(0, 0, -GetComponent<Rigidbody2D>().velocity.x * 2);

			#endregion

			#region Touch/Click Input Check

			#if UNITY_STANDALONE || UNITY_EDITOR		
			
			if(Input.GetMouseButtonDown(0))
			{
				Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector2 MousePos = new Vector2(WorldPosition.x, WorldPosition.y);
				
				Collider2D Coll = Physics2D.OverlapPoint(MousePos);
				if(Coll != null)
				{
					if(Coll.tag == "House")
					{
						HouseBehaviour House = Coll.GetComponent<HouseBehaviour>();
						if(House != null)
						{
							if(House.NoDelivery)
							{
								if(CurrentPickup != PickupType.GoldenPaper)
								{
									GameObject Paper = (GameObject)Instantiate(Resources.Load("Player/Paper"), transform.position, Quaternion.identity);
									Paper.GetComponent<PaperController>().StartPaper(Coll.gameObject, false);
								}
								else
								{
									GameObject Paper = (GameObject)Instantiate(Resources.Load("Player/GoldenPaper"), transform.position, Quaternion.identity);
									Paper.GetComponent<PaperController>().StartPaper(Coll.gameObject, false);

									Global.Instance.Dollars += 4;
									ResetPickup();
								}
								House.NoDelivery = false;
							}
							House.Deliver();
						}
					}
					else if(CurrentPickup == PickupType.SteelPaper && Coll.tag == "Obstacle")
					{
						GameObject SteelPaper = (GameObject)Instantiate(Resources.Load("Player/SteelPaper"), transform.position, Quaternion.identity);
						GameObject DestroyParticle = (GameObject)Instantiate(Resources.Load("Obstacles/DestroyParticle"), Coll.transform.position, Quaternion.identity);

						Destroy (DestroyParticle, 0.5F);

						SteelPaper.GetComponent<PaperController>().StartPaper(Coll.gameObject, true);

						ResetPickup();
					}
				}
			}
			#elif UNITY_ANDROID || UNITY_IOS
			
			if(Input.touchCount == 1)
			{
				Vector3 WorldPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
				Vector2 TouchPosition = new Vector2(WorldPosition.x, WorldPosition.y);
				
				Collider2D Coll = Physics2D.OverlapPoint(TouchPosition);
				if(Coll != null)
				{
					if(Coll.tag == "House")
					{
						HouseBehaviour House = Coll.GetComponent<HouseBehaviour>();
						if(House != null)
						{				
							if(House.NoDelivery)
							{
								if(CurrentPickup != PickupType.GoldenPaper)
								{
									GameObject Paper = (GameObject)Instantiate(Resources.Load("Player/Paper"), transform.position, Quaternion.identity);
									Paper.GetComponent<PaperController>().StartPaper(Coll.gameObject, false);
								}
								else
								{
									GameObject Paper = (GameObject)Instantiate(Resources.Load("Player/GoldenPaper"), transform.position, Quaternion.identity);
									Paper.GetComponent<PaperController>().StartPaper(Coll.gameObject, false);
									
									Global.Instance.Dollars += 4;
									ResetPickup();
								}
								House.NoDelivery = false;
							}
							House.Deliver();
						}
					}
					else if(CurrentPickup == PickupType.SteelPaper && Coll.tag == "Obstacle")
					{
						GameObject SteelPaper = (GameObject)Instantiate(Resources.Load("Player/SteelPaper"), transform.position, Quaternion.identity);

						GameObject DestroyParticle = (GameObject)Instantiate(Resources.Load("Obstacles/DestroyParticle"), Coll.transform.position, Quaternion.identity);
						
						Destroy (DestroyParticle, 0.5F);

						SteelPaper.GetComponent<PaperController>().StartPaper(Coll.gameObject, true);
						
						ResetPickup();
					}
				}
			}
			
			#endif

			#endregion
		}
		else
		{
			if(Anim != null)
				Anim.enabled = false;
			transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}
    //movement
    private void PlayerMovement() {
        //	Speed = 15F;
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * Speed, 0);

        rb.AddForce(new Vector2(Input.GetAxis("Horizontal"), 0) * Speed * 4);

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            Global.Instance.Speed += 0.05f;

        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            Global.Instance.Speed -= 0.05f;
        }
        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            //use lerp to restore the speed of the player if no keys are pressed and the cola pickup is not active
            LerpMovement();
        }

    }

    private void LerpMovement() {
        if (_colaPickupActive == false)
            Global.Instance.Speed = Mathf.Lerp(2f, 6f, lerpTime);

    }
    //pickups
    //cola

    private void ColaPickup() {
        if (Anim != null)
            Anim.enabled = true;
        if (ColaBottleParticles != null && CurrentPickup == PickupType.ColaBottle)
        {
            ColaBottleParticles.enableEmission = true;

            //ColaBottleParticles.GetComponent<Renderer>().material.mainTexture = transform.FindChild("Sprite").GetComponent<Renderer>().material.mainTexture; ;
        }
        else if (ColaBottleParticles != null)
            ColaBottleParticles.enableEmission = false;

        if (CurrentPickup == PickupType.ColaBottle)
        {
            StartColaPickup();

            ColaTimeout += Time.deltaTime;
            if (ColaTimeout >= 3)
            {
                ResetPickup();
            }
        }
    }
	public void SetPickup(PickupType Type)
	{
		if(CurrentPickup == PickupType.None) // If the player does not have a pickup
		{
			CurrentPickup = Type; // Then set the pickup 

			switch(Type) // And check which pickup it is, to set it's properties to the player
			{			
				case PickupType.Bike:
					Anim.SetBool("Cycling", true);
				break;

				case PickupType.ColaBottle:
					GlobalInitSpeed = Global.Instance.Speed;
                    _colaPickupActive = true;
				break;
				
				default: break;
			}
		}
	}
	private void ResetPickup()
	{
        Debug.Log("reset");
		GetComponent<BoxCollider2D>().enabled = true;
		
		Anim.SetBool("Cycling", false);

		CurrentPickup = PickupType.None;
        _colaPickupActive = false;
	}

	void StartColaPickup()
	{
		GetComponent<BoxCollider2D>().enabled = false;

		Global.Instance.Speed = GlobalInitSpeed * 2;
	}

	void OnTriggerEnter2D(Collider2D Coll)
	{
        //should use global tag script
		if(Global.Instance.IsPlaying && Coll.tag == "Obstacle")
		{
			if(CurrentPickup != PickupType.Bike)
			{
				Global.Instance.PlayerDead();
				IsDeath = true;
			}
			else
			{
				Spawner.Destroy(Coll.gameObject);
				ResetPickup();
			}
		}
	}
}
