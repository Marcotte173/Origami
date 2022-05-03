using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static World instance;
    public List<Biome> world;
    public Biome currentBiome;
    private void Awake()
    {
        instance = this;
    }

    
}
