using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject follow;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - follow.transform.position;       
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = follow.transform.position + offset;        
    }
}
