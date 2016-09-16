using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour {

	private static State gameState;
    public static float startObstacleMoveSpeed = -3.5f;
	public static float obstacleSpeed = -5.5f;
    private static float actualObstacleSpeed = 0;

    public static float acceleration = 1.0f;
    public static float decceleration = 2.0f;

    public float initialTimeScale = 2.5f;
    private float timeScale = 1;

    private bool gameStarted = false;

    public static GameController instance;

	// Use this for initialization
	void Start () {
        GameController.instance = this;
		gameState = State.MAIN_MENU;
        gameStarted = false;
        Time.timeScale = initialTimeScale;
        timeScale = initialTimeScale;
	}
	
	// Update is called once per frame
	void Update () {
        if(gameState == State.MAIN_MENU || gameState == State.PAUSED || gameState == State.CHARACTER_SELECTION) {
            if (Input.GetKeyDown("p")){
                resume();
                return;
            }
           // obstacleSpeed = 0;
           // actualObstacleSpeed = 0;
            return;
        } 
        if(gameState == State.PLAYING && !gameStarted) {

            gameStarted = true;
        }
        if(gameState == State.PLAYING) {
            Time.timeScale += 0.0001f * Time.deltaTime;
            timeScale = Time.timeScale;
            if (Input.GetKeyDown("p")) {
                pause();
                return;
            }
        }
        if(Mathf.Abs(obstacleSpeed - actualObstacleSpeed) < 0.2f) {
            actualObstacleSpeed = obstacleSpeed;
            if (gameState != State.GAMEOVER) {
                obstacleSpeed = startObstacleMoveSpeed;
            }
        } else {
            actualObstacleSpeed = Mathf.SmoothStep(actualObstacleSpeed, obstacleSpeed,
                (obstacleSpeed > actualObstacleSpeed? decceleration : acceleration) * Time.deltaTime * Time.timeScale);
        }

	}

    public float getTimeScale() {
        return timeScale;
    }

    public void pause() {
        gameState = State.PAUSED;
        Time.timeScale = 0;
        //obstacleSpeed = 0;
        //actualObstacleSpeed = 0;
    }

    public void resume() {
        gameState = State.PLAYING;
        Time.timeScale = timeScale;
       // obstacleSpeed = startObstacleMoveSpeed;
    }

	public static float getObstacleSpeed(){
		return actualObstacleSpeed * Time.timeScale;
	}

  

	public static State getGameState(){
		return gameState;
	}

	public enum State {
		MAIN_MENU, PAUSED, CHARACTER_SELECTION, PLAYING, GAMEOVER
	}

    public void goToMainMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void setGameOver()
    {
        gameState = State.GAMEOVER;
    }

    public static void setObstacleSpeed(float speed) {
        obstacleSpeed = speed;
    }
}
