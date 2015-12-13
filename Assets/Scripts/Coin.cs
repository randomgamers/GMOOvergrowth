using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour {

	Vector3 velocity;
	float relativeY = 0;

	float creationTime;

	void Start () {
		creationTime = Time.time;
		velocity = new Vector3(Random.RandomRange(-0.03f, 0.03f), 0.06f + Random.RandomRange(-0.03f, 0.03f), 0);
	}

	void Update () {
		Debug.Log("DDDE " + (Time.time - creationTime));
		if (Time.time - creationTime > 15) {
			Destroy(this.gameObject);
			return;
		}
		
		Vector3 pos = transform.position;
		pos.y += velocity.y;
		pos.x += velocity.x;
		relativeY += velocity.y;
		transform.position = pos;
		
		velocity.y -= 0.003f;
		
		if (relativeY < 0 && velocity.y < 0) {
			this.enabled = false;
		}
	}
}
