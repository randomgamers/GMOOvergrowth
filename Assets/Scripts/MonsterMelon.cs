using UnityEngine;
using System.Collections;

public class MonsterMelon : Monster {

	public static int lives = 5;
	public static float speed;
	public static float rage_speed {
		get {
			return GameManager.Level <= 5 ? 0.05f : 0.08f;
		}
	}
	public static float hitTime = 0;
	private Animator melonAnimator;

	// Use this for initialization
	void Awake() {
		_lives = lives;
		nCoins = 4;
		coinRange = 0.8f;

		Debug.Log("Melon lives " + lives);
		melonAnimator = GetComponent<Animator>();
	}
	
	public override void BulletHit(Bullet bullet, ContactPoint2D contact) {
		base.BulletHit(bullet, contact);
		hitTime = Time.time;
	}
	
	public override void Move() {
		if (Time.time - hitTime < 1.2f) {
			speed = 0f;
		} else {
			speed = rage_speed;
		}
		MoveTowardsPlayer(7, speed);
		Biting(1.0f, 20);
	}
	
	protected override void OnBite() {
		melonAnimator.SetTrigger("Bite");
		SoundManager.Instance.Play(SoundManager.Instance.melonBite);
		// Invoke("StopBite", 0.5f);
	}
	
	protected override void OnBiteEnd() {
		melonAnimator.SetTrigger("StopBite");
	}
}
