using UnityEngine;
using System.Collections;
using System;

public class ObstacleSettings : MonoBehaviour {

    private string objName = "default";
    private bool dangerous;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setName(String name)
    {
        this.objName = name;
    }

    public string getName()
    {
        return objName;
    }

    public void setDangerous(bool isDangerous)
    {
        this.dangerous = isDangerous;
    }

    public bool isDangerous()
    {
        return dangerous;
    }
}
