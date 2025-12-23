using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGxdp_7 : Citem_33
{

}
public class DGxdp_7 : DataBase
{

}
public class SGxdp_7 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGxdp_7 card = _card as CGxdp_7;
        MsgOnItem msg = _msg as MsgOnItem;
        DGxdp_7 config = basicConfig as DGxdp_7;

        //Debug.Log(msg.op);
    }
}