using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireButton : MonoBehaviour {

	public GameControl gameController;
	public Player player;

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

	void OnMouseDown () {
		if (gameController.playerTurn) {
			player.Fire ();
		}
	}
}
