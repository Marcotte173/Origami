using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteList : MonoBehaviour
{
    public static SpriteList instance;
    public List<Sprite> grass;
    public List<Sprite> water;
    public List<Sprite> tallGrass;
    public List<Sprite> trees; 
    public List<Sprite> rocks;
    public Sprite gazelle;
    public Sprite bison;
    public Sprite horse;
    public Sprite sand;
    private void Awake()
    {
        instance = this;
    }
}
