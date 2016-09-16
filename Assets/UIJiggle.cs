using UnityEngine;
using System.Collections;

public class UIJiggle : MonoBehaviour {

    public float multiplier;
    public float offset;

    public bool move_down_when_playing = false;

    private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () { 
        float yDelta = multiplier * 3000f * Mathf.Sin(Time.time + Random.Range(-0.1f, 0.1f) + offset);
        if(move_down_when_playing && GameController.getGameState() == GameController.State.PLAYING && rectTransform.anchoredPosition.y > -500) {
            yDelta = -300f;
        }

        rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + yDelta * Time.deltaTime);
	}
}
