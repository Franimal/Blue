using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Vector3 followPosition;
    public Vector3 followRotation;

    private bool gameStarted = false;
    private GameObject player;
    private Vector3 startPosition;
    private Vector3 offset;

    private float speed = 0.3f;

    //These should all be set in setTargetPosition()
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float interpolationSpeed;
    private float interpolationStartTime;
    
	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        targetPosition = transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {

        if(GameController.getGameState() == GameController.State.MAIN_MENU) {

            //return;
        }

	    if(GameController.getGameState() == GameController.State.PLAYING) {
            if (!gameStarted) {
                resume();
                gameStarted = true;
            }
            Vector3 targetPos = new Vector3(startPosition.x,
             player.transform.position.y * 0.2f - offset.y,
             player.transform.position.z * 0.5f - offset.z);
            Quaternion targetRot = Quaternion.Euler(followRotation);
            if(speed < 5.5f) {
                speed += 0.5f * Time.deltaTime;
            }
           
            setTargetPosition(targetPos, targetRot, speed);
        }

        Vector3 pos = Vector3.Slerp(transform.position, targetPosition, Time.deltaTime * interpolationSpeed);
        Quaternion rot = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * interpolationSpeed);

        transform.position = pos;
        transform.rotation = rot;
    }

    void resume() {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = player.transform.position - followPosition;
        startPosition = followPosition;
    }

    void setTargetPosition(Vector3 pos, Quaternion rot, float interpolationSpeed) {
        this.interpolationStartTime = Time.time;
        this.interpolationSpeed = interpolationSpeed;
        this.targetPosition = pos;
        this.targetRotation = rot;
    }
}
