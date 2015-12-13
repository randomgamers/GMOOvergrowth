using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ItemGenerator : MonoBehaviour {

	[Range(0,1)] public float chest3prob;
	[Range(0,1)] public float heartProb;

	public void GenerateItems(List<GameObject> objectContainer, int[,] map, int width, int height)
	{
		GenerateChests(objectContainer, map, width, height);
		GenerateHearts(objectContainer, map, width, height);
	}

	void GenerateChests(List<GameObject> objectContainer, int[,] map, int width, int height)
	{
		Object chest = Resources.Load("Prefabs/Items/Chest");
		
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				if (map[x,y] != MapGenerator.EMPTY) continue;
				
				int neighs = 0;

				for (int i = -1; i <= 1; i++) {
					for (int j = -1; j <= 1; j++) {
						if (i == 0 && j == 0) continue;
						neighs += map[x+i, y+j];
					}
				}
				
				if (neighs >= 5) {
					if (Random.Range(0f, 1f) < chest3prob) {
						GameObject chestGameObject = GameObject.Instantiate(chest) as GameObject;
						chestGameObject.name = chest.name;
						chestGameObject.transform.position = new Vector3((float)x, (float)y ,0f);
						objectContainer.Add(chestGameObject);
					}
				}
			}
		}
	}

	void GenerateHearts(List<GameObject> objectContainer, int[,] map, int width, int height)
	{
		Object heart = Resources.Load("Prefabs/Items/Heart");
		
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				if (map[x,y] != MapGenerator.EMPTY) continue;
				
				int neighs = 0;

				for (int i = -1; i <= 1; i++) {
					for (int j = -1; j <= 1; j++) {
						if (i == 0 && j == 0) continue;
						neighs += map[x+i, y+j];
					}
				}
				
				if (neighs >= 5) {
					if (Random.Range(0f, 1f) < heartProb) {
						GameObject heartGameObject = GameObject.Instantiate(heart) as GameObject;
						heartGameObject.name = heart.name;
						heartGameObject.transform.position = new Vector3((float)x, (float)y ,0f);
						objectContainer.Add(heartGameObject);
					}
				}
			}
		}
	}


}
