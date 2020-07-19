using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{    
    public MeshRenderer mesh;
    public float velocity;
    public GameManager gm;
    public MeshCollider collider;
    public GameObject startSpawn;
    public GameObject endSpawn;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.activeSelf)
            CheckCameraPosition();
    }

    void CheckCameraPosition()
    {
        float colliderSize = collider.bounds.size.z;
        //print("size: " + collider.bounds.size.z);
        if ((this.transform.position.z + colliderSize) < gm.camera.transform.position.z)
        {
            this.gameObject.SetActive(false);
        }
    }

}
