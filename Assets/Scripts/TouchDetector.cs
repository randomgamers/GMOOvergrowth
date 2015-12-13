using UnityEngine;
using System.Collections;

public class TouchDetector : MonoBehaviour {	
	void OnTriggerEnter2D(Collider2D trigger) {
		if (trigger.tag.Equals("Gate")) {
			Application.LoadLevel("Scenes/LevelScene");
		} else if (trigger.tag.Equals("Heart")) {
			SoundManager.Instance.Play(SoundManager.Instance.health);
			GameManager.Instance.Heal(10);
			Destroy(trigger.gameObject);		
		} else if (trigger.tag.Equals("Coin")) {
			SoundManager.Instance.Play(SoundManager.Instance.coin);
			GameManager.Instance.GainMoney(1);
			Destroy(trigger.gameObject);		
		}
	}
}
