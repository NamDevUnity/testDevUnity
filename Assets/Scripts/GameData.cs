using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public List<BlockData> blocks = new List<BlockData>();
    public int[] blockQuantities;
}
