using UnityEngine;
using System.Collections;
using System.Reflection;

public class SetSortingLayer : MonoBehaviour {
	
	public string layerName;
	
	public static string[] GetSortingLayerNames() {
         /*System.Type internalEditorUtilityType = typeof(InternalEditorUtility);
         PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
         return (string[])sortingLayersProperty.GetValue(null, new object[0]);*/
		 return new string[] {"Default", "Cage", "Chest", "Bullets", "Items", "Enemies", "Coin", "Player", "Dungeon"};
    }
	
	public static void SetZ(GameObject gameObject) {
		string[] sortingLayers = GetSortingLayerNames();
		for (int i = 0; i < sortingLayers.Length; ++i) {
			if (sortingLayers[i] == gameObject.GetComponent<Renderer>().sortingLayerName) {
				//Debug.Log("gameObject " + this + " is " + layerName);
				gameObject.transform.position = new Vector3(
					gameObject.transform.position.x,
					gameObject.transform.position.y,
					1 + i);
				break;
			}
		}
	}
	
	void Start () {
		gameObject.GetComponent<Renderer>().sortingLayerName = layerName;
		SetZ(this.gameObject);
		//Renderer renderer = gameObject.GetComponent<Renderer>();
		//renderer.sortingLayerName = layerName;
		//renderer.sortingOrder = 0;
	}
}
