using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFyzzbj_18 : Cbuffbase_36
{

}
public class DFyzzbj_18 : DataBase
{

}
public class SFyzzbj_18 : Sbuffbase_36
{
    public static int bid;
    public override void Init()
    {
        base.Init();
        bid = id;
    }
}