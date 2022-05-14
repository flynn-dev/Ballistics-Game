using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this script controls the buttons that rotate the cannon
public class RotationButton : MonoBehaviour {

	public GameControl gameController;
	public int direction; //direction that the button rotates the cannon. 0 is left, 1 is right
	public Player player;
	bool mouseDown = false;

	public Material startColor;
	public Material mouseOverColor;
	Renderer rend;

	void OnMouseOver()
	{
		if (gameController.playerTurn) {
			rend.sharedMaterial = mouseOverColor;
		}
	}

	void OnMouseExit()
	{
		rend.sharedMaterial = startColor;
	}

	// Start is called before the first frame update
	void Start()
	{
		rend = GetComponent<Renderer>();
		rend.enabled = true;
		rend.sharedMaterial = startColor;
	}

	void Update () {
		if (Input.GetMouseButton (0) && mouseDown && gameController.playerTurn) {
			player.RotateCannon (direction);
		}
	}

	void OnMouseDown () {
		mouseDown = true;
	}

	void OnMouseUp () {
		mouseDown = false;
	}

}
