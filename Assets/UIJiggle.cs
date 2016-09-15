using UnityEngine;
using System.Collections;

public class UIJiggle : MonoBehaviour {

    public float multiplier;
    public float offset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float xDelta = multiplier * 10f * Mathf.Sin(Time.time + Random.Range(-0.1f, 0.1f) + offset);
        if(GameController.getGameState() == GameController.State.PLAYING && transform.position.y > -500) {
            xDelta = -0.5f;
        }
        transform.position = new Vector3(transform.position.x + xDelta * Time.deltaTime, transform.position.y, transform.position.z);
	}
}
