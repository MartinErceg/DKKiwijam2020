using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public GameObject[] TerrainPool;
    public GameObject terrain;
    public GameManager gm;

    void Start()
    {
        TerrainPool = new GameObject[gm.terrainCount];
        for(int i = 0; i < gm.terrainCount; i++)
        {
            Vector3 lengthPos = gm.b_goal.GetComponent<Transform>().localPosition - gm.b_spawner.GetComponent<Transform>().localPosition;
            Vector3 splitLength = lengthPos / gm.terrainCount;

            int newI = i + 1;
            float newY = -(splitLength.y * newI) + gm.b_goal.transform.position.y;
            float newZ = -(splitLength.z * newI) + gm.b_goal.transform.position.z;

            Vector3 calculatedPos = new Vector3(0, newY, newZ);

            TerrainPool[i] = Instantiate(terrain, calculatedPos, gm.b_spawner.transform.rotation);
        }
    }

    void Update()
    {
        
    }

}
