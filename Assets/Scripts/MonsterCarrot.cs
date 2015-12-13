using UnityEngine;
using System.Collections;

public class MonsterCarrot : Monster {
	// Use this for initialization
	public static int lives {
		get {
			return GameManager.Level <= 2 ? 2 : 3;
		}
	}
	public static float speed {
		get {
			return GameManager.Level <= 3 ? 0.02f : 0.04f;
		}
	}
	public static int biteDamage {
		get {
			return 2;
		}
	}
	public static float biteDelay = 0.8f;
	private Animator carrotAnimator;
	
	protected int nCoins = 1;
	
	void Awake() {
		_lives = lives;
		carrotAnimator = GetComponent<Animator>();
	}
	
	public override void Move() {
		MoveTowardsPlayer(10, speed);
		Biting(biteDelay, biteDamage);
	}
	
	protected override void OnBite() {
		carrotAnimator.SetTrigger("Bite");
		SoundManager.Instance.Play(SoundManager.Instance.carrotBite);
		// Invoke("StopBite", 0.5f);
	}
	
	protected override void OnBiteEnd() {
		carrotAnimator.SetTrigger("StopBite");
	}
}
