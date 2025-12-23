using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTqhstone : CGqhsbase_11
{

}
public class DTqhstone : DataBase
{

}
public class STqhstone : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CTqhstone card = _card as CTqhstone;
        MsgOnItem msg = _msg as MsgOnItem;
        DTqhstone config = basicConfig as DTqhstone;

        Debug.Log(msg.op);
    }
}