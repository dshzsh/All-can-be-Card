using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGch_25 : Citem_33
{

}
public class DGch_25 : DataBase
{

}
public class SGch_25 : Sitem_33
{
    public static int sid;
    public override void Init()
    {
        base.Init();
        sid = id;
    }
}