using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public GameObject shell;			//the bullet game object
	public GameObject shotPoint;		//the game object that marks the position where the bullet shoots from
	public GameObject CannonController;	//the game object that controls the cannon
	public GameControl gameController;
	public GameObject player;
	public AudioSource soundSource;		//the component that controls the audio

	public float rotationAmount = 20f;	//controls how much the cannon rotates
	public float force = 18f;			//the force of the cannon shot
	public float minForce = 15;			//the minimum force of shots
	public float maxForce = 22;			//the maximum force of shots
	public int rotation;				//the position that the cannon will rotate to
	//public int playerHitRotation;		
	public float shotTime = 10f;		//maximum amount of time that they will calculate their shots
	private bool turn = true;			//bool that controls whether or not its their turn
	public int difficulty = 1;			//stores the difficulty of the enemy. 1 is easy, 2 is medium, 3 is hard
	public float randomRotaAmount;
	//private bool fire = false;

	// Use this for initialization
	void Start () {
		soundSource = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		//resets the turn value when its no longer the enemy's turn
		if (gameController.enemyTurn == false) {
			turn = true;
		}
		//if its the enemy's turn, run AiMove
		if (gameController.enemyTurn == true && turn == true) {
			AiMove ();
			turn = false;
		}
	}

	public Vector3 PlotTrajectoryAtTime (Vector3 start, Vector3 startVelocity, float time) {
		return start + startVelocity*time + Physics.gravity*time*time*0.5f;
	}

	//this method calculates where the shot will hit based on the enemy's current angle and power
	public bool PlotTrajectory (Vector3 start, Vector3 startVelocity, float timestep, float maxTime) {

		Vector3 prev = start;
		for (int i=1;;i++) {
			float t = timestep*i;
			if (t > maxTime) break;
			Vector3 pos = PlotTrajectoryAtTime (start, startVelocity, t);
			RaycastHit2D hit = Physics2D.Linecast (prev, pos, 1 << LayerMask.NameToLayer("Player"));	
			if (hit.collider != null) {
				return true;
			}
			if (Physics2D.Linecast (prev, pos)) break;
			Debug.DrawLine (prev,pos,Color.red);
			prev = pos;
		}
		return false;
	}

	//this method controls what the enemy does during its turn
	void AiMove () {
		force = Random.Range (minForce, maxForce);
		//checks if the player is left or right of the enemy and fires in that direction
		if (player.transform.position.x < gameObject.transform.position.x) {
			rotation = Random.Range (100, 180);
		} else {
			rotation = Random.Range (0, 80);
		}
		StartCoroutine(RotateCannon (rotation));

	}


	//this method fires a shot
	public void Fire () {
		soundSource.Play();
		GameObject Bullet = (GameObject)Instantiate (shell, shotPoint.transform.position, CannonController.transform.rotation);
		Bullet.GetComponent<Rigidbody2D> ().AddForce (Bullet.transform.right*force, ForceMode2D.Impulse);
		StartCoroutine (TurnEnder (Bullet));
	}


	IEnumerator TurnEnder (GameObject bullet) {
		bool End = false;
		int Counter = 0;
		while (End == false && Counter <= 40) {
			yield return new WaitForSeconds (0.2f);
			if (bullet == null) {
				End = true;
			}
			Counter++;
		}
		gameController.PlayerTurn ();
	}

	//this method will rotate the cannon in the specified direction. 0 is left, 1 is right.
	IEnumerator RotateCannon (int rotation) {
		int direction;
		int amount;		//the amount that the cannon needs to rotate
		if (rotation > (int)CannonController.transform.localRotation.eulerAngles.z) {
			direction = 0;
			amount = rotation - (int)CannonController.transform.localRotation.eulerAngles.z;
		} else {
			direction = 1;
			amount = (int)CannonController.transform.localRotation.eulerAngles.z - rotation;
		}

		for (int i = 0; i <= amount; i = i + (int)rotationAmount) {
			if (PlotTrajectory (shotPoint.transform.position, shotPoint.transform.right * force, 0.02f, shotTime)) {		//checks if the current angle will hit the player
				if (difficulty == 1 && gameController.enemiesLeft > 2) {
					for (int j = 0; j < (int)Random.Range (0, randomRotaAmount); j++) {	//chance to rotate the cannon a little bit by a random amount if the difficulty is easy
						Rotate (direction);
					}
				}
				i = amount;
				break;
			}
			Rotate (direction);
			yield return new WaitForSeconds (0.05f);
		}
		yield return new WaitForSeconds (0.5f);
		Fire ();
	}

	private void Rotate (int direction) {
		if (direction == 0 && (CannonController.transform.localRotation.eulerAngles.z < 180 || (CannonController.transform.localRotation.eulerAngles.z > 200 && CannonController.transform.localRotation.eulerAngles.z <= 360))) {
			CannonController.transform.Rotate (new Vector3 (0, 0, rotationAmount));
		} else if (direction == 1 && CannonController.transform.localRotation.eulerAngles.z > 0 && CannonController.transform.localRotation.eulerAngles.z < 200) {
			CannonController.transform.Rotate (new Vector3 (0, 0, -rotationAmount));
		}
	}

	//destroys the enemy if a bullet hits it
	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.layer == 9) {	//layer 9 is the layer that bullets are on
			gameController.EnemyDecrease();
			Destroy (this.gameObject);
		}
	}
}
