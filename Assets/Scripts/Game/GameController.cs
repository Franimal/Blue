using UnityEngine;
using System.Collections;
using System;

public class GameController : MonoBehaviour {

	private static State gameState;
    public float obstacleMoveSpeed;
	public static float obstacleSpeed = -5.5f;

	// Use this for initialization
	void Start () {
		gameState = State.PLAYING;
	}
	
	// Update is called once per frame
	void Update () {
        obstacleSpeed = Time.timeScale * obstacleMoveSpeed;
	}

	public static float getObstacleSpeed(){
		return obstacleSpeed;
	}

	public static State getGameState(){
		return gameState;
	}

	public enum State {
		MAIN_MENU, CHARACTER_SELECTION, PLAYING, GAMEOVER
	}

    public static void setGameOver()
    {
        gameState = State.GAMEOVER;
    }
}
