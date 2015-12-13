using UnityEngine;
using System.Collections;

public class Intro2Game : MonoBehaviour {

	public void StartTutorial()
	{
		Debug.Log("tutorial started");
		Application.LoadLevel("Scenes/GameScene");
		GameManager.Instance.InitGame();
		GameManager.Instance.ResetStats();
	}
}
