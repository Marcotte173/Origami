using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static Utility instance;
    private void Awake()
    {
        instance = this;
    }
    public Tile Location(Biome b, Transform a)
    {
        foreach(Tile t in b.tileList)
        {
            if (t.x == System.Math.Round(a.position.x) && t.y == System.Math.Round(a.position.y)) return t;
        }
        return null;
    }
    public Tile Location(Biome b, int x, int y)
    {
        foreach (Tile t in b.tileList)
        {
            if (t.x == x && t.y == y) return t;
        }
        return null;
    }
    public void TurnOff(GameObject g)
    {
        if (g.activeSelf) g.SetActive(false);
    }
    public void TurnOn(GameObject g)
    {
        if (!g.activeSelf) g.SetActive(true);
    }
    public List<Tile> AdjecentTiles(Biome b, Tile t)
    {
        List<Tile> list = new List<Tile> { };
        foreach(Tile tile in b.tileList)
        {
            if (t.x == tile.x && t.y == tile.y + 1) list.Add(tile);
            if (t.x == tile.x-1 && t.y == tile.y + 1) list.Add(tile);
            if (t.x == tile.x+1 && t.y == tile.y + 1) list.Add(tile);
            if (t.x == tile.x && t.y == tile.y - 1) list.Add(tile);
            if (t.x == tile.x+1 && t.y == tile.y - 1) list.Add(tile);
            if (t.x == tile.x-1 && t.y == tile.y - 1) list.Add(tile);
            if (t.x == tile.x+1 && t.y == tile.y ) list.Add(tile);
            if (t.x == tile.x-1 && t.y == tile.y ) list.Add(tile);
        }
        return list;
    }

    public List<Tile> Terrain(Biome b,int terrain)
    {
        List<Tile> list = new List<Tile> { };
        foreach (Tile t in b.tileList) if (b.terrainType == terrain) list.Add(t);
        return list;
    }
    public bool InRange(Biome biome,int range, int terrainType, Tile t)
    {
        //Need to fix and make more rebust
        for (int i = 0; i < range; i++)
        {
            if (Location(biome, t.x + i, t.y + j).terrainType == terrainType) return true;
        }
        return false;
    }
}
