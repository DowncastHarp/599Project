﻿using UnityEngine;
using System.Collections;

public class Enemy1_2 : MonoBehaviour {
	public const int WALK = 1;
	public const int ATTACK = 2;
	public const int IDLE = 3;
	public const int GETHIT = 4;
	
	public AudioClip arrowFireSound;
	
	public float attackDis;
	public float patrolDis;
	public Vector3 targetPosition;
	public Vector3 startPosition;
	public float Speed = 3;
	public float RotateSpeed = 20;
	public GameObject Bullet;
	public Transform Bow;
	public float attackRate;
    private float walkingSpeed;
	
	public int state = IDLE;
	public bool moveLeft = true;
	//private bool canAttack = false;
	private float lastAttackTime = 0;
	
	public Transform player = null;
	
	private Animation myAni;
	public float[] angle0 = { -2f, -2.5f };
	// Use this for initialization
	void Start () {
		if(player == null) {
			player = GameObject.FindGameObjectWithTag("Player").transform;
		}
        walkingSpeed = Speed;
		myAni = transform.GetComponentInChildren<Animation> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		switch (state) {
		case WALK:
			//lastAttackTime = 0;
			myAni.CrossFade("idle",0.1f);
			if(moveLeft)
			{
				if(transform.position.x - startPosition.x + patrolDis <= 0.01f)
				{
					moveLeft = false;
				}else
				{
					targetPosition = new Vector3(startPosition.x - patrolDis, transform.position.y ,0);
				}
			}else
			{

				if(startPosition.x + patrolDis - transform.position.x   <= 0.01f)
				{
					moveLeft = true;
				}else
				{
					//Debug.Log("lalalalal");
					targetPosition = new Vector3(startPosition.x + patrolDis, transform.position.y ,0);
				}
			}
			break;
		case ATTACK:
			myAni.CrossFade("idle",0.1f);
			if(Time.time - lastAttackTime > attackRate)
			{ 
				GameObject temp = (GameObject) Instantiate( Bullet, Bow.position,Bullet.transform.rotation );
				// Play sound
				GetComponent<AudioSource>().PlayOneShot(arrowFireSound);
				if(temp.transform.GetComponentInChildren<BulletTest_P>())
				{
					temp.transform.GetComponentInChildren<BulletTest_P>().shooter = this.transform;
					temp.transform.GetComponentInChildren<BulletTest_P>().enemy = player;
				}else if(temp.transform.GetComponentInChildren<BulletTest_Homing>())
				{
					temp.transform.GetComponentInChildren<BulletTest_Homing>().shooter = this.transform;
					temp.transform.GetComponentInChildren<BulletTest_Homing>().target = player;
				}

				/*
				temp.transform.GetComponentInChildren<BulletTest_P>().ifHardCodeAngle = true;
				temp.transform.GetComponentInChildren<BulletTest_P>().angleHardCode = angle0[0];
				temp = (GameObject) Instantiate( Bullet, Bow.position-new Vector3(0,0.2f,0),Bullet.transform.rotation );
				temp.transform.GetComponentInChildren<BulletTest_P>().shooter = this.transform;
				temp.transform.GetComponentInChildren<BulletTest_P>().enemy = player;
				temp.transform.GetComponentInChildren<BulletTest_P>().ifHardCodeAngle = true;
				temp.transform.GetComponentInChildren<BulletTest_P>().angleHardCode = angle0[1];
				*/
				lastAttackTime = Time.time;
			}
			
			
			//temp.transform.parent = this.transform;
			//SetState(IDLE);
			break;
		case IDLE:
			myAni.CrossFade("idle",0.1f);
			break;
		case GETHIT:
			
			break;
		}
		
		//targetPosition.y = transform.position.y;
		Vector3 targetRotation = Vector3.zero;
		if (moveLeft && state != ATTACK&& state != IDLE) {
			targetRotation = new Vector3 (0, 180, 0);
			transform.eulerAngles = Vector3.Lerp (this.transform.eulerAngles, targetRotation, Time.deltaTime * RotateSpeed);
		} else if (!moveLeft && state != ATTACK&& state != IDLE){
			targetRotation = new Vector3 (0, 0, 0);
			transform.eulerAngles = Vector3.Lerp (this.transform.eulerAngles, targetRotation, Time.deltaTime * RotateSpeed);
		}
		
		//
		//Quaternion rotationTarget = Quaternion.LookRotation((targetPosition - this.transform.position).normalized);
		//transform.rotation = Quaternion.Lerp(this.transform.rotation,rotationTarget,Time.deltaTime * 5);
		
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * walkingSpeed);
		
	}
	
	public void SetState(int state) 
	{
		if (this.state == state)
			return;
		this.state = state;
		switch (state) {
		case WALK:
			walkingSpeed = Speed;
			break;
		case ATTACK:
			walkingSpeed = 0;
			break;
		case IDLE:
			walkingSpeed = 0;
			break;
		case GETHIT:
			walkingSpeed = 0;
			break;
		}
	}
	
	
	
	
}
