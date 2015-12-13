using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {


	public AudioSource efxSource;
	public AudioSource musicSource;
	public AudioClip backgroundMusic;
	public AudioClip coin;
	public AudioClip death;
	public AudioClip enemyDeath;
	public AudioClip health;
	public AudioClip hit;
	public AudioClip shoot;
	public AudioClip shotgun;
	public AudioClip melonBite;
	public AudioClip carrotBite;
	public AudioClip cage;
	public AudioClip dash;


	private static SoundManager instance;
	public static SoundManager Instance {
		get {
			if (instance == null)
				instance = GameObject.FindGameObjectWithTag("Global").GetComponent<SoundManager>();
			return instance;
		}
	}

	public void Play(AudioClip clip)
	{
		efxSource.clip = clip;
		efxSource.pitch = Random.Range(0.95f, 1.05f);
		efxSource.Play();
	}


}
