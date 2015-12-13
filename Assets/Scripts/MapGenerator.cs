using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Tuples;

public class MapGenerator : MonoBehaviour {

	//[Range(1,100)] public int seed;
	[Range(0,1)] public float wallPerc;
	
	[Range(0,1)] public float accesibleAreaPerc;
	
	public int maxEmptyTile = 3;
	
	[Range(50,10000)] public int width;
	[Range(50,10000)]public int height;
	[Range(1,10)]public int smoothingEpochs;
	int[,] map;
	
	int playerX, playerY;
	int gateX, gateY;
	
	public const int EMPTY = 0;
	public const int WALL = 1;
	public const int GATE = 2;	
	
	public float minimapConst = 2.0f;
	
	private int minimapWidth = 55;
	private int minimapHeight = 55;
	
	// public GameObject minimapBorder;
	
	public static List<GameObject> tiles = new List<GameObject>();
	
	private static MapGenerator instance;
	public static MapGenerator Instance {
		get {
			if (instance == null)
				instance = GameObject.FindGameObjectWithTag("Map").GetComponent<MapGenerator>();
			return instance;
		}
	}
	
	void Awake()
	{
		//DontDestroyOnLoad(gameObject);
	}
	
	public void CreateMap(int lvlNr)
	{
		if (lvlNr == 0 && GameManager.Instance.firstRun) {
			CreateTutorialMap();
			GameManager.Instance.firstRun = false;
			return;
		}
		
		
		SetDifficulty(lvlNr);
		width += 10; 
		height += 10; 
			
		Random.seed = (int) (Random.Range(int.MinValue, int.MaxValue));
		
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		map = new int[width, height];
		
		GameObject minimap = GameObject.FindGameObjectWithTag("Minimap");
		Camera minimapCamera = minimap.GetComponent<Camera>();
		// minimapCamera.orthographicSize = height / 2;
		// minimapCamera.transform.position = new Vector2(width / 2, height / 2);
		
		minimapCamera.rect = new Rect(1 - minimapConst * (float)minimapWidth / (float)Screen.width, 1 - minimapConst * (float)minimapHeight / (float)Screen.height, minimapWidth, minimapHeight);
		// minimapBorder.transform.position = new Vector2(minimapConst * (float)width, minimapConst * (float)height);
		
		// minimapBorder.transform.localScale = new Vector3(width, height, 1);
		// minimapBorder.transform.position = new Vector3(minimapCamera.rect.x * Screen.width, minimapCamera.rect.y * Screen.height, 5);
		
		// init map somehow validly
		GenerateAndSmooth(50);

		// move the player to the starting position
		player.transform.position = new Vector3((float) playerX, (float) playerY, player.transform.position.z);

		DestroyOldTiles();

		SpawnTiles();

		
		GameObject.FindGameObjectWithTag("Global").GetComponent<ItemGenerator>().GenerateItems(tiles, map, width, height);
		GameObject.FindGameObjectWithTag("Global").GetComponent<MonsterGenerator>().GenerateMonsters(tiles, map, width, height);
	}

	void CreateTutorialMap()
	{
		Random.seed = 1;
		
		int oldW = width;
		int oldH = height;
		
		width = 30;
		height = 30;
		
		GameObject minimap = GameObject.FindGameObjectWithTag("Minimap");
		Camera minimapCamera = minimap.GetComponent<Camera>();
		minimapCamera.rect = new Rect(1 - minimapConst * (float)minimapWidth / (float)Screen.width, 1 - minimapConst * (float)minimapHeight / (float)Screen.height, minimapWidth, minimapHeight);
		
		map = new int[width, height];
		GenerateAndSmooth(50);
		SpawnTiles();
		
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = new Vector3(4f, 4f, player.transform.position.z);
		
		//AddGate(25,25);
		
		
		Object chest = Resources.Load("Prefabs/Items/Chest");
		GameObject chestGameObject = GameObject.Instantiate(chest) as GameObject;
		chestGameObject.GetComponent<Chest>().isChestCageable = false;
		chestGameObject.name = chest.name;
		chestGameObject.transform.position = new Vector3((float)7, (float)7 ,0f);
		tiles.Add(chestGameObject);

		GameObject carrot = GameObject.Instantiate(Resources.Load("Prefabs/Monsters/Carrot")) as GameObject;
		carrot.transform.position = new Vector3((float)17, (float)17 ,0f);
		tiles.Add(carrot);				
		
		
		minimapWidth = -1;
		minimapHeight = -1;
		width = oldW;
		height = oldH;
	}
	
	private GUIStyle guiStyle = new GUIStyle(); 

	void OnGUI()
	{
		if (GameManager.Instance.currentLevel == 0)
		{
			
			guiStyle.fontSize = 20;
			GUI.backgroundColor = Color.gray;
			guiStyle.normal.textColor = Color.white;
			guiStyle.wordWrap = true;
			guiStyle.alignment = TextAnchor.MiddleCenter;
			Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
			guiStyle.normal.background = texture;
			 
			
			Vector3 chestPos = new Vector3(7.0f, 7.0f, 5.0f);
			Vector3 gatePos = new Vector3(22.0f, 23.0f, 5.0f);
			
			Vector3 realChestPos = Camera.main.WorldToScreenPoint(new Vector2(chestPos.x, chestPos.y));
			Vector3 realGatePos = Camera.main.WorldToScreenPoint(new Vector2(gatePos.x, gatePos.y));
			
			GUI.Label(new Rect(realChestPos.x - 110, Screen.height-realChestPos.y+60, 220, 50), "Shoot the chest to get coins. Collect them!", guiStyle);
			GUI.Label(new Rect(realChestPos.x + 145, Screen.height-realChestPos.y+60, 250, 50), "The more coins you have the better your weapon is.", guiStyle);
			GUI.Label(new Rect(realChestPos.x - 50, Screen.height-realChestPos.y+250, 200, 70), "Find a way from the garden of more than grown vegetables.", guiStyle);
			GUI.Label(new Rect(realChestPos.x + 250, Screen.height-realChestPos.y+200, 200, 70), "Use LMB to shoot and RMB to dash. Can you make it on time?", guiStyle);
			GUI.Label(new Rect(realChestPos.x + 250, Screen.height-realChestPos.y-100, 200, 25), "Let's find the way out.", guiStyle);
			GUI.Label(new Rect(realChestPos.x + 500, Screen.height-realChestPos.y+100, 260, 50), "Be careful about using dash. It can get dangerous!", guiStyle);
			GUI.Label(new Rect(realChestPos.x + 650, Screen.height-realChestPos.y-400, 280, 70), "In the upper left corner you may see your stats. There’s a minimap in the right one.", guiStyle);
			GUI.Label(new Rect(realChestPos.x + 650, Screen.height-realChestPos.y-300, 280, 70), "Time is money!\nWhen you’re out of time, you pay for it with money.", guiStyle);
			
			switch (Random.Range(0, 3)) {
				case 0:
					GUI.backgroundColor = Color.blue;
					guiStyle.normal.textColor = Color.red;
					break;
				case 1:
					GUI.backgroundColor = Color.cyan;
					guiStyle.normal.textColor = Color.yellow;
					break;
				case 2:
					GUI.backgroundColor = Color.green;
					guiStyle.normal.textColor = Color.magenta;
					break;
			}
			GUI.Label(new Rect(realChestPos.x + 70, Screen.height-realChestPos.y-850, 280, 70), "iF y0u M3aT cAgE\nMoNEyz W1lL B3 hE4rTs", guiStyle);
			GUI.backgroundColor = Color.gray;
			guiStyle.normal.textColor = Color.white;
			
			GUI.Box(new Rect(realGatePos.x - 450, Screen.height-realGatePos.y+500, 250, 50), "If your health or time decreses to zero, you die.", guiStyle);
			GUI.Box(new Rect(realGatePos.x - 200, Screen.height-realGatePos.y+50, 400, 70), "This is the exit gate. Walk through it in order to get out. Have fun!", guiStyle);
			
			if (GameObject.FindGameObjectWithTag("Monster") != null) 
			{
				Vector3 monsterPos = GameObject.FindGameObjectWithTag("Monster").transform.position;
				Vector3 realMonsterPos = Camera.main.WorldToScreenPoint(new Vector2(monsterPos.x, monsterPos.y));
				GUI.Box(new Rect(realMonsterPos.x - 140, Screen.height - realMonsterPos.y + 45, 280, 50), "Beware, this the mutant carrot! Shoot this motherf*cker!", guiStyle);
			}	
		}
		
	}

	void SpawnTiles()
	{
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				if (map[x,y] == WALL) {
					AddRandomWallTile(x,y);
				} else if (map[x,y] == GATE) {
					AddGate(x,y);
				} else if (map[x,y] == EMPTY) {
					AddRandomEmptyTile(x,y);
				}
			}
		}
	}

	void AddRandomWallTile(int x, int y)
	{
		float roulette = Random.Range(0f, 1f);
		int wallType = 0;
		if (roulette < 0.33) wallType = 0; 
		else if (roulette < 0.66) wallType = 1; 
		else if (roulette < 1.0) wallType = 2; 

		GameObject wall = GameObject.Instantiate(Resources.Load("Prefabs/Tiles/WallTile" + wallType)) as GameObject;
		wall.transform.position = new Vector3((float)x, (float)y, 0f);
		tiles.Add(wall);		
	}

	void AddRandomEmptyTile(int x, int y)
	{
		float roulette = Random.Range(0f, 1f);
		int grassType = 0;
		if (roulette < 0.45) grassType = 0; 
		else if (roulette < 0.9) grassType = 1; 
		else if (roulette < 0.95) grassType = 2; 
		else if (roulette < 1.0) grassType = 3; 

		GameObject grass = GameObject.Instantiate(Resources.Load("Prefabs/Tiles/EmptyTile" + grassType)) as GameObject;
		grass.transform.position = new Vector3((float)x, (float)y, 0f);
		tiles.Add(grass);
	}

	void AddGate(int x, int y)
	{
		GameObject grass = GameObject.Instantiate(Resources.Load("Prefabs/Tiles/EmptyTile1")) as GameObject;
		grass.transform.position = new Vector3((float)x, (float)y, 0f);
		tiles.Add(grass);

		GameObject gate = GameObject.Instantiate(Resources.Load("Prefabs/Tiles/GateTile")) as GameObject;
		gate.transform.position = new Vector3((float)x, (float)y, 0f);
		tiles.Add(gate);
	}
	
	void GenerateAndSmooth(int limit)
	{
		while (true) {
			Debug.Log("Iter!");
			if (limit-- <= 0) {
				Debug.Log("Failed to generate cool map :(");
				break;
			}
			
			RandomInit();
			for (int e = 0; e < smoothingEpochs; e++) {
				Smooth();
			}
			
			do {	// set player pos
				playerX = Random.Range(1, 10);
				playerY = Random.Range(1, 10);
			} while (map[playerX, playerY] != EMPTY);
			
			gateX = Random.Range(width-10, width);
			gateY = Random.Range(height-10, height);
			int prev = map[gateX, gateY];
			map[gateX, gateY] = GATE;
			if (IsValid()) break;
			else map[gateX, gateY] = prev;
		}
	}
	void DestroyOldTiles()
	{
		foreach (GameObject tile in tiles) {
			Destroy(tile);
		}
	}
	
	bool IsValid()
	{
		bool gateFound = false;
		int accesableFields = 0;
		
		HashSet<Tuple<int, int>> visited = new HashSet<Tuple<int, int>>();
		Queue<Tuple<int, int>> q = new Queue<Tuple<int, int>>();
		
		q.Enqueue(Tuple.Create(playerX, playerY));
		
		while (q.Count > 0) {
			Tuple<int, int> currentPos = q.Dequeue();
			if (visited.Contains(currentPos)) continue;
			if (currentPos.Item1 < 0 || currentPos.Item1 >= width || currentPos.Item2 < 0 || currentPos.Item2 >= height) continue;
			
			visited.Add(currentPos);

			if (map[currentPos.Item1, currentPos.Item2] == EMPTY) {
				accesableFields++;
			} else if (map[currentPos.Item1, currentPos.Item2] == GATE) {
				gateFound = true;
			}

			if (map[currentPos.Item1, currentPos.Item2] != WALL) {
				q.Enqueue(Tuple.Create(currentPos.Item1-1, currentPos.Item2));
				q.Enqueue(Tuple.Create(currentPos.Item1+1, currentPos.Item2));
				q.Enqueue(Tuple.Create(currentPos.Item1, currentPos.Item2-1));
				q.Enqueue(Tuple.Create(currentPos.Item1, currentPos.Item2+1));
			}
		}
		
		return gateFound && (((float) accesableFields)/(width * height) > accesibleAreaPerc);
	}
	
	void RandomInit()
	{
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				if (x == 0 || y == 0 || x == width-1 || y == height-1) map[x,y] = WALL;
				map[x,y] = (Random.Range(0f,1f) < wallPerc) ? WALL : EMPTY;
			}
		}
	}
	
	void Smooth() {
		int[,] nextMap = new int[width, height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				if (x == 0 || y == 0 || x == width-1 || y == height-1) {
					nextMap[x,y] = WALL;
				} else {
					int neighs = 0;
	
					for (int i = -1; i <= 1; i++) {
						for (int j = -1; j <= 1; j++) {
							if (i == 0 && j == 0) continue;
							neighs += map[x+i, y+j];
						}
					}
					
					nextMap[x,y] = (neighs > 4) ? WALL : EMPTY;
				}
			}	
		}
		map = nextMap;
	}
	
	public void UpdateMinimap(Vector2 point)
	{
		
	}
	
	public bool isInBush(float x, float y)
	{
		if (x < 0 || y < 0 || x >= width || y >= height) return true;
		return map[(int)(x+0.5f),(int)(y+0.5f)] == WALL;
	}
	
	protected void SetDifficulty(int level) {
		LevelProgression.SetDifficulty(level);
	}
}
