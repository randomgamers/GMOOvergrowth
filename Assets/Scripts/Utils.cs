using UnityEngine;
using System.Collections;

public static class Utils {
	private static float maxRangeDistance = 50.0f;
	public static bool ObjectInRange(GameObject go) {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		return Vector2.Angle(go.transform.position - player.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position) < maxRangeDistance;
	}
	
	public static Vector3 Position (this GameObject gameObject) {
		return gameObject.transform.position;
	}
	
	public static Vector3 Position (this MonoBehaviour behaviour) {
		return behaviour.transform.position;
	}
	
	public static Vector2 Position2D (this GameObject gameObject) {
		return gameObject.transform.position.AsVector2();
	}
	
	public static Vector2 Position2D (this MonoBehaviour behaviour) {
		return behaviour.transform.position.AsVector2();
	}
	
	public static bool IsInBounds (this Vector3 vector, float minX, float minY, float xLimit, float yLimit) {
		return vector.x >= minX && vector.x < xLimit &&
				vector.y >= minY && vector.y < yLimit; 
	}
	
	public static Vector2 AsVector2(this Vector3 vector) {
		return new Vector2(vector.x, vector.y);
	}
	
	public static Vector3 AsVector3(this Vector2 vector) {
		return new Vector3(vector.x, vector.y, 0);
	}
	
	public static Vector3 AsVector3(this Vector2 vector, float z) {
		return new Vector3(vector.x, vector.y, z);
	}
	
	public static Vector2 Scale(this Vector2 vector, float mul) {
		return vector * mul;
	}
	
	public static Vector3 Scale2D(this Vector3 vector, float mul) {
		vector.Scale(new Vector3(mul, mul, 0));
		return vector;
	}
	
	public static GameObject InstantiatePrefab(string path, float x, float y, string layer) {
		GameObject gameObject = GameObject.Instantiate(Resources.Load("Prefabs/" + path)) as GameObject;
		gameObject.transform.position = new Vector3(x,y,0f);
		
		gameObject.GetComponent<Renderer>().sortingLayerName = "Items";
		SetSortingLayer.SetZ(gameObject);
		return gameObject;
	}
	
	public static float AngleBetween(Vector2 a, Vector2 b) {
		float ang = Vector2.Angle(a, b);
		Vector3 cross = Vector3.Cross(a, b);
 
 		if (cross.z > 0)
     		ang = 360 - ang;
			 
		return ang;
	}
}
