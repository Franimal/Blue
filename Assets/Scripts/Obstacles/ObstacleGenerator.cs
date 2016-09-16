using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleGenerator : MonoBehaviour {

	//Array of GameObject prefabs to use as obstacles.
	//Should all contain an "ObstacleSettings" script 
	//that contains the coordinates we can create the 
	//obstacle at. (y-height, z-position)
	public GameObject[] obstaclePrefabs;

    public GameObject currencyPrefab;

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

    private GameObject gameParent;

	// Use this for initialization
	void Start () {
        gameParent = GameObject.FindGameObjectWithTag("GameParent");
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
			ob.transform.position += moveVec;
    
            if(ob.transform.position.x < -100)
            {
                obstacles.Remove(ob);
                obstaclePool.Add(ob);
                Collider[] colliders = ob.GetComponentsInChildren<Collider>();
                foreach(Collider c in colliders) {
                    c.enabled = false;
                }
            }
		}
   
        if(obstacles[obstacles.Count-1].transform.position.x < 100)
        {
            addObstacle();
        }

	}

	void addObstacle(){
		GameObject ob = getRandomObstacle ();
        ob.transform.parent = gameParent.transform;
        GenerateCoins(ob);
		obstacles.Add (ob);
        Collider[] colliders = ob.GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders) {
            c.enabled = true;
        }
    }

    void GenerateCoins(GameObject obstacle) {

        Vector3[] coinPositions = obstacle.GetComponent<ObstacleSettings>().GetCoinPositions();

        foreach(Vector3 pos in coinPositions) {
            GameObject coin = newObject(currencyPrefab);
            coin.transform.parent = gameParent.transform;
            coin.transform.position = new Vector3(obstacle.transform.position.x + pos.x, obstacle.transform.position.y + pos.y, obstacle.transform.position.z + pos.z);
            coin.GetComponent<ObstacleSettings>().setStartY(coin.transform.position.y);
            obstacles.Add(coin);
        }
      
    }

    GameObject getFromPool(string prefabName) {
        for (int i = 0; i < obstaclePool.Count; i++) {
            if (obstaclePool[i].GetComponentInChildren<ObstacleSettings>().getName().Equals(prefabName)) {
                return obstaclePool[i];
            }
        }
        return null;
    }

    GameObject newObject(GameObject prefab) {

        GameObject pooledObstacle = getFromPool(prefab.name);

		if (pooledObstacle != null) {
			obstaclePool.Remove (pooledObstacle);
            pooledObstacle.transform.position = Vector3.zero;
            return pooledObstacle;
		}
   
        return (GameObject)Instantiate(prefab, Vector3.zero, prefab.transform.rotation);
    }

	//Gets a random obstacle from our set of prefabs.  Will either get one from the pool,
	//or create one if there aren't any available.
	GameObject getRandomObstacle(){

		

		//Get a random prefab from the array of prefabs "obstaclePrefabs"
		GameObject prefabToUse = obstaclePrefabs[Random.Range (0, obstaclePrefabs.Length)];

        GameObject obstacleToReturn = newObject(prefabToUse);
        
        ObstacleSettings settings = obstacleToReturn.GetComponent<ObstacleSettings>();
        settings.setName(prefabToUse.name);
        settings.setDangerous(true);
		

        //We now have a GameObject stored in "obstacleToReturn".  We should now set it's position accordingly.

        //reset it's position to be within the range minObstacleGap -> maxObstacleGap of the last obstacle in
        //"obstacles"
        float lastPosX = spawnPosition.x;
        float add = 0;
        if(obstacles.Count > 0)
        {
            GameObject lastObstacle = obstacles[obstacles.Count - 1];

            add = lastObstacle.GetComponent<Collider>().bounds.size.x / 2f;
            lastPosX = lastObstacle.transform.position.x;
        } 

		float createXPosition = lastPosX + Random.Range(minObstacleGap, maxObstacleGap);
        //prefabToUse.GetComponent<ObstacleSettings>.CreateY <-- choose Y with something like this?
        float z = randomLaneZ();
        if (obstacleToReturn.GetComponent<ObstacleSettings>().three_lanes_wide) {
            z = RunnerControls.centerZ;
        }
        Vector3 translation = new Vector3 (createXPosition, 0, z);
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
