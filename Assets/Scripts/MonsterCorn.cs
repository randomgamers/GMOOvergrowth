using UnityEngine;
using System.Collections;

public class MonsterCorn : Monster {

	public static int lives {
		get {
			return GameManager.Level <= 3 ? 1 : 2;
		}
	}
	float lastShot = 0;
	public static float bulletSpeed {
		get {
			return GameManager.Level <= 6 ? 0.1f : 0.2f;
		}
	}
	public static float speed {
		get {
			return GameManager.Level <= 4 ? 0.003f : 0.01f;
		}
	}
	
	// Use this for initialization
	void Awake() {
		_lives = lives;
		nCoins = 2;
	}
	
	public override void Move() {
		MoveTowardsPlayer(10, 0.003f);
		// Do not move
		if (Time.time - lastShot > 2.0f &&
			Vector3.Distance(Player.Instance.transform.position, this.transform.position) < 10) {
			
			lastShot = Time.time;
			
			GameObject gameObject = GameObject.Instantiate(Resources.Load("Prefabs/Weapons/CornBullet")) as GameObject;
			Bullet bullet = gameObject.GetComponent<Bullet>();
			bullet.transform.position =  new Vector3(this.Position().x, this.Position().y, bullet.Position().z);
			
			Vector2 relativePosition = Player.Instance.Position().AsVector2() - this.Position().AsVector2();
			relativePosition.Normalize();
			bullet.velocity = relativePosition.Scale(bulletSpeed);
			bullet.friendly = false;
		}
	}
}
