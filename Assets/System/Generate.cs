using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Generate : MonoBehaviour
{
    public static Generate instance;    
    public int edgeLikelyHood;
    public float waterPercent;
    public float treePercent;
    public float tallGrassPercent;
    public int rocks;
    public int biomeX;
    public int biomeY;
    private void Start()
    {
        instance = this;
        Camera.main.transform.position = new Vector3(biomeX/2, biomeY/2, -10);
        Camera.main.orthographicSize = biomeX/2;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float a = Time.realtimeSinceStartup;
            CreateWorld();
            float b = Time.realtimeSinceStartup;
            Debug.Log(b - a);
        }
    }
    public void CreateWorld()
    {
        if (World.instance.world.Count > 0)
        {
            foreach (Biome b in World.instance.world.ToList())
            {
                Destroy(b.gameObject);
                World.instance.world.Clear();
            }
        }
        BuildGrassBiome(biomeX, biomeY,1);
        World.instance.currentBiome = World.instance.world[0];
        foreach (Biome b in World.instance.world) if (b != World.instance.currentBiome) Utility.instance.TurnOff(b.gameObject);
    }
    
    private Biome BuildGrassBiome(int a, int b, int terrainType)
    {
        Biome biome = NewBiome(a, b, terrainType);
        ArtisinalTerrain(b, terrainType);
        biome.terrainType = terrainType;
        World.instance.world.Add(biome);
        biome.id = World.instance.world.Count;
        biome.name = "Biome " + biome.id + ": " + ((biome.terrainType == 1) ? "Grass " : (biome.terrainType == 0) ? "Water " : "Sand ");
        AddTallGrass(a, b, biome, tallGrassPercent);
        AddTreeTerrain(a, b, biome, treePercent);
        AddWaterTerrain(a, b, biome, waterPercent);
        AddRocksTerrain(biome, rocks);
        SpawnCreatures(biome,biomeX,biomeY,3,2,0);
        return biome;
    }

    private void SpawnCreatures(Biome biome,int a, int b,int terrainType, int range, int creature)
    {
        int attempt = 30;
        int creatures = 0;
        int maxCreatures = System.Convert.ToInt32(System.Math.Sqrt(System.Convert.ToInt32(a * b * tallGrassPercent)));
        while (creatures < maxCreatures)
        {
            List<Tile> possibleSpawn = new List<Tile> { };
            foreach (Tile t in biome.tileList)
            {
                if (t.terrainType == terrainType || (Utility.instance.InRange(biome,range, terrainType, t) && t.terrainType == biome.terrainType))
                    possibleSpawn.Add(t);
            }
            Tile spawn = possibleSpawn[Random.Range(0, possibleSpawn.Count)];
            possibleSpawn.Remove(spawn);
            Spawn.instance.Agent(biome, creature, spawn.x, spawn.y);
            creatures++;
            attempt--;
            if (attempt <= 0 || possibleSpawn.Count <= 1) break;
        }
    }

    private void AddRocksTerrain(Biome biome, int rocks)
    {
        for (int i = 0; i < rocks; i++)
        {
            Tile theTile = FindEmptyLand(biome, 4,2,6);
            MakeARock(biome, theTile.x, theTile.y, 0);
            MakeARock(biome, theTile.x+1, theTile.y, 1);
            MakeARock(biome, theTile.x+2, theTile.y, 2);
            MakeARock(biome, theTile.x+3, theTile.y, 3);
            MakeARock(biome, theTile.x + 1, theTile.y+1, 4);
            MakeARock(biome, theTile.x + 2, theTile.y+1, 5);
        }
    }

    private void MakeARock(Biome biome, int x, int y, int v)
    {
        Utility.instance.Location(biome, x, y).terrainType = 4;
        Utility.instance.Location(biome, x, y).GetComponent<SpriteRenderer>().sprite = SpriteList.instance.rocks[v];
    }

    private void AddTallGrass(int a, int b, Biome biome, float tallGrassPercent)
    {        
        int terrainSquareTiles = System.Convert.ToInt32(System.Math.Sqrt(System.Convert.ToInt32(a * b * tallGrassPercent)));
        while (terrainSquareTiles > 2)
        {           
            int tilesToUse = Random.Range(3, terrainSquareTiles);
            Tile theTile = FindEmptyLand(biome, tilesToUse);
            if (theTile != null)
            {
                MakeTerrain(biome, tilesToUse, theTile,3);    
                AddEdges(biome,3);
                terrainSquareTiles -= tilesToUse;             
            }            
        }        
    }   

    private void AddTreeTerrain(int a, int b, Biome biome, float treePercent)
    {
        int TerrainSquareTiles = System.Convert.ToInt32(System.Math.Sqrt(System.Convert.ToInt32(a * b * treePercent)));
        while (TerrainSquareTiles > 2)
        {
            int tilesToUse = Random.Range(2, TerrainSquareTiles);
            Tile theTile = FindEmptyLand(biome, tilesToUse);
            if (theTile != null)
            {
                MakeTerrain(biome, tilesToUse, theTile,2);
                AddEdges(biome, 2);         
                TerrainSquareTiles -= tilesToUse;
            }
        }
    }

    private void AddWaterTerrain(int a, int b, Biome biome, float waterPercent)
    {
        int TerrainSquareTiles = System.Convert.ToInt32(System.Math.Sqrt(System.Convert.ToInt32(a * b * waterPercent)));
        while (TerrainSquareTiles > 2)
        {
            int tilesToUse = Random.Range(2, TerrainSquareTiles);
            Tile theTile = FindEmptyLand(biome,tilesToUse);
            if(theTile != null)
            {
                MakeTerrain(biome, tilesToUse, theTile,0);
                AddEdges(biome, 0);                
                TerrainSquareTiles -= tilesToUse;
            }            
        }
        //Add Beach
        foreach (Tile tile in biome.tileList)
        {
            foreach (Tile newTile in tile.neighbor)
            {
                if (newTile.terrainType == 0 && tile.terrainType !=0)
                {
                    UpdateTerrain(tile, 5);
                    break;
                }
            }
        }
    }

    private Tile FindEmptyLand(Biome biome, int size)
    {
        return FindEmptyLand(biome, size, size);
    }
    private Tile FindEmptyLand(Biome biome, int sizeX, int sizeY)
    {
        return FindEmptyLand(biome, sizeX, sizeY,0);
    }
    private Tile FindEmptyLand(Biome biome,int sizeX,int sizeY,int edgeModifier)
    {
        int attempts = 0;
        while (true)
        {
            Tile t = Utility.instance.Location(biome, Random.Range(0, biomeX-edgeModifier), Random.Range(0, biomeY-edgeModifier));
            if (Check(biome, t, sizeX, sizeY)) return t;
            else attempts++;
            if (attempts > 15) return null;
        }
    }

    private bool Check(Biome b, Tile t, int amountX, int amountY)
    {
        amountX = (t.x + amountX < biomeX) ? amountX : biomeX - t.x-1;
        amountY = (t.y + amountY < biomeY) ? amountY : biomeY - t.y - 1;
        for (int i = 0; i < amountY; i++)
        {
            for (int j = 0; j < amountX; j++)
            {
                if (Utility.instance.Location(b, t.x + j, t.y + i).terrainType != b.terrainType) return false;
            }
        }
        return true;
    }

    private void ArtisinalTerrain(int b, int terrainType)
    {
        
    }

    public Biome NewBiome(int a, int b,int terrainType)
    {        
        Biome biome = Instantiate(GameObjectList.instance.biome, transform);
        for (int y = 0; y < a; y++)
        {
            for (int x = 0; x < b; x++)
            {
                Tile l = Instantiate(GameObjectList.instance.tile, biome.transform);
                l.x = x;
                l.y = y;
                biome.tileList.Add(l);
                l.transform.position = new Vector2(l.x, l.y);
                l.name = $"{l.x} , {l.y}";
                UpdateTerrain(l, terrainType);
            }
        }
        foreach (Tile l in biome.tileList)
        {
            foreach (Tile loc in biome.tileList)
            {
                if (loc.x == l.x + 1 && loc.y == l.y) if (!l.neighbor.Contains(loc)) l.neighbor.Add(loc);
                if (loc.x == l.x - 1 && loc.y == l.y) if (!l.neighbor.Contains(loc)) l.neighbor.Add(loc);
                if (loc.x == l.x && loc.y == l.y + 1) if (!l.neighbor.Contains(loc)) l.neighbor.Add(loc);
                if (loc.x == l.x && loc.y == l.y - 1) if (!l.neighbor.Contains(loc)) l.neighbor.Add(loc);

                if (loc.x == l.x + 1 && loc.y == l.y + 1) if (!l.neighbor.Contains(loc)) l.neighbor.Add(loc);
                if (loc.x == l.x - 1 && loc.y == l.y + 1) if (!l.neighbor.Contains(loc)) l.neighbor.Add(loc);
                if (loc.x == l.x + 1 && loc.y == l.y - 1) if (!l.neighbor.Contains(loc)) l.neighbor.Add(loc);
                if (loc.x == l.x - 1 && loc.y == l.y - 1) if (!l.neighbor.Contains(loc)) l.neighbor.Add(loc);
            }
        }     
        return biome;
    }

    private void UpdateTerrain(Tile l,int terrainType)
    {
        l.terrainType = terrainType;
        if(l.terrainType == 0)
        {
            l.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.water[Random.Range(0, SpriteList.instance.water.Count)];
        }
        if(l.terrainType == 1)
        {
            l.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.grass[Random.Range(0, SpriteList.instance.grass.Count)];
        }
        if (l.terrainType == 2)
        {
            l.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.trees[Random.Range(0, SpriteList.instance.trees.Count)];
        }
        if (l.terrainType == 3)
        {
            l.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.tallGrass[Random.Range(0, SpriteList.instance.tallGrass.Count)];
        }
        if (l.terrainType == 4)
        {
            l.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.rocks[Random.Range(0, SpriteList.instance.rocks.Count)];
        }
        if (l.terrainType == 5)
        {
            l.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.sand;
        }
    }

    public void  MakeTerrain(Biome biome,int howBig,Tile where,int terrainType)
    {
        for (int i = 0; i < howBig; i++)
        {
            for (int j = 0; j < howBig; j++)
            {
                if (where.x + j < biomeX && where.y + i < biomeY)
                {
                    UpdateTerrain(Utility.instance.Location(biome, where.x + j, where.y + i), terrainType);
                }
            }
        }
    }
       
    private void AddEdges(Biome biome, int terrainType)
    {
        foreach (Tile t in biome.tileList.ToList())
        {
            if (t.terrainType == terrainType)
            {
                foreach (Tile n in t.neighbor)
                {
                    if (n.terrainType != terrainType)
                    {
                        int terrainNeighbors = 0;
                        foreach (Tile w in n.neighbor) if (w.terrainType == terrainType) terrainNeighbors++;
                        int roll = Random.Range(1, 101);
                        if (roll < terrainNeighbors * edgeLikelyHood)
                        {
                            UpdateTerrain(n, terrainType);
                        }
                    }
                }
            }
        }
    }
}