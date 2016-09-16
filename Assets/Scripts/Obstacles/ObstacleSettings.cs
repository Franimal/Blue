using UnityEngine;
using System.Collections;
using System;

public class ObstacleSettings : MonoBehaviour {

    private string objName = "default";
    private bool dangerous;
    private MeshRenderer render;

    public Vector3[] coinPositions;

    private float alpha = 1;
    // Use this for initialization

    public bool collectable = false;

    public bool three_lanes_wide = false;

    private float startY;

	void Start () {
        
	}

    void Awake() {
        render = GetComponentInChildren<MeshRenderer>();
        alpha = render.material.color.a;
    }
	
	// Update is called once per frame
	void Update () {
        //v.vertex.y -= 0.002 * (v.vertex.x + 70) * (v.vertex.x + 70);
        float yPos = (-0.00055f * ((transform.position.x + 95) * (transform.position.x + 95))) + 5;
        yPos = Mathf.Min(yPos, 0.5f);
        transform.position = new Vector3(transform.position.x, yPos + startY, transform.position.z);
    }

    public void setStartY(float startY) {
        this.startY = startY;
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

    internal Vector3[] GetCoinPositions() {
        return coinPositions;
    }
}
