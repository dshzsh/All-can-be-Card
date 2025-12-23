using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBjg_8 : Cbullet_10
{

}

public class SBjg_8 : Sbullet_10
{
    public static int bid;
    public override void Init()
    {
        base.Init();
        bid = id;
    }
}