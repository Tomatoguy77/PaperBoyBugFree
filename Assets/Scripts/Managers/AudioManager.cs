using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	private float WaitForNewPitchTime = 0.5F;
	private float CurrentWaitForNewPitchTime = 0;

	private float NewPitch = 1;

	private AudioSource source;

	void Start () 
	{
		GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("AudioVolume");

		source = GetComponent<AudioSource>();
	}

	void Update()
	{
		/*CurrentWaitForNewPitchTime += Time.deltaTime;
		if(CurrentWaitForNewPitchTime >= WaitForNewPitchTime)
		{

			NewPitch = Random.Range(0.5F, 2F);
			CurrentWaitForNewPitchTime = 0;
		}

		if(source.pitch != NewPitch)
		{
			source.pitch = Mathf.MoveTowards(source.pitch, NewPitch, 0.7F);
		}*/
	}
}
