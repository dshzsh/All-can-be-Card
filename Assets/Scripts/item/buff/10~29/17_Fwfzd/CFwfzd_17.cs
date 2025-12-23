using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFwfzd_17 : Cbuffbase_36
{

}
public class DFwfzd_17 : DataBase
{

}
public class SFwfzd_17 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFwfzd_17 card = _card as CFwfzd_17;
        MsgOnItem msg = _msg as MsgOnItem;

        if (!TryGetCobj(card, out var cobj)) return;

        cobj.obj.AddWfzd(msg.op);
    }
}