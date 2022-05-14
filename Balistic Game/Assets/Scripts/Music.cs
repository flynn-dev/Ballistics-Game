using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

	private bool enabled = true;

	//this code makes the game object persist between scene changes, so that the music doesnt stop when you go from the game to the menu
	private static Music s_Instance = null;

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

	public void toggleMusic () {
		if (enabled) {
			gameObject.GetComponent<AudioSource> ().Pause ();
			enabled = false;
		} else {
			gameObject.GetComponent<AudioSource> ().UnPause ();
			enabled = true;
		}
	}
}
