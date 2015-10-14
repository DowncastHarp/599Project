﻿using UnityEngine;
using System.Collections;

public class CameraMovementC1 : MonoBehaviour 
{
	public const short FROZEN = 0;
	public const short EXPEND = 1;
	public const short SHRINK = 2;
	public const short BOUNDARY = 3;

	public short status;
	public Transform trackSphere;
	
	public float distanceZ;

	public float hardBoundary = 10f;
	public float softBoundary = 5f;
	public float cameraSpeed = 40;
	public float expendCof = 0.01f;
	public float shrinkCof = 0.015f;
	public Transform target;

	private bool isVictory = false;
	private bool isStart = false;
	private GameObject m_player;
	private DirectionChecker m_dc;
	private RaycastCharacterController m_rc;
	private int lastDir = 0;

	void Start()
	{
		m_player = GameManager.Instance.player;
		m_dc = m_player.GetComponentInChildren<DirectionChecker> ();
		m_rc = m_player.GetComponent<RaycastCharacterController> ();

		if (trackSphere == null) 
		{
			trackSphere = GameObject.Find("HeroCharacter/TrackSphere").transform;
		}

		m_player.GetComponent<RaycastCharacterController> ().controllerActive = false;
		
		if(GameManager.Instance.curTimes>1 || !GameManager.Instance.isCutScene )//Time.time - lasttime > 30
		{
			m_player.GetComponent<RaycastCharacterController>().controllerActive = true;
		}

		trackSphere.localPosition = Vector3.zero;

		this.transform.position = new Vector3 (trackSphere.position.x, trackSphere.position.y + 5, trackSphere.position.z - distanceZ);
		isVictory = false;
		isStart = false;
		status = EXPEND;
	}


	void Update () //Interpolate the position of sphere
	{
		if (isVictory) {
			DoVictory ();
		} else 
		{
			switch(status)
			{
			case BOUNDARY:
			{
				if(m_rc.State == CharacterState.IDLE)
				{
					status = SHRINK;
				}else
				{
					trackSphere.Translate(Vector3.zero);
				}

			}
				break;
			case FROZEN:
			{
				trackSphere.Translate(Vector3.zero);
				if(m_rc.State == CharacterState.WALKING) //oppsite dir
				{
					status = EXPEND;
				}

			}
				break;
			case SHRINK:
			{

				if (Mathf.Abs (trackSphere.localPosition.x) <= softBoundary) 
				{
					if(m_rc.State == CharacterState.WALKING)
					{
						status = EXPEND;
					}
				}else
				{
					trackSphere.Translate(new Vector3(-lastDir,0,0)*m_rc.movement.walkSpeed*shrinkCof);
				}
			}
				break;
			case EXPEND:
			{

				if(m_rc.State == CharacterState.WALKING)
				{
					//Debug.Log(new Vector3(m_dc.CurrentDirection,0,0));
					lastDir = m_dc.CurrentDirection;
					isStart = true;
					trackSphere.Translate(new Vector3(m_dc.CurrentDirection,0,0)*m_rc.movement.walkSpeed*expendCof);
				}else
				{
					trackSphere.Translate(Vector3.zero);
				}

				if (Mathf.Abs (trackSphere.localPosition.x) >= hardBoundary) 
				{
					status = BOUNDARY;
				}else if(isStart &&  m_rc.State == CharacterState.IDLE)
				{
					if(Mathf.Abs (trackSphere.localPosition.x) > softBoundary)
					{
						status = SHRINK;
					}else
					{
						status = FROZEN;
					}
				}
			}
				break;
			

			}


		}
	}

	void LateUpdate () //Interpolate Camera by tracksphere
	{
		if (!isVictory)
		{
			transform.position = Vector3.MoveTowards (transform.position,new Vector3 (trackSphere.position.x,m_player.transform.position.y+5,trackSphere.position.z -distanceZ),Time.deltaTime*cameraSpeed);
		}
	}


	void DoVictory()	
	{
		transform.position = Vector3.MoveTowards (transform.position,new Vector3 (target.position.x,target.position.y+5,target.position.z -distanceZ),Time.deltaTime*80); 
	}

	public void FirstLevelVictory(Victory win)
	{
		isVictory = true;
		target = win.family [0].transform;
		
	}







}