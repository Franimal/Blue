using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleGenerator : MonoBehaviour {

	//Array of GameObject prefabs to use as obstacles.
	//Should all contain an "ObstacleSettings" script 
	//that contains the coordinates we can create the 
	//obstacle at. (y-height, z-position)
	public GameObject[] obstaclePrefabs;

	public float minObstacleGap = 10f;
	public float maxObstacleGap = 30f;

    public Vector3 deletePosition = new Vector3(-30, 0, 0);

	//The amount of obstacles to have in our world at any time.
	//This will be the size of the List "obstacles".
	public float obstacleAmount = 10f;

	//A list of current, active obstacles in the game.
	//These are added as they are created, so the list
	//is naturally ordered from closest to furthest.
	//The only element that should be removed is [0].
	//Elements should always be added to the end.
	private List<GameObject> obstacles;

	//A list of obstacles in our pool, ready to use
	//as new obstacles.
	private List<GameObject> obstaclePool;

    private float stoppingObstacleSpeed;
    private bool initGameEnd = false;

    private Vector3 spawnPosition = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
		obstacles = new List<GameObject> ();
		obstaclePool = new List<GameObject> ();

		//Now, create [obstacleAmount] of obstacles, and add them to the obstacles list.
		for (int i = 0; i < obstacleAmount; i++) {
			addObstacle ();
		}

	}
	
	// Update is called once per frame
	void Update () {
     
		float speed = GameController.getObstacleSpeed ();

        if (GameController.getGameState() == GameController.State.GAMEOVER)
        {
            if (!initGameEnd)
            {
                initGameEnd = true;
                stoppingObstacleSpeed = speed;
            } else
            {
                stoppingObstacleSpeed *= (0.99f);
            }

            speed = stoppingObstacleSpeed;
        }

        Vector3 moveVec = new Vector3 (speed*Time.deltaTime, 0, 0);

		for (int i = 0; i < obstacles.Count; i++) {
			GameObject ob = obstacles[i];
			ob.transform.Translate (moveVec);

            if((ob.transform.position.x - deletePosition.x) < -20)
            {
                obstacles.Remove(ob);
                obstaclePool.Add(ob);
                ob.GetComponent<Collider>().enabled = false;
                break;
            }
		}

        if(obstacles.Count < obstacleAmount)
        {
            addObstacle();
        }

	}

	void addObstacle(){
		GameObject ob = getRandomObstacle ();
		obstacles.Add (ob);
        ob.GetComponent<Collider>().enabled = true;
    }

	//Gets a random obstacle from our set of prefabs.  Will either get one from the pool,
	//or create one if there aren't any available.
	GameObject getRandomObstacle(){

		GameObject obstacleToReturn = null;

		//Get a random prefab from the array of prefabs "obstaclePrefabs"
		GameObject prefabToUse = obstaclePrefabs[Random.Range (0, obstaclePrefabs.Length)];

		//Get the name of the prefab we want to use, for checking whether it's in the pool.
		string prefabName = prefabToUse.name;
		GameObject pooledObstacle = null;
		//Check whether our pool contains one of these obstacles already
		for(int i = 0; i < obstaclePool.Count; i++){
			//If this obstacles name matches the prefab, save it and break.
			if(obstaclePool[i].GetComponentInChildren<ObstacleSettings>().getName().Equals (prefabName)){
				pooledObstacle = obstaclePool[i];
				break;
			}

		}

		//If we found a matching object, remove it from the pool and keep it saved.
		if (pooledObstacle != null) {
			obstacleToReturn = pooledObstacle;
			obstaclePool.Remove (pooledObstacle);
		}
		//If there was an obj available in the pool, it should now be contained in "obstacleToReturn", 
		//And should no longer be in the pool as we've removed it.

		//If the pool did not contain the object, we now need to Instantiate a new clone
		//of the prefab we've decided to use.
		if (obstacleToReturn == null) {
			obstacleToReturn = (GameObject) Instantiate(prefabToUse, Vector3.zero, prefabToUse.transform.rotation);
            ObstacleSettings settings = obstacleToReturn.GetComponent<ObstacleSettings>();
            settings.setName(prefabName);
            settings.setDangerous(true);
		}

        //We now have a GameObject stored in "obstacleToReturn".  We should now set it's position accordingly.

        //reset it's position to be within the range minObstacleGap -> maxObstacleGap of the last obstacle in
        //"obstacles"
        float lastPosX = spawnPosition.x;
        float add = 0;
        if(obstacles.Count > 0)
        {
            GameObject lastObstacle = obstacles[obstacles.Count-1];
            add = lastObstacle.GetComponent<Collider>().bounds.size.x / 2f;
            lastPosX = lastObstacle.transform.position.x;
        } 

		float createXPosition = lastPosX + Random.Range(minObstacleGap, maxObstacleGap);
		//prefabToUse.GetComponent<ObstacleSettings>.CreateY <-- choose Y with something like this?
		Vector3 translation = new Vector3 (createXPosition, 0, randomLaneZ());
        obstacleToReturn.transform.position = translation;

		return obstacleToReturn;
	}

    float randomLaneZ()
    {
        int rand = (int)Random.Range(0, 3);
    
        switch (rand)
        {
            case 0: return RunnerControls.leftZ;
            case 1: return RunnerControls.centerZ;
            case 2: return RunnerControls.rightZ;
        }
        return RunnerControls.centerZ;
    }

}
