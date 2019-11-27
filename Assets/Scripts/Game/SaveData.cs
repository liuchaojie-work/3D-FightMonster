using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<int> livingTarfetPositions = new List<int>();
    public List<int> livingMonsterTypes = new List<int>();

    public int shootNum = 0;
    public int score = 0;
}
