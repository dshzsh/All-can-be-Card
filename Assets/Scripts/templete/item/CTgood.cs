using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTgood : Citem_33
{

}
public class DTgood : DataBase
{

}
public class STgood : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CTgood card = _card as CTgood;
        MsgOnItem msg = _msg as MsgOnItem;
        DTgood config = basicConfig as DTgood;

        Debug.Log(msg.op);
    }
}