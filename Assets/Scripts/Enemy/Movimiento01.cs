using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Movimiento01:MonoBehaviour{
	private Vector2 origen;
	public Vector2 velocidadOrigen=new Vector2(0,0);
	public Vector2 velocidad=new Vector2(5,0);
	public float frecuencia=2;
	void Start(){
		origen=new Vector2(gameObject.transform.position.x,gameObject.transform.position.y);
		if(GetComponent<Rigidbody2D>()==null){
			gameObject.AddComponent<Rigidbody2D>();
			GetComponent<Rigidbody2D>().gravityScale=0;
		}
		gameObject.GetComponent<Rigidbody2D>().velocity=velocidad;
	}
	void Update(){
		GetComponent<Rigidbody2D>().AddForce((origen-new Vector2(gameObject.transform.position.x,gameObject.transform.position.y))*frecuencia);
	}
	void FixedUpdate(){
		origen=new Vector2(origen.x+velocidadOrigen.x*Time.fixedDeltaTime,origen.y+velocidadOrigen.y*Time.fixedDeltaTime);
	}
}
