using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectList : MonoBehaviour
{
    public static GameObjectList instance;
    public Tile tile;
    public Biome biome;
    public Agent agent;
    private void Awake()
    {
        instance = this;
    }
}
