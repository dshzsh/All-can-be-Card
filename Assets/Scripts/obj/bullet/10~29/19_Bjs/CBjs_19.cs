using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBjs_19 : Cbullet_10
{

}

public class SBjs_19 : Sbullet_10
{
    public static int bid;
    public override void Init()
    {
        base.Init();
        bid = id;
    }
    
}