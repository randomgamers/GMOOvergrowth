using UnityEngine;
using System.Collections.Generic;

public class MoveController : MonoBehaviour {
	public float speed = 0.01f;
	private float maxSpeed = 5f;
	public Animator playerAnimator;
	
	private Vector3 oldPosition;
	void Update() {
		if (GameManager.Instance.gamePaused) {
			Rigidbody2D rig = GetComponent<Rigidbody2D>();
			rig.angularDrag = 0;
			rig.isKinematic = true;
			return;
		}

		if (GameManager.Instance.isSplashOn)
			return;
		
		
		Vector2 relativePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		
		float mgn = relativePosition.magnitude;
		if (mgn > maxSpeed) {
			float mul = maxSpeed / mgn;
			relativePosition *= mul;
		}
		
		relativePosition.Scale(new Vector2(speed, speed));
		
		
		if (ActionController.Instance.dashing) {
			Vector2 dashDirection = ActionController.Instance.dashDirection;
			float dashSpeed = ActionController.Instance.dashSpeed;
			
			relativePosition = dashDirection.Scale(dashSpeed);
			Debug.Log("DASH " + relativePosition);
		}
		
		Rigidbody2D r = GetComponent<Rigidbody2D>();
		r.MovePosition(r.position + relativePosition);
		
		float speedPerSec = Vector3.Distance (oldPosition, transform.position) / Time.deltaTime;
 		float mySpeed = Vector3.Distance (oldPosition, transform.position);
 		oldPosition = transform.position;
		
		if (mySpeed > 0.02f) {
			playerAnimator.SetBool("Animation", true);
		} else {
			playerAnimator.SetBool("Animation", false);
		}
		
		UpdateSprite();
		
		// GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		
		// mainCamera.transform.position = transform.position;
		
		// Rigidbody2D r = GetComponent<Rigidbody2D>();
		// r.MovePosition(new Vector2 (r.position.x + movex * speed, r.position.y + movey * speed));
		
	}
	
	void UpdateSprite() {
		Vector2 v = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		if (v.x > v.y) {
			if (v.x > -v.y) {
				playerAnimator.SetInteger("Direction", 1);
				// playerRenderer.material = playerMaterials[1];
			} else {
				playerAnimator.SetInteger("Direction", 2);
				// playerRenderer.material = playerMaterials[2];
			}
		} else {
			if (v.x > -v.y) {
				playerAnimator.SetInteger("Direction", 0);
				// playerRenderer.material = playerMaterials[0];
			} else {
				playerAnimator.SetInteger("Direction", 3);
				// playerRenderer.material = playerMaterials[3];
			}
		}
	}
}
