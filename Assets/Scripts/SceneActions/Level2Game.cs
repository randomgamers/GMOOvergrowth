using UnityEngine;
using System.Collections;

public class Level2Game : MonoBehaviour {

	public void ContinueNextLevel()
	{
		Debug.Log("next level");
		Application.LoadLevel("Scenes/GameScene");
		GameManager.Instance.InitGame();
	}
}
