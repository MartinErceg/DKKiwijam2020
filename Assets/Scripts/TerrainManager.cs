using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public float spawnDistance; 
    
    public GameObject[] TerrainObjects;
    public int terrainInstances;

    public GameManager gm;

    private Vector3 lastSpawn;   
    
    [HideInInspector]
    public GameObject[] TerrainPool;

    void Start()
    {
        TerrainPool = new GameObject[terrainInstances * TerrainObjects.Length];       
        
        int j = 0;
        int terrainIndex = 0;

        // Instantiate       
        foreach(GameObject t in TerrainObjects)
        {           
            for(int i = 0; i < terrainInstances; i++)
            {
                TerrainPool[j] = Instantiate(TerrainObjects[terrainIndex], new Vector3(0, 0, -1000), gm.b_spawner.transform.rotation);
                TerrainPool[j].gameObject.SetActive(false);
                j++;
            }
            terrainIndex++;
        }

        Vector3 spawnPos = new Vector3(0,-75,0);
        lastSpawn = spawnPos;

        SpawnNewTerrain();
        SpawnNewTerrain();
        SpawnNewTerrain();
        /*
        // Spawn
        for(int i = 0; i < gm.terrainSpawnCount; i++)
        {
            int spawnIndex = Random.Range(0, TerrainPool.Length-1);
            print(spawnIndex);

            GameObject go = TerrainPool[spawnIndex];
            
            go.transform.position = spawnPos;
           
            Bounds terrainBounds = go.GetComponent<Collider>().bounds;
   
            spawnPos.y -= terrainBounds.size.y;
            spawnPos.z += terrainBounds.size.z;

        }*/
    }

    private void Update()
    {
        //print("cam " + gm.camera.transform.position.z + " lastSpawn.Z: " +lastSpawn.z + " SpawnDistance: " + spawnDistance + "  CALC: "+ (lastSpawn.z - gm.camera.transform.position.z));
        if((lastSpawn.z - gm.camera.transform.position.z) > spawnDistance)
        {
            bool success = SpawnNewTerrain();

            if(!success)
                print("FAILED TO FIND TERRAIN");
        }
    }
    
    private bool SpawnNewTerrain()
    {
        for(int i = 0; i < TerrainPool.Length; i++)
        {
            if(!TerrainPool[i].gameObject.activeSelf)
            {
                TerrainPool[i].gameObject.SetActive(true);

                /*Terrain terrainObj = TerrainPool[i].GetComponent<Terrain>();

                float offsetY = terrainObj.endSpawn.transform.position.y - terrainObj.startSpawn.transform.position.y;
                float offsetZ = terrainObj.endSpawn.transform.position.z - terrainObj.startSpawn.transform.position.z;

                //Bounds terrainBounds = TerrainPool[i].GetComponent<Collider>().bounds;             
                TerrainPool[i].transform.position = new Vector3(gm.camera.transform.position.x, lastSpawn.y + (offsetY), lastSpawn.z + (offsetZ));

                lastSpawn.y = terrainObj.transform.position.y;
                lastSpawn.z = terrainObj.transform.position.z;*/

                Terrain terrainObj = TerrainPool[i].GetComponent<Terrain>();
                TerrainPool[i].transform.position = new Vector3(gm.camera.transform.position.x, lastSpawn.y, lastSpawn.z);

                Bounds terrainBounds = TerrainPool[i].GetComponent<Collider>().bounds;             

                lastSpawn.y -= Mathf.Abs(terrainBounds.size.y);
                lastSpawn.z += terrainBounds.size.z;

                return true;
            }
        }

        return false;
    }
}
