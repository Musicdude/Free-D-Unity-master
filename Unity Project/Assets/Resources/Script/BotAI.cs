﻿using UnityEngine;
using System.Collections;

public class BotAI : MonoBehaviour {
	public Transform target;
	public int moveSpeed;
	public int rotationSpeed;
	public int maxdistance;
	
	private Transform myTransform;
	
	void Awake(){
		myTransform = transform;
	}
	
	
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		
		target = go.transform;
		
		maxdistance = 2;
	}
	
	
	void Update () {

		GameObject go = GameObject.FindGameObjectWithTag("Player");
		
		target = go.transform;
		
		maxdistance = 2;

		Debug.DrawLine(target.position, myTransform.position, Color.red); 
		
		
		myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
		
		if(Vector3.Distance(target.position, myTransform.position) > maxdistance){
			//Move towards target
			myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
			
		}
	}
	
}