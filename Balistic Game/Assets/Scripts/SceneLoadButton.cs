using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadButton : MonoBehaviour {

	public int scene;
	public Material startColor;
	public Material mouseOverColor;
	Renderer rend;
	public AudioSource buttonSource;

	// Use this for initialization
	void Start () {
		buttonSource = GetComponent<AudioSource> ();
		rend = GetComponent<Renderer>();
		rend.enabled = true;
		rend.sharedMaterial = startColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnMouseOver() {
		rend.sharedMaterial = mouseOverColor;
	}

	void OnMouseExit()
	{
		rend.sharedMaterial = startColor;
	}

	void OnMouseDown() {
		buttonSource.Play ();
		if (scene != -1)
		SceneManager.LoadScene(scene);
	}
}
