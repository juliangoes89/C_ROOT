using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public GameObject GameManagerGO;
	public GameObject explosionPrefab; // This is the explosion prefab.
	public GameObject shieldsObject; // This is the shields prefab.

	// These are the player bullet prefabs.
	public GameObject PlayerBulletBlue;
	public GameObject PlayerBulletRed;

	// These are empty Transforms used as a reference for your bullets firing point.
	public Transform firePoint01;
	public Transform firePoint02;
	public Transform firePoint03;
	public Transform firePoint04;
	public Transform firePoint05;

	int weaponID = 1; // This is the default weapon ID.
	private float fireRate = 6f; // You can set a default firing rate here or per case in the weaponID switch.
	float timeToFire = 0f;

	public AudioSource laserSound;

	public Text powerUpText; // The power up UI text gameObject.
	string powerUpDesc; // This is the pop up text for your power ups.
	float powerUpTimer; // This timer controls the duration of power ups.

	public float moveSpeed;

	public void Init()
	{
		// Activate the gameObject.
		gameObject.SetActive (true);
		// Set our starting position.
		transform.position = new Vector2 (0, 1.25f);
	}
	
	void Update () 
	{
		// This counts down the power up timer.
		powerUpTimer -= Time.deltaTime;
		// If the power up timer reaches < 0, cap it to 0 and reset the players weapon to default.
		if (powerUpTimer < 0) {
			powerUpTimer = 0;
			ResetWeapon ();
		}

#if !MOBILE_INPUT
		// The player will follow the current mousePosition.
		{
			//var pos = Input.mousePosition;
			//pos.z = transform.position.z - Camera.main.transform.position.z;
			//pos = Camera.main.ScreenToWorldPoint(pos);
			//transform.position = Vector3.Lerp(transform.position, pos, moveSpeed * Time.deltaTime);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.GetAxis("Horizontal") * 5.2f, 0) - GetComponent<Rigidbody2D>().velocity * .8f);
		}
#else
		// These are the touchScreen controls.
		if(Input.touchCount > 0) {
			Vector2  touchDeltaPosition =  Input.GetTouch(0).deltaPosition/30;
			transform.Translate (touchDeltaPosition.x * moveSpeed * Time.deltaTime, touchDeltaPosition.y * moveSpeed * Time.deltaTime, 0);
		}
#endif

		Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0,0));
		Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1,1));
		
		max.x = max.x - 0.225f; 
		min.x = min.x + 0.225f; 
		
		max.y = max.y - 0.285f; 
		min.y = min.y + 0.285f; 
		// This makes sure our player never leaves the screen area.
		GetComponent<Rigidbody2D>().position = new Vector2 
			(
				Mathf.Clamp (GetComponent<Rigidbody2D>().position.x, min.x, max.x),  //X
				Mathf.Clamp (GetComponent<Rigidbody2D>().position.y, min.y, max.y)	 //Y
			);
		// This will limit the firing rate of the player, and fire the weapon whenever the screen is touched.
		if (fireRate == 0f) {
			if (Input.GetKey("space")) {
				FireWeapon ();
			}
		} else {
			if (Input.GetKey("space") && Time.time > timeToFire) {
				timeToFire = Time.time + 1f / fireRate;
				FireWeapon ();
			}
		}
	}

	void FireWeapon ()
	{
		// Play the laser sound effect.
		laserSound.Play();
		// Find which weapon ID we are using (if we have default or power up) and switch accordingly.
		switch (weaponID)
		{
		case 1:
			Instantiate(PlayerBulletBlue, firePoint01.position, firePoint01.rotation);
			break;
		case 2:
			Instantiate(PlayerBulletBlue, firePoint02.position, firePoint02.rotation);
			Instantiate(PlayerBulletBlue, firePoint03.position, firePoint03.rotation);
			break;
		case 3:
			Instantiate(PlayerBulletRed, firePoint01.position, firePoint01.rotation);
			Instantiate(PlayerBulletRed, firePoint04.position, firePoint04.rotation);
			Instantiate(PlayerBulletRed, firePoint05.position, firePoint05.rotation);
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		// Detect collision of the player ship with the enemy ship or player bullet.
		if ((col.tag == "EnemyShip") || (col.tag == "EnemyBullet")) 
		{
			// This is the current amount you get hurt when hit by an enemy ship or bullet.
			GetComponent<PlayerHealth>().TakeDamage(10f);

			if(GetComponent<PlayerHealth>().curHealth > 0) // If the player still has life remaining then play the shield animation.
			{
				PlayShields();
			}
			else if(GetComponent<PlayerHealth>().curHealth <= 0) // If the player is out of life, destroy player.
			{
				// Explode if the player is out of health.
				PlayExplosion();
				// Change the gamestate to 'Game Over'.
				GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
				// Hide the player ship.
				gameObject.SetActive(false);
			}
		}
		// Detect collision of the player ship with a world hazard, and reduce current health to 0.
		else if ((col.tag == "WorldHazard"))
		{
			// Currently hitting a world hazard will reduce your health to 0 and destroy you.
			GetComponent<PlayerHealth>().TakeDamage(100f);

			// Explode if the player is out of health.
			PlayExplosion();
			// Change the gamestate to 'Game Over'.
			GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
			// Hide the player ship.
			gameObject.SetActive(false);
		}
		// COMBAT EXAMPLE: This is a combat power up example which switches the players weapon type, you could customise this in different ways.
		else if ((col.tag == "BluePowerUp"))
		{
			// Set the power up description.
			powerUpDesc = "Double Barrel!";
			// Set the power up duration.
			powerUpTimer = 8f;
			// Set the weaponID.
			weaponID = 2;
			// Show the power up text.
			StartCoroutine(ShowPowerUpText());
		}
		// COMBAT EXAMPLE: This is a combat power up example which switches the players weapon type, you could customise this in different ways.
		else if ((col.tag == "RedPowerUp"))
		{
			// Set the power up description.
			powerUpDesc = "Tri-Laser!";
			// Set the power up duration.
			powerUpTimer = 8f;
			// Set the weaponID.
			weaponID = 3;
			// Show the power up text.
			StartCoroutine(ShowPowerUpText());
		}
		// UTILITY EXAMPLE: This is a utility power up example which increases the players health by 50.
		else if ((col.tag == "GreenPowerUp"))
		{
			// Set the power up description.
			powerUpDesc = "Shields Increased!";
			// Increase the players health by 50, capping at 100.
			GetComponent<PlayerHealth>().GiveHealth(50f);

			if (GetComponent<PlayerHealth>().curHealth > 100f) {
				GetComponent<PlayerHealth>().curHealth = 100f;
			}
			// Show the power up text.
			StartCoroutine(ShowPowerUpText());
		}
	}

	IEnumerator ShowPowerUpText ()
	{
		// Update the UI Text to output the powerUpDesc string.
		powerUpText.text = powerUpDesc;
		yield return new WaitForSeconds (2f);
		// Return the text to null.
		powerUpText.text = "";
	}
		
	// This function resets the player weapon to default, useful for when the power up timer has finished.
	void ResetWeapon ()
	{
		// Default weaponID.
		weaponID = 1;
	}

	// Function to instantiate the shield animation.
	void PlayShields()
	{
		// Start the animate shields coroutine.
		StartCoroutine (AnimateShields ());
	}

	IEnumerator AnimateShields()
	{
		// Set our shield gameObject to be visible.
		shieldsObject.SetActive (true);
		yield return new WaitForSeconds (0.1f);
		// Set our shield gameObject to be invisible.
		shieldsObject.SetActive (false);
	}

	// Function to instantiate an explosion.
	void PlayExplosion()
	{
		GameObject explosion = (GameObject)Instantiate (explosionPrefab);
		explosion.transform.position = transform.position;
	}
}