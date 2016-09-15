using UnityEngine;
using System.Collections;
using System;

public class ObstacleSettings : MonoBehaviour {

    private string objName = "default";
    private bool dangerous;
    private MeshRenderer render;

    private float alpha = 1;
    // Use this for initialization

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
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
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
