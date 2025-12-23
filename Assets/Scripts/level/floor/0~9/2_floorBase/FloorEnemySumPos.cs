using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorEnemySumPos : MonoBehaviour
{
    public enum PosType
    {
        enemy,
        player,
        end
    }
    public PosType type;
    public int turn = 0;
}
