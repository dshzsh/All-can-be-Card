using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBbz_24 : Cbullet_10
{

}

public class SBbz_24 : Sbullet_10
{
    public static int bid = 0;
    public override void Init()
    {
        base.Init();
        bid = id;
    }
    
}