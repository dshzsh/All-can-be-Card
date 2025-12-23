using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBzft_26 : Cbullet_10
{

}

public class SBzft_26 : Sbullet_10
{
    public static int bid;
    public override void Init()
    {
        base.Init();
        bid = id;
    }
    // 原则上子弹自身不要带有任何绑定的效果，要有也得以buff的形式附加
}