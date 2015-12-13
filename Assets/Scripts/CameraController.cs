using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject map;
	public GameObject player;
	
	void Awake() {
		//DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos = player.transform.position;
		float minWidth = Camera.main.orthographicSize * Camera.main.aspect - 0.5f;
		float minHeight = Camera.main.orthographicSize - 0.5f;
		float maxWidth = map.GetComponent<MapGenerator>().width - minWidth - 1.0f;
		float maxHeight = map.GetComponent<MapGenerator>().height - minHeight - 1.0f;
		
		pos.x = Mathf.Clamp(pos.x, minWidth, maxWidth);
		pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);
		
		transform.position = pos;
	}
}
