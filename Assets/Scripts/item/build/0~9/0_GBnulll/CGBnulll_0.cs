using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGBnulll_0 : Citem_33
{

}
public class DGBnulll_0 : DataBase
{

}
public class SGBnulll_0 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGBnulll_0 card = _card as CGBnulll_0;
        MsgOnItem msg = _msg as MsgOnItem;
        DGBnulll_0 config = basicConfig as DGBnulll_0;

        Debug.Log(msg.op);
    }
}