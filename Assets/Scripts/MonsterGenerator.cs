using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterGenerator : MonoBehaviour {

	public float monsterProb {
		get {
			return 0.02f + (GameManager.Level - 1) * 0.01f;
		}
	}

	public void GenerateMonsters(List<GameObject> objectContainer, int[,] map, int width, int height)
	{
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				if (map[x,y] != MapGenerator.EMPTY) continue;
				
				if (Random.Range(0f, 1f) < monsterProb) {
					
					float roulette = Random.Range(0f, 1f);
					string mType = "Carrot";
					
					if (roulette < 0.07) mType = "Melon";
					else if (roulette < 0.45) mType = "Corn";
					else if (roulette <= 1.0) mType = "Carrot";
					
					GameObject carrot = GameObject.Instantiate(Resources.Load("Prefabs/Monsters/" + mType)) as GameObject;
					carrot.transform.position = new Vector3((float)x, (float)y ,0f);
					objectContainer.Add(carrot);				
				}

			}
		}
	

	}
}
