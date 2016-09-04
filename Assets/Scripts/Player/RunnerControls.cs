﻿using UnityEngine;
using System.Collections;

public class RunnerControls : MonoBehaviour {

	//inside class
	private Vector2 firstPressPos;
	private Vector2 secondPressPos;
	private Vector2 currentSwipe;

	public GameObject player;

    private Vector3 startPosition;

	public static float leftZ = 3.4f;
	public static float centerZ = 0f;
	public static float rightZ = -3.4f;

    private float currentLaneZ;

	/** The time it takes to move from one position to another e.g leftZ to centerZ*/
	public float sideSpeed;
	public float jumpForce;
	public float diveForce;
	private long moveStartTime;

	private Lane currentLane;
	private Lane targetLane;

	private float startTime;
	private float speedMult = 1.0f;

	enum Lane {
		LEFT, CENTER, RIGHT
	}

	// Use this for initialization
	void Start () {
		currentLane = Lane.CENTER;
		targetLane = Lane.CENTER;

        startPosition = transform.position;

        StartGame();
	}

	void StartGame(){
		startTime = Time.realtimeSinceStartup;
		Time.timeScale = 1.7f;
	}
	
	// Update is called once per frame
	void Update () {
		Time.timeScale += 0.0001f * Time.deltaTime;

        switch (currentLane) {

            case Lane.CENTER: currentLaneZ = centerZ; break;
            case Lane.LEFT: currentLaneZ = leftZ; break;
            case Lane.RIGHT: currentLaneZ = rightZ;  break;

        }

		SwipeTouch ();
        SwipeMouse();

		updateTweens ();

	}
	

	private void updateTweens(){

		if (targetLane == currentLane) {
			return;
		}

		Vector3 pos = player.transform.position;

		float startZ = pos.z;
		float targetZ = 0;

		switch (targetLane) {

		case Lane.CENTER: targetZ = centerZ;break;
		case Lane.LEFT: targetZ = leftZ;break;
		case Lane.RIGHT: targetZ = rightZ;break;

				}

		if (targetZ < startZ) {
			player.transform.Translate (new Vector3(0, 0, -Time.deltaTime*sideSpeed));
		}

		if (targetZ > startZ) {
			player.transform.Translate (new Vector3(0, 0, Time.deltaTime*sideSpeed));
		}
		if(Mathf.Abs (targetZ - player.transform.position.z) < 0.2){
			player.transform.Translate (0, 0, targetZ-player.transform.position.z);
		}

		if (targetZ == player.transform.position.z) {
			currentLane = targetLane;
		}

	}

	private void swipeLeft(){
		switch (targetLane) {
		case Lane.CENTER : targetLane = Lane.LEFT; break;
		case Lane.RIGHT: targetLane = Lane.CENTER; break;
		case Lane.LEFT: ;break;
		}
		player.GetComponent<Animation>() ["leanleft"].speed = 2.5f;
		player.GetComponent<Animation>().CrossFade ("leanleft", 0.25f);
	}

	private void swipeRight(){
		switch (targetLane) {
		case Lane.CENTER : targetLane = Lane.RIGHT; break;
		case Lane.RIGHT: ;break;
		case Lane.LEFT: targetLane = Lane.CENTER; break;
		}
		player.GetComponent<Animation>() ["leanright"].speed = 2.5f;
		player.GetComponent<Animation>().CrossFade ("leanright", 0.25f);
	}

	private void swipeUp(){
		if (player.transform.position.y < 0.2) {
			player.GetComponent<Animation>().Stop ();
			player.GetComponent<Animation>() ["jump"].speed = 1.1f;
			player.GetComponent<Animation>().Play ("jump", PlayMode.StopAll);
			player.GetComponent<Rigidbody>().velocity = Vector3.zero;
			player.GetComponent<Rigidbody>().AddForce (new Vector3 (0, jumpForce, 0));
		}

	}

	private void swipeDown(){
        if (player.transform.position.y > -0.4) {
			player.GetComponent<Rigidbody>().velocity = Vector3.zero;
			player.GetComponent<Rigidbody>().AddForce (new Vector3 (0, diveForce, 0));
			//player.GetComponent<Animation>() ["dive"].speed = 1.7f;
			//player.GetComponent<Animation>().CrossFade ("dive", 0.25f);
		}
	}

	public void SwipeTouch()
	{
		if(Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began)
			{
				//save began touch 2d point
				firstPressPos = new Vector2(t.position.x,t.position.y);
			}
			if(t.phase == TouchPhase.Ended)
			{
				//save ended touch 2d point
				secondPressPos = new Vector2(t.position.x,t.position.y);
				
				//create vector from the two points
				currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
				
				//normalize the 2d vector
				currentSwipe.Normalize();
				
				//swipe upwards
				if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
					swipeUp ();
				}
				//swipe down
				if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
					swipeDown ();
				}
				//swipe left
				if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
					swipeLeft();
				}
				//swipe right
				if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
					swipeRight ();
				}
			}
		}
	}

		public void SwipeMouse()
		{
			if(Input.GetMouseButtonDown(0))
			{
				//save began touch 2d point
				firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
			}
			if(Input.GetMouseButtonUp(0))
			{
				//save ended touch 2d point
				secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
				
				//create vector from the two points
				currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
				
				//normalize the 2d vector
				currentSwipe.Normalize();
				
				//swipe upwards
				if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
				swipeUp();
				}
				//swipe down
				if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
				swipeDown ();
				}
				//swipe left
				if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
				swipeLeft();
				}
				//swipe right
				if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
				swipeRight ();
				}
			}
	}

    void OnCollisionEnter(Collision col)
    {
        ObstacleSettings settings = col.gameObject.GetComponent<ObstacleSettings>();
        if (settings != null)
        {
            if (settings.isDangerous())
            {
                GetComponent<Rigidbody>().freezeRotation = false;
                GameController.setGameOver();
            }
        }
    }

}
