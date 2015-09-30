using UnityEngine;
using System.Collections;

/// <summary>
/// A simple character input. Arrows to move, left SHIFT to run, SPACE to jump.
/// </summary>
public class SimpleCharacterInput : RaycastCharacterInput
{

	/// <summary>
	/// IF true always run.
	/// </summary>
	public bool alwaysRun;

	/// <summary>
	/// If true dropping from a passthrough platform requires user to press down and then jump.
	/// </summary>
	public bool jumpAndDownForDrop;

	private int movingDirection;

	void Update ()
	{
		if (Input.GetKey(KeyCode.R)) {
			//GameManager.Instance.deathPos = GameObject.Find("HeroCharacter").transform.position;
			GameManager.Instance.curTimes++;
			Application.LoadLevel(0);
		}
		
		jumpButtonHeld = false;
		jumpButtonDown = false;
		dropFromPlatform = false;
		x = 0;
		y = 0;
		
		if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) {
			x = 0.5f;
			movingDirection = 1;
		} else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
			x = -0.5f;
			movingDirection = -1;
		} else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)){
			x = movingDirection / 2.0f;
		}
	
		// Shift to run
		if (alwaysRun || Input.GetKey(KeyCode.LeftShift)) {
			x *= 2;
		}
		
		if (Input.GetKey(KeyCode.W) ) {
			y = 1;
		} else if (Input.GetKey(KeyCode.S) ) {
			y = -1;
			if (!jumpAndDownForDrop) dropFromPlatform = true;
		}
		
		if (Input.GetKey(KeyCode.Space) ) {
			jumpButtonHeld = true;
			if (Input.GetKeyDown(KeyCode.Space)) {
				if (jumpAndDownForDrop && Input.GetKey(KeyCode.S)) {
					dropFromPlatform = true;
				} else {
					jumpButtonDown = true;	
				}
				swimButtonDown = true;	
			} else {
				jumpButtonDown = false;		
				swimButtonDown = false;
			}
		} else {
			jumpButtonDown = false;
			swimButtonDown = false;
		}
	}	
}
