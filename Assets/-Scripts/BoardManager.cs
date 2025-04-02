using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ladder
{
    public RectTransform fromTile;
    public RectTransform toTile;
}

public class BoardManager : MonoBehaviour
{
    public List<RectTransform> tiles;
    public List<Ladder> ladders;
}
