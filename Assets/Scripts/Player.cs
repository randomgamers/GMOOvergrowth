using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private static Player instance;
	public static Player Instance {
		get {
			if (instance == null)
				instance = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
			return instance;
		}
	}
	
	void Update()
	{
		if (GameManager.Instance.gamePaused) return;

		if (MapGenerator.Instance.isInBush(gameObject.Position().x, gameObject.Position().y)) {
			GameManager.Instance.GameOver("Cage");
		}
	}
}
