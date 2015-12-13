using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ActionController : MonoBehaviour {
	
	static ActionController instance;
	public static ActionController Instance {
		get {
			if (instance == null)
				instance = (ActionController)GameObject.FindObjectOfType(typeof(ActionController));
			return instance;
		}
	} 


	// Controls player actions
	public List<int> UpgradeLevels = new List<int>(){30, 80, 200};

	float fireDelay {
		get {
			int money = GameManager.Instance.money;
			if (money < UpgradeLevels[0] || (money >= UpgradeLevels[1] && money < UpgradeLevels[2])) {
				return 0.5f;
			}
			// submachine gun (solves up to 600 carrots a minute)
			return 0.2f;
		}
	} 
	float lastShot = 0;
	
	const float dashDelay = 1.5f;
	float dashDuration = 0.04f;
	public float dashSpeed = 0.0000001f;
	public Vector2 dashDirection;
	public bool dashing = false;
	float lastDash = 0;
	
	// Update is called once per frame
	void Update () {
		if (GameManager.Instance.gamePaused) return;
		
		//Debug.Log("Input " + Input.GetMouseButton(0) + "Time - lastShot " + (Time.time - lastShot));
		if (Input.GetButton("Shoot") && Time.time - lastShot > fireDelay) {
			//Debug.Log("FIRE!");
			lastShot = Time.time;
			FireWeapon();
		}
		if (Input.GetButton("Dash") && Time.time - lastDash > dashDelay) {
			//Debug.Log("DASH!");
			lastDash = Time.time;
			
			SoundManager.Instance.Play(SoundManager.Instance.dash);
			Vector2 relativePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
			relativePosition.Normalize();
			dashDirection = relativePosition;
			dashing = true;
		}
		if (dashing) {
			if (Time.time - lastDash > dashDuration) {
				dashing = false;
			}
		}
	}
	
	
	void FireWeapon() {
		int money = GameManager.Instance.money;
		Vector2 baseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		baseDir.Normalize();
		GenerateBullet(baseDir);
		if (money >= UpgradeLevels[1]) {
			SoundManager.Instance.Play(SoundManager.Instance.shotgun);			
			for (int i = 0; i < 3; ++i) {
				Vector2 dir = baseDir;
				dir.x += Random.Range(-0.5f, 0.5f);
				dir.y += Random.Range(-0.5f, 0.5f);
				dir.Normalize();
				dir.Scale(Random.Range(0.6f, 1.4f));
				GenerateBullet(dir);
			}
		} else {
			SoundManager.Instance.Play(SoundManager.Instance.shoot);			
		}
	}
	
	void GenerateBullet(Vector2 direction) {
		GameObject gameObject = GameObject.Instantiate(Resources.Load("Prefabs/Weapons/Bullet")) as GameObject;
		Bullet bullet = gameObject.GetComponent<Bullet>();
		bullet.transform.position = Player.Instance.Position();
		
		bullet.velocity = direction.Scale(0.5f);
		bullet.friendly = true; 
	}
	
}
