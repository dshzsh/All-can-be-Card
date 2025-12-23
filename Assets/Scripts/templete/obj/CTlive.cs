using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTlive : CEbase_29
{

}
public class STlive : SEbase_29
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CTlive card = _card as CTlive;
        MsgOnItem msg = _msg as MsgOnItem;

        Debug.Log(msg.op);
    }
}