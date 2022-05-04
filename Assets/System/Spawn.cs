using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public static Spawn instance;
    private void Awake()
    {
        instance = this;
    }
    public void Agent(Biome b, int animalType, int x, int y)
    {
        Agent a = Instantiate(GameObjectList.instance.agent, b.transform);
        a.transform.position = new Vector2(x, y);
        b.agentList.Add(a);
        if (animalType == 0)
        {
            a.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.gazelle;
        }
        else if (animalType == 1)
        {
            a.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.bison;
        }
        else if (animalType == 2)
        {
            a.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.horse;
        }
    }
    public void Agent(Biome b, Tile t)
    {
        Agent a = Instantiate(GameObjectList.instance.agent, b.transform);
        a.transform.position = new Vector2(t.x, t.y);
        b.agentList.Add(a);
        float rollMax = 0;
        for (int i = 0; i < t.terrainType.Count; i++) if (t.terrainType[i] > 0) rollMax += t.terrainType[i];
        Debug.Log(rollMax);
        float roll = Random.Range(0, rollMax);
        for (int i = 0; i < t.terrainType.Count; i++)
        {
            if (t.terrainType[i] > 0)
            {
                if (t.terrainType[i] > roll)
                {
                    Debug.Log(i);
                    if(i == 1) a.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.horse;
                    else if(i == 2)
                    {

                    }
                    else if ( i == 3) a.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.gazelle;
                    else if (i == 5) a.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.bison;
                    break;
                }
                else
                {
                    roll -= t.terrainType[i];
                }
            }
        }
    }
}
