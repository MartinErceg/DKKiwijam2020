﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoxCollider b_spawner;
    public BoxCollider b_goal;    
    public Camera camera;
    public int terrainSpawnCount;

    public Vector3 lastSpawn;

}
