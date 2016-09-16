using UnityEngine;
using System.Collections;

public class GameoverText : MonoBehaviour {

    public Vector3 gameover_menu_position;

    private RectTransform rectTransform;

    // Use this for initialization
    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update () {
        if (GameController.getGameState() == GameController.State.GAMEOVER && rectTransform.anchoredPosition.y - gameover_menu_position.y > 0.1f) {
            float yDelta = -300f;
            rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + yDelta * Time.deltaTime);
        }      
    }
}
