using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;
    public float g;
    public float h;
    public float f;
    public Tile parent;
    public List<Tile> neighbor;
    public int cost;
    public int terrainType;
    public Agent occupiedBy;
}
