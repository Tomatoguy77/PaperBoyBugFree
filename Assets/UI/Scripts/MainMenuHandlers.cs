using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuHandlers : MonoBehaviour 
{	
	public Slider AudioSlider;

	private Animator Anim;

	private bool Readonly = true;

	public Texture Noise;
	public Texture Borders;
	public Texture Paper;
	
	public Image ArrowLeft;
	public Image ArrowRight;

	public Image[] RainbowImages;
	
	private bool IsDisco = false;
	
	public Text HighscoreText;

	void Start()
	{		
		Anim = GetComponent<Animator>();

		AudioSlider.value  = PlayerPrefs.GetFloat("AudioVolume");
		Readonly = false;

		FindObjectOfType<DiscoSetting>().IsDisco = false;		
		
		HighscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0);
	}

	void Update()
	{
		#if UNITY_ANDROID
		if(ArrowLeft != null && ArrowRight != null)
		{
			if(Input.acceleration.x > 0)
			{
				ArrowRight.color = new Color(ArrowRight.color.r, ArrowRight.color.g, ArrowRight.color.b, Input.acceleration.x);
			}
			else if(Input.acceleration.x < 0)
			{
				ArrowLeft.color = new Color(ArrowLeft.color.r, ArrowLeft.color.g, ArrowLeft.color.b, Mathf.Abs(Input.acceleration.x));
			}
			else
			{
				ArrowLeft.color = ArrowLeft.color;
				ArrowRight.color = ArrowRight.color;
			}
		}
		#endif
	}

	public void InitPlay()
	{		
		FindObjectOfType<DiscoSetting>().IsDisco = IsDisco;
		Play ();
	}
	private void Play()
	{
		Application.LoadLevel("PlayScene");
	}

	public void ChangeDisco()
	{
		IsDisco = !IsDisco;
		if(IsDisco)
		{
			StartCoroutine(WaitForColorChange());

			Camera.main.gameObject.GetComponent<AudioSource>().Play();
		}
		else
		{
			StopAllCoroutines();

			Camera.main.gameObject.GetComponent<AudioSource>().Stop();

			foreach(Image bla in RainbowImages)
			{
				if(bla != null)
					bla.color = Color.white;
			}
		}
	}

	public void OpenFacebook()
	{
		Application.OpenURL("https://www.facebook.com/pages/Base-Games/233956963444538?ref=ts&fref=ts");
	}
	public void OpenTwitter()
	{
		Application.OpenURL("https://twitter.com/basegames");
	}

	IEnumerator WaitForColorChange()
	{
		yield return new WaitForSeconds(0.2F);

		foreach(Image bla in RainbowImages)
		{
			if(bla != null)
				bla.color = new Color(Random.Range(0F, 1F), Random.Range(0F, 1F), Random.Range(0F, 1F), 1); 
		}

		yield return StartCoroutine(WaitForColorChange());
	}

	public void SetAudioVolume()
	{
		if(Readonly)
			return;

		PlayerPrefs.SetFloat("AudioVolume", AudioSlider.value); // Set the volume for the audio under the PlayerPrefs key 'AudioVolume'.
	}

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Noise);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Borders);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Paper);
	}
}
