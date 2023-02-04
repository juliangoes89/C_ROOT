using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For the Queue.

public class BackgroundScrollController : MonoBehaviour {

	public GameObject[] BGScrollObjects; // An array of BG prefabs.

	// Queue to hold the objects.
	Queue<GameObject> availableObjects = new Queue<GameObject>();

	Vector2 max; // This is the top-right point of the screen.
	Vector2 min;
	void awake() {
		max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
		min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
	}
	// Use this for initialization
	void Start () 
	{
		// Add the available objects to the queue.
		//availableObjects.Enqueue (BGObjects [0]);
		//availableObjects.Enqueue (BGObjects [1]);
		//availableObjects.Enqueue (BGObjects [2]);
		availableObjects.Enqueue(BGScrollObjects[0]);
		availableObjects.Enqueue(BGScrollObjects[1]);
		availableObjects.Enqueue(BGScrollObjects[2]);

		// Call the MoveObjectDown function every 20 seconds.
		InvokeRepeating ("MoveObjectUp", 0, 10f);	
	}

	// Function to dequeue an object, and set its isMoving flag to true
	// so that the object will start to scroll down the screen.
	void MoveObjectDown()
	{
		EnqueueObjects ();

		// If the queue is empty, then return.
		if (availableObjects.Count == 0)
			return;

		// Get an object from the queue.
		GameObject aPlanet = availableObjects.Dequeue ();
		// Set the objects isMoving flag to true.
		aPlanet.GetComponent<BGScrollObject> ().isMoving = true;
	}

	// Function to dequeue an object, and set its isMoving flag to true
	// so that the object will start to scroll down the screen.
	void MoveObjectUp()
	{
		EnqueueObjectsUp();

		// If the queue is empty, then return.
		if (availableObjects.Count == 0)
			return;

		// Get an object from the queue.
		GameObject aPlanet = availableObjects.Dequeue();
		// Set the objects isMoving flag to true.
		aPlanet.GetComponent<BGScrollObject>().isMoving = true;
	}



	// Function to enqueue objects that are below the screen and not moving.
	void EnqueueObjects()
	{
		foreach (GameObject anObject in BGScrollObjects) 
		{
			// If the planet is below the screen and the planet is not moving.
			if((anObject.transform.position.y < 0) && (!anObject.GetComponent<BGScrollObject>().isMoving))
			{
				// Reset the planet position.
				anObject.GetComponent<BGScrollObject>().ResetPosition();
				// Enqueue the object.
				availableObjects.Enqueue(anObject);
			}
		}
	}

	// Function to enqueue objects that are below the screen and not moving.
	void EnqueueObjectsUp()
	{
		foreach (GameObject anObject in BGScrollObjects)
		{
			// If the planet is over the screen and the planet is not moving.
			if ((anObject.transform.position.y > max.y + anObject.GetComponent<BGScrollObject>().GetComponent<SpriteRenderer>().sprite.bounds.extents.y) && (!anObject.GetComponent<BGScrollObject>().isMoving))
			{
				// Reset the planet position.
				anObject.GetComponent<BGScrollObject>().ResetScrollPosition();
				// Enqueue the object.
				availableObjects.Enqueue(anObject);
			}
		}
	}
}