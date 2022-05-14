using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wintext2 : MonoBehaviour {

	public GameControl gamecontroller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (gamecontroller.win == true) {
			gameObject.transform.position = new Vector3 (0f, 13.56f, -2.58f);
		}
	}
}
