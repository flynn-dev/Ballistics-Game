using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class Player : MonoBehaviour {
	public GameObject shell;			//the bullet game object
	public GameObject shotPoint;		//the game object that marks the position where the bullet shoots from
	public GameObject CannonController;	//the game object that controls the cannon
	public GameObject powerText;		//the text that displays the power level
	public GameControl gameController;	//the GameObject that controls the game
	public GameObject fuelText;			//the text that displays how much fuel is left
	public AudioSource soundSource;	//the component that controls the audio

	public float rotationAmount = 20f;	//controls how much the cannon rotates when you click the rotate button
	public float force = 20f;			//the force of the cannon shot
	public float forceAmount = 4;		//controls how much the cannon force increases or decreases when you click the power increase/decrease button
	public float minForce = 0;			//the minimum force of shots
	public float maxForce = 25;			//the maximum force of shots
	public float shotTime = 2f;			//the time that the shot line calculates
	public float fuelAmount = 100;		// the amount that fuel will reset to after each turn

	public float fuel;
	private LineRenderer lineRenderer;
	private float lineSize;

	// Use this for initialization
	void Start () {
		soundSource = GetComponent<AudioSource> ();
		powerText.GetComponent<TextMesh> ().text = force.ToString();
		fuel = fuelAmount;
	}

	// Update is called once per frame
	void Update () {
		if (gameController.playerTurn == true) {
			PlotTrajectory (shotPoint.transform.position, shotPoint.transform.right * force, 0.02f, shotTime);	//creates the line that shows how your shot will travel
		}
		fuelText.GetComponent<TextMesh> ().text = fuel.ToString ();	//updates the fuel meter
		//allows you to rotate the cannon with the up and down keys
		if ((Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow)) && gameController.playerTurn)  {
			RotateCannon (0);
		} else if ((Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) && gameController.playerTurn) {
			RotateCannon (1);
		}

	}

	//this method fires a shot
	public void Fire () {
		soundSource.Play();	//plays the fire sound effect
		GameObject Bullet = (GameObject)Instantiate (shell, shotPoint.transform.position, CannonController.transform.rotation);	//creates the bullet
		Bullet.GetComponent<Rigidbody2D> ().AddForce (Bullet.transform.right*force, ForceMode2D.Impulse);	//gives the bullet force
		gameController.TurnEnd ();
		StartCoroutine (TurnEnder (Bullet));
	}

	//this method calculates the trajectory of a shot at a certain time
	public Vector3 PlotTrajectoryAtTime (Vector3 start, Vector3 startVelocity, float time) {
		return start + startVelocity*time + Physics.gravity*time*time*0.5f;
	}

	//this method calculates the trajectory of a shot
	public void PlotTrajectory (Vector3 start, Vector3 startVelocity, float timestep, float maxTime) {
		
		Vector3 prev = start;
		for (int i=1;;i++) {
			float t = timestep*i;
			if (t > maxTime) break;
			Vector3 pos = PlotTrajectoryAtTime (start, startVelocity, t);
			Physics2D.Linecast (prev, pos, 1 << LayerMask.NameToLayer("Enemy"));	
			if (Physics2D.Linecast (prev, pos)) break;
			//Debug.DrawLine (prev,pos,Color.red);
			DrawLine (prev, pos, Color.red, 0.02f);
			prev = pos;
		}
	}

	//this method draws the trajectory line on the screen
	void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
	{
		GameObject myLine = new GameObject();
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Hidden/Internal-Colored"));
		lr.startColor = color;
		lr.endColor = color;
		lr.startWidth = 0.2f;
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		GameObject.Destroy(myLine, duration);
	}
		
	//this runs when the player shoots. It ends the player's turn after the bullet is destroyed
	IEnumerator TurnEnder (GameObject bullet) {
		bool End = false;
		int Counter = 0;
		while (End == false && Counter <= 20) {
			yield return new WaitForSeconds (0.2f);
			if (bullet == null) {
				End = true;
			}
			Counter++;
		}
		gameController.EnemyTurn ();
		fuel = fuelAmount;
	}

	//this method will rotate the cannon in the specified direction. 0 is left, 1 is right.
	public void RotateCannon (int direction) {
		if (direction == 0 && (CannonController.transform.localRotation.eulerAngles.z <180 || (CannonController.transform.localRotation.eulerAngles.z > 200 && CannonController.transform.localRotation.eulerAngles.z <= 360) )) {
			CannonController.transform.Rotate (new Vector3 (0, 0, rotationAmount));
		} else if (direction == 1 && CannonController.transform.localRotation.eulerAngles.z > 0 && CannonController.transform.localRotation.eulerAngles.z < 200) {
			CannonController.transform.Rotate (new Vector3 (0, 0, -rotationAmount));
		}
	}

	//this function increases or decreases the force variable
	public void IncreaseForce (bool increase) {
		if (increase && force + forceAmount <= maxForce) {
			force = force + forceAmount;
			powerText.GetComponent<TextMesh> ().text = force.ToString();
		} else if (!increase && force - forceAmount >= minForce) {
			force = force - forceAmount;
			powerText.GetComponent<TextMesh> ().text = force.ToString();
		}
	}
}