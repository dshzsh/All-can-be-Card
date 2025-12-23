using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGRnulll_0 : Citem_33
{

}
public class DGRnulll_0 : DataBase
{

}
public class SGRnulll_0 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGRnulll_0 card = _card as CGRnulll_0;
        MsgOnItem msg = _msg as MsgOnItem;
        DGRnulll_0 config = basicConfig as DGRnulll_0;

        Debug.Log(msg.op);
    }
}