using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMusic : MonoBehaviour {

	private bool enabled;
	public Music musicObject;

	//this code makes the game object persist between scene changes, so that the music doesnt stop when you go from the game to the menu
	private static DisableMusic s_Instance = null;

	void Awake()
	{
		if (s_Instance == null)
		{
			s_Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}



	void OnMouseDown () {
		musicObject.toggleMusic ();
	}
}
