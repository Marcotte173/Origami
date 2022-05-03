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
    public void Agent(Biome b, int animalType,int x, int y)
    {
        Agent a = Instantiate(GameObjectList.instance.agent, b.transform);
        a.transform.position = new Vector2(x, y);
        b.agentList.Add(a);
        if(animalType == 0)
        {
            a.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.gazelle;
        }
        else if(animalType == 1)
        {
            a.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.bison;
        }
        else if (animalType == 2)
        {
            a.GetComponent<SpriteRenderer>().sprite = SpriteList.instance.horse;
        }
    }
}
