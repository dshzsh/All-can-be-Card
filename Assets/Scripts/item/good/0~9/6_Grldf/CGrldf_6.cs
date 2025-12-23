using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGrldf_6 : Citem_33
{

}
public class DGrldf_6 : DataBase
{

}
public class SGrldf_6 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGrldf_6 card = _card as CGrldf_6;
        MsgOnItem msg = _msg as MsgOnItem;
        DGrldf_6 config = basicConfig as DGrldf_6;

        //Debug.Log(msg.op);
    }
}