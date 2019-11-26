﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "CreateManagerVarsContainer")]
public class ManagerVars : ScriptableObject
{
    public static ManagerVars GetManagerVars()
    {
        return Resources.Load<ManagerVars>("ManagerVarsContainer");
    }
    public List<Sprite> bgThemeSpriteList = new List<Sprite>();
    public List<Sprite> platformThemeSpriteList = new List<Sprite>();
    public List<Sprite> skinSpriteList = new List<Sprite>();
    public List<Sprite> characterSkinSpriteList = new List<Sprite>();
    public List<string> skinNameList = new List<string>();
    public List<int> skinPrice = new List<int>();

    public GameObject skinChooseItemPrefab;
    public GameObject characterPrefabs;
    public GameObject normalPlatform;
    public List<GameObject> commonPlatformGroup = new List<GameObject>();
    public List<GameObject> grassPlatformGroup = new List<GameObject>();
    public List<GameObject> winterPlatformGroup = new List<GameObject>();
    public GameObject spikePlatformLeft;
    public GameObject spikePlatformRight;
    public GameObject diamondPrefab;
    public GameObject deathEffect;
    public float nextXPos = 0.554f;
    public float nextYPos = 0.645f;

    public AudioClip jumpCLip;
    public AudioClip fallClip;
    public AudioClip hitClip;
    public AudioClip diamondClip;
    public AudioClip buttonClip;

    public Sprite musicOn;
    public Sprite musicOff;


}
