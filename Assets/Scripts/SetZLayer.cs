using UnityEngine;
using System.Collections;

public class SetZLayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position += new Vector3(0, 0, gameObject.layer);
	}
}
