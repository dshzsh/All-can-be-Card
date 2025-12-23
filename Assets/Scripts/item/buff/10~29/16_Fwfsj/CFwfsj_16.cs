using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFwfsj_16 : Cbuffbase_36
{

}
public class DFwfsj_16 : DataBase
{

}
public class SFwfsj_16 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeDamageBefore, BeDamageBefore, HandlerPriority.Highest);
    }
    void BeDamageBefore(CardBase _card, MsgBase _msg)
    {
        _msg.valid = false;
    }
}