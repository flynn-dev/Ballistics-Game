using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public GameControl gameController;
	public GameObject playerControl;

	public float runSpeed = 40f;
	float horizontalMove = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(gameController.enemyTurn || Input.GetAxisRaw ("Horizontal") == 0 || playerControl.GetComponent<Player> ().fuel <= 0) {	
			playerControl.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePositionX;
		} else if (!gameController.enemyTurn && playerControl.GetComponent<Player> ().fuel > 0) {
			playerControl.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
			horizontalMove = Input.GetAxisRaw ("Horizontal") * runSpeed;
			playerControl.GetComponent<Player> ().fuel--;

		}
	}

	void FixedUpdate () {
		if (!gameController.enemyTurn) {
			controller.Move (horizontalMove * Time.fixedDeltaTime, false, false);
		}
	}
}
