using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	public int _lives;
	
	protected int nCoins = 1;
	protected float coinRange = 0.4f;
	
	// Update is called once per frame
	void Update () {
		if (GameManager.Instance.isSplashOn)
			return;
		Move();
	}
	
	public virtual void Move() {
		// Do nothing
	}
	
	public virtual void BulletHit(Bullet bullet, ContactPoint2D contact) {
		Debug.Log("Hit Monster with " + _lives + "");
		--_lives;
		GetComponent<Rigidbody2D>().AddForceAtPosition(
			bullet.velocity.Scale(10f), contact.point
		);
		if (_lives == 0) {
			OnDie();
			Destroy(this.gameObject);
		}
	}
	
	
	protected virtual void OnDie()
	{
		SoundManager.Instance.Play(SoundManager.Instance.enemyDeath);

		for (int i = 0; i < nCoins; i++) {
			GameObject coin = GameObject.Instantiate(Resources.Load("Prefabs/Items/Coin")) as GameObject;
			coin.transform.position = new Vector3((float)gameObject.Position().x + Random.Range(-coinRange, coinRange), (float)gameObject.Position().y + Random.Range(-coinRange, coinRange), coin.Position().z);
			MapGenerator.tiles.Add(coin);
		}
	}
	
	// METHODS TO BE USED BY MONSTERS FROM NOW ON
	protected void MoveTowardsPlayer(float threshold, float speed) {
		Player player = Player.Instance;
		if (Vector3.Distance(player.Position2D(), this.Position2D()) < threshold) {
			Vector3 newPos = Vector3.MoveTowards(this.Position(), player.Position(), speed);
			Rigidbody2D r = GetComponent<Rigidbody2D>();
			Vector2 force = (newPos - this.Position()).AsVector2();
			
			r.MovePosition(newPos);
		}
	}
	
	private bool biting = false;
	private float biteStart;
	protected void Biting(float bitingTime, int damage) {
		Player player = Player.Instance;
		if (biting == false && Vector2.Distance(player.Position2D(), this.Position2D()) < 1.2) {
			// start biting
			OnBite();
			biting = true;
			biteStart = Time.time;
		}
		if (biting) {
			if (Time.time - biteStart > bitingTime) {
				OnBiteEnd();
				biting = false;
				if (Vector2.Distance(player.Position2D(), this.Position2D()) < 1.2) {
					Debug.Log("You have been bitten by a carrot!");
					GameManager.Instance.Hurt(damage);
				}
			}
		}
	}
	
	protected virtual void OnBite() { }
	protected virtual void OnBiteEnd() { }
	
}
