using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private GameObject gameController;
	public GameObject dirtParticles;
	public GameObject bulletParticles;

	// Use this for initialization
	void Start () {
		gameController = GameObject.Find("GameController");
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.layer == 8) {
			GameObject particles = Instantiate (dirtParticles, gameObject.transform.position, Quaternion.identity);
			particles.GetComponent<ParticleSystem> ().Play ();
		} else {
			GameObject particles = Instantiate (bulletParticles, gameObject.transform.position, Quaternion.identity);
			particles.GetComponent<ParticleSystem> ().Play ();

		}
			
		if (collision.gameObject.layer == 11) {		//layer 11 is the layer that the player is on
			gameController.GetComponent<GameControl>().PlayerHit();
			}
		gameController.GetComponent<GameControl> ().PlayHitSound ();
		Destroy (this.gameObject);
	}

}


/*
if (collision.gameObject.layer == 10) {
	Destroy (this.gameObject);
}
*/