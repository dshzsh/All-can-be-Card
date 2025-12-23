using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBlj_2 : Cbullet_10
{

}

public class SBlj_2 : Sbullet_10
{
    public static int bid;
    public override void Init()
    {
        base.Init();
        bid = id;
    }
}