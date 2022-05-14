using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

	public GameObject playerTurnText;	//"your turn"
	public GameObject enemyTurnText;	//"enemy turn"
	public GameObject player;
	public bool playerTurn = true;		//bool that controls whether or not the player can do anything
	public bool enemyTurn = false;		//bool that controls whether or not the enemies can do anything
	public GameObject winText;			//the text for when you win
	public GameObject loseText;			//the text for when you lose
	public int enemiesLeft;				//tracks how many enemies are left
	private int enemyTurnsLeft;			//tracks how many enemies have finished their turn
	public GameObject livesText;
	public int lives = 2;
	public bool gameEnd = false;
	public AudioSource buttonSource;	//the component that controls the audio
	private CharacterController2D charControl;	//the character controller
	public bool win = false;


	// Use this for initialization
	void Start () {
		enemiesLeft = GameObject.FindGameObjectsWithTag ("Enemy").Length;	//sets enemiesLeft to the total number of enemies
		livesText.GetComponent<TextMesh> ().text = lives.ToString ();		//updates the life counter with the current value for lives
		buttonSource = GetComponent<AudioSource> ();
		charControl = player.GetComponent<CharacterController2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//plays the dirt hit sound when a bullet hits the ground.
	public void PlayHitSound() {
		buttonSource.Play ();
	}

	//an enemy will call this method when it dies. this function decreases the value of enemiesLeft.
	public void EnemyDecrease () {
		enemiesLeft = GameObject.FindGameObjectsWithTag ("Enemy").Length - 1;
		enemyTurnsLeft--;
		if (enemiesLeft <= 0) {
			Win ();
		}
	}
		
	//this method runs when you win
	private void Win () {
		Debug.Log ("you win!");
		gameEnd = true;
		win = true;
		winText.SetActive (true);
		playerTurn = false;
		enemyTurn = false;
		playerTurnText.SetActive (false);
		enemyTurnText.SetActive (false);
	}

	private void Lose () {
		Destroy (player);
		gameEnd = true;
		Debug.Log ("you lose");
		loseText.SetActive (true);
		playerTurn = false;
		enemyTurn = false;
		playerTurnText.SetActive (false);
		enemyTurnText.SetActive (false);
	}

	//this method will run when the player's turn ends.
	public void TurnEnd () {
		if (!gameEnd) {
			playerTurn = false;
		}
	}

	//this method runs when the enemy turn starts
	public void EnemyTurn () {
		if (!gameEnd) {
			enemyTurnsLeft = enemiesLeft;
			enemyTurnText.SetActive (true);
			playerTurnText.SetActive (false);
			enemyTurn = true;
		}
	}

	//this method runs when the player's turn starts
	public void PlayerTurn () {
		if (!gameEnd) {
			enemyTurnsLeft--;
			if (enemyTurnsLeft <= 0) {
				enemyTurn = false;
				enemyTurnText.SetActive (false);
				playerTurnText.SetActive (true);
				playerTurn = true;
				enemyTurn = false;
				if (!charControl.m_Grounded) {
					player.transform.eulerAngles = new Vector3 (gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);
				}
			}
		}
	}

	public void PlayerHit () {
		lives--;
		livesText.GetComponent<TextMesh> ().text = lives.ToString ();	//updates the lives counter with the current value for lives
		if (lives <= 0) {
			Lose ();
		}
	}
}