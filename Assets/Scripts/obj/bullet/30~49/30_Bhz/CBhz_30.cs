using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBhz_30 : Cbullet_10
{

}

public class SBhz_30 : Sbullet_10
{
    public override void Init()
    {
        base.Init();
    }
    // 原则上子弹自身不要带有任何绑定的效果，要有也得以buff的形式附加
}