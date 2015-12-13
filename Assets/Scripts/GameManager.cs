using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	//public GameObject splashScreen;
	//public Text splashText;
	public Text textBar;
	
	//public GameObject splashImage;

	public int currentLevel;

	public bool firstRun = true;

	public float splashScreenDelay;

	public int timeLimit;
	int remainingTime;
	
	float lvlStart;
	
	public static int Level {
		get {
			return Instance.currentLevel;
		}
	}

	private static GameManager instance;
	public static GameManager Instance {
		get {
			if (instance == null)
				instance = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
			return instance;
		}
	}

	public bool isSplashOn = false;
	public bool gamePaused = true;

	bool genererreateNewMap;
	
	public void InitGame()
	{
		currentLevel++;
		Debug.Log("Init Game");
		//ResetStats();
		genererreateNewMap = false;

		remainingTime = timeLimit;
		lvlStart = Time.time;
		gamePaused = false;
	}

	float lastTimeCheck;

	void Awake() {
		if (instance == null) {
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		

		lastTimeCheck = Time.time;
		InitGame();
	}

	void Start () {
		// Don't put anything in here
		Debug.Log("GM started");
		ResetStats();
	}
		
	public void UpdateTextBar()
	{
		if (textBar == null) {
			if (GameObject.FindGameObjectWithTag("TextBar") != null)
				textBar = GameObject.FindGameObjectWithTag("TextBar").GetComponent<Text>();
			else return;
		}
		
		string nextUpgrade = "inf";
		foreach (int upgrade in ActionController.Instance.UpgradeLevels) {
			if (upgrade > money) {
				nextUpgrade = upgrade.ToString();
				break;
			}
		}
		
		
		textBar.text =  "HP: " + hp + "\n" +
						"$" + money + " / " + nextUpgrade + "\n" +
						"Time: " + remainingTime + "\n" + 
						"Level: " + currentLevel;	  
	}

	void Update() 
	{
		if (!genererreateNewMap) {
			Debug.Log("generating shit");
			if (GameObject.FindGameObjectWithTag("Map") != null && GameObject.FindGameObjectWithTag("Map").GetComponent<MapGenerator>()) {
				GameObject.FindGameObjectWithTag("Map").GetComponent<MapGenerator>().CreateMap(currentLevel);
				genererreateNewMap = true;
			} else return;
		}

		if (gamePaused) return;

		int lvlElapsedTime = (int) (Time.time - lvlStart);
		remainingTime = timeLimit - lvlElapsedTime;
		if (remainingTime <= 0) {
			if (money >= 0) {
				if (Time.time - lastTimeCheck > 1) {
					PayMoney(1);
					lastTimeCheck = Time.time;
				}
				remainingTime = 0;
			} else {
				GameOver("time");
			}	
		}
		UpdateTextBar();
	}

	public void GameOver(string reason = "just so")
	{
		gamePaused = true;
		Debug.Log("You died because of: " + reason);
		SoundManager.Instance.Play(SoundManager.Instance.death);
		GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("Death");
		GameObject.FindGameObjectWithTag("PlayerSprite").GetComponent<SpriteRenderer>().sprite = Sprite.Create(Resources.Load<Texture2D>("Materials/Textures/Minimap/player_minimap_head_baw"), new Rect(0, 0, 25, 19), new Vector2(0.5f, 0.5f), 8);
		if (currentLevel == 0) GameManager.Instance.firstRun = true;
		Invoke("LoadScreenOfDeath", 3.0f);

		//isSplashOn = true;
	}
	
	void LoadScreenOfDeath()
	{
		Debug.Log("screen of death, nigga");
		Application.LoadLevel("Scenes/DeathScene");
	}

	[HideInInspector] public int hp;
	
	[Range(1,10000)] public int maxHP;
	
	[HideInInspector] public int money = 0;

	public void ResetStats()
	{
		hp = maxHP - 10;
		money = 0;
		if (firstRun) currentLevel = 0;
		else currentLevel = 1;
		UpdateTextBar();
	}

	public void Heal(int amount)
	{
		hp = Mathf.Min(hp + amount, maxHP);
		UpdateTextBar();
	}

	public void Hurt(int amount)
	{
		hp = Mathf.Max(hp - amount, 0);
		UpdateTextBar();
		if (hp <= 0) GameOver("HP");
	}
	
	public void RestoreFullHealt()
	{
		hp = maxHP;
		UpdateTextBar();
	}

	public void GainMoney(int amount)
	{
		money += amount;
		UpdateTextBar();
	}
	
	public void PayMoney(int amount)
	{
		money = Mathf.Max(money - amount, 0);
		UpdateTextBar();
	}



}
