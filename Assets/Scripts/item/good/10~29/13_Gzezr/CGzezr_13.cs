using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGzezr_13 : Citem_33
{

}
public class DGzezr_13 : DataBase
{

}
public class SGzezr_13 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGzezr_13 card = _card as CGzezr_13;
        MsgOnItem msg = _msg as MsgOnItem;
        DGzezr_13 config = basicConfig as DGzezr_13;

    }
}