using UnityEngine;
using System.Collections;

public class MinimapController : MonoBehaviour {
	public GameObject map;
	public GameObject player;
	
	// Update is called once per frame
	void Update () {
		Camera camera = GetComponent<Camera>();
		
		Vector2 pos = player.transform.position;
		float minWidth = camera.orthographicSize * camera.aspect - 0.5f;
		float minHeight = camera.orthographicSize - 0.5f;
		float maxWidth = map.GetComponent<MapGenerator>().width - minWidth - 1.0f;
		float maxHeight = map.GetComponent<MapGenerator>().height - minHeight - 1.0f;
		
		pos.x = Mathf.Clamp(pos.x, minWidth, maxWidth);
		pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);
		
		transform.position = pos;
	}
}
