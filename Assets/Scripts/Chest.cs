using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	protected int nCoins = 5;
	protected float coinRange = 0.1f;

	public bool isChestCageable = false;

	bool showCage = false;
	bool moveUp = true;

	int iter = 0;
	const int peek = 75;
	
	const float animSpeed = 0.01f;
	const float cageY =  -0.1378f;

	GameObject cageImg;

	float cageProb()
	{
		return 0.1f + GameManager.Instance.currentLevel * 0.05f; 
	}

	void Start() 
	{
		isChestCageable = Random.Range(0f, 1f) < cageProb();
		Debug.Log("Cagable: " + isChestCageable);
	}


	public void Destroy() {
		
		if (isChestCageable) {
			if (showCage) return;
			
			SoundManager.Instance.Play(SoundManager.Instance.cage);
			
			showCage = true;
			moveUp = true;
			iter = 0;
			cageImg = GameObject.Instantiate(Resources.Load("Prefabs/Cage")) as GameObject;
			Vector3 thisPos = gameObject.transform.position;
			cageImg.transform.position = new Vector3(thisPos.x, thisPos.y + cageY, thisPos.z);

			if (GameManager.Instance.money >= 10) {
				GameManager.Instance.Heal(5);
				GameManager.Instance.PayMoney(20);
			} else {
				GameManager.Instance.money = 0;
				GameManager.Instance.Hurt(5);
			}
			
		} else {
			for (int i = 0; i < nCoins; ++i) {
				var coin = Utils.InstantiatePrefab(
					"Items/Coin",
					this.Position().x + Random.Range(-coinRange, coinRange),
					this.Position().y + Random.Range(-coinRange, coinRange),
					"Items");
				MapGenerator.tiles.Add(coin);
			}
			Destroy(this.gameObject);
		}
	}
	
	void Update()
	{
		if (showCage) {

			float dy = 0;
			if (moveUp) {
				if (iter == peek) {
					iter = 0;
					moveUp = false;
				}
				dy = 1;
			} else {
				if (iter == peek) {
					showCage = false;
					Destroy(cageImg);
				}
				dy = -1;
			}
			Vector3 cagePos = cageImg.transform.position;
			cageImg.transform.position = new Vector3(cagePos.x, cagePos.y + dy*animSpeed, cagePos.z);
		iter++;
		}
		
	}
}
