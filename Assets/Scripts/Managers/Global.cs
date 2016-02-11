using UnityEngine;
using System.Collections;

public class Global 
{
	private static Global instance;
	public static Global Instance
	{
		get
		{
			if(instance == null)
				instance = new Global();

			return  instance;
		}
		set
		{
			instance = value;
		}
	}

	private float _speed = 1F;
	public float Speed
	{
		get { return _speed; }
		set
		{
			_speed = Mathf.Clamp(value, 1, 12);
		}
	}
	public float InitialSpeed;

	public int Dollars;
	public float DistanceScore = 0;	

	public float ComboMultiplier;
	
	public bool IsPlaying;
	public bool IsPlayerDead;

	public bool Tooltip;

	public float TotalScore
	{
		get
		{
			if(Dollars >= 5)
				return (Dollars * 0.2F) * DistanceScore;
			else
				return DistanceScore;
		}
	}

	public bool IsDisco;

	public Global()
	{
		DistanceScore = 0;

		Speed = 5;
		InitialSpeed = Speed;

		ComboMultiplier = 1;

		IsPlaying = true;
		IsPlayerDead = false;
	}

	public void PlayerDead()
	{
		IsPlaying = false;
		IsPlayerDead = true;
	}
	public void PauseGame()
	{
		IsPlaying = false;
	}
}
