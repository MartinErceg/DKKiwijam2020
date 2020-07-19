using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public float spawnDistance;

    public GameObject[] TerrainPool;
    public GameObject terrain;
    public GameManager gm;

    private Vector3 lastSpawn;

    void Start()
    {
        TerrainPool = new GameObject[gm.terrainCount];

        Vector3 spawnPos = new Vector3(0,-75,0);

        for(int i = 0; i < gm.terrainCount; i++)
        {
            // TODO: Slecet terrain at random

            TerrainPool[i] = Instantiate(terrain, spawnPos, gm.b_spawner.transform.rotation);

            Bounds terrainBounds = TerrainPool[i].GetComponent<Collider>().bounds;

            // spawnPos.x = player.x;
            spawnPos.y -= terrainBounds.size.y;
            spawnPos.z += terrainBounds.size.z;

            print("terrainbounds: " + terrainBounds);

            /*Vector3 lengthPos = gm.b_goal.GetComponent<Transform>().localPosition - gm.b_spawner.GetComponent<Transform>().localPosition;
            Vector3 splitLength = lengthPos / gm.terrainCount;
            */
            //int newI = i + 1;            

            //float newY = //-(splitLength.y * newI) + gm.b_goal.transform.position.y;
            //float newZ = //-(splitLength.z * newI) + gm.b_goal.transform.position.z;            

            //Vector3 calculatedPos = new Vector3(0, newY, newZ);                       
        }

        lastSpawn = spawnPos;
    }

    private void Update()
    {
        //print("cam " + gm.camera.transform.position.z + " lastSpawn.Z: " +lastSpawn.z + " SpawnDistance: " + spawnDistance + "  CALC: "+ (lastSpawn.z - gm.camera.transform.position.z));
        if((lastSpawn.z - gm.camera.transform.position.z) > spawnDistance)
        {
            bool success = SpawnNewTerrain();

            //if (!success)
            //    print("FAILED TO FIND TERRAIN");
        }
    }

    private bool SpawnNewTerrain()
    {
        for (int i = 0; i < gm.terrainCount; i++)
        {
            Terrain t = TerrainPool[i].gameObject.GetComponent<Terrain>();

            if(t.isDead)
            {
                lastSpawn = new Vector3(gm.camera.transform.position.x, lastSpawn.y, lastSpawn.z);

                t.transform.position = lastSpawn;
                t.isDead = false;
                
                Bounds terrainBounds = t.GetComponent<Collider>().bounds;

                lastSpawn.y -= terrainBounds.size.y;
                lastSpawn.z += terrainBounds.size.z;

                print("*************************** SPAWN");

                return true;
            }
        }

        return false;
    }
}
