using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour {

    public ParticleSystem splash;

    public ParticleSystem[] jumpParticles;

    private bool canSplashAgain = true;
	// Use this for initialization
	void Start () {
        splash.GetComponent<Renderer>().material.renderQueue = 3002;

        foreach(ParticleSystem p in jumpParticles){
            p.GetComponent<Renderer>().material.renderQueue = 3002;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(GameController.getGameState() == GameController.State.MAIN_MENU) {
            return;
        }
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        if (velocity.y > 0) {
            canSplashAgain = true;
        }
       if(canSplashAgain && velocity.y < -3 && transform.position.y < 1) {
            splash.Play();
            canSplashAgain = false;
       }
	}

    public void jump() {
        foreach (ParticleSystem p in jumpParticles) {
            p.Play();
        }
    }

    public void dive() {
        canSplashAgain = false;
        foreach (ParticleSystem p in jumpParticles) {
            p.Play();
        }
    }
}
