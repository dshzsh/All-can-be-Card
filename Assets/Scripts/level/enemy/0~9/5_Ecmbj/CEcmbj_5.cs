using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CEcmbj_5 : CEbase_29
{

}

public class SEcmbj_5 : SEbase_29
{
    public static int eid;
    public override void Init()
    {
        base.Init();
        eid = id;
    }
    
}