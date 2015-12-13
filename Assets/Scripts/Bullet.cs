using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public Vector2 velocity;
	public bool friendly;
	
	private float shadowStart;
	private bool isShadow = false;
	
	Vector2 startPos;
	
	void Start() {
		startPos = this.Position2D();
		
		gameObject.layer = friendly ? LayerMask.NameToLayer("FriendlyBullets") : LayerMask.NameToLayer("EnemyBullets");
		GetComponent<Renderer>().sortingLayerName = "Bullets";
		SetSortingLayer.SetZ(this.gameObject);
		Vector3 eulAngles = transform.eulerAngles;
		eulAngles.z += Utils.AngleBetween(velocity, new Vector2(1, 0));
		transform.eulerAngles = eulAngles;
		//Debug.Log("Bullet created! " + gameObject.layer + " " + GetComponent<Renderer>().sortingLayerName);
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector2.Distance(this.Position2D(), startPos) > 12) {
			Destroy(this.gameObject);
		}
		
		if (isShadow) {
			// #spinnercarrots
			if (Time.time - shadowStart > 0.05)
				Destroy(this.gameObject);
		}
		Vector3 newPos = this.Position() + new Vector3(velocity.x, velocity.y, 0);
		Rigidbody2D r = GetComponent<Rigidbody2D>();
		r.MovePosition(newPos);
		
		MapGenerator mapGenerator = GameObject.FindGameObjectWithTag("Map").GetComponent<MapGenerator>();
		if (!newPos.IsInBounds(0, 0, mapGenerator.width, mapGenerator.height)) {
			Destroy(this.gameObject, 0);
		}
	}
	
	void OnCollisionEnter2D(Collision2D collision) {
		if (isShadow)
			return;
		
		Collider2D other = collision.collider;
		if (other.tag == "Player" && !friendly) {
			Debug.Log("Player hit!");
			SoundManager.Instance.Play(SoundManager.Instance.hit);

			GameManager.Instance.Hurt(5);
			Destroy(this.gameObject);
		} else if (other.tag == "Monster" && friendly) {
			Debug.Log("hitt monster " + GetComponent<Bullet>() + " " + collision.contacts[0]);
			other.gameObject.GetComponent<Monster>().BulletHit(GetComponent<Bullet>(), collision.contacts[0]);
			isShadow = true; shadowStart = Time.time;
			GetComponent<Renderer>().enabled = false;
			//Destroy(this.gameObject);
		} else if (other.gameObject.name == "Chest" && friendly) {
			other.GetComponent<Chest>().Destroy();
			Destroy(this.gameObject);
		} else {
			Destroy(this.gameObject);
		}
	}
}
