using UnityEngine;
using System.Collections;

public class Death2Intro : MonoBehaviour {

	public void GoToIntro()
	{
		Debug.Log("getting back to intro splash");
		Application.LoadLevel("Scenes/IntroScene");
	}
}
