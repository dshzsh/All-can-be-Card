using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CchargeTime_44 : CardBase
{
    public float time;
    public float percent;
}

public class SchargeTime_44: SystemBase
{
    public static int cid;
    public override void Init()
    {
        base.Init();
        cid = id;
    }
}