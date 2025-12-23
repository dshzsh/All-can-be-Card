using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTTzssh_3 : CardBase
{

}

public class STTzssh_3 : SystemBase
{
    public static int tid;
    public override void Init()
    {
        base.Init();
        tid = id;
    }
}