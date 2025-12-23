using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTatt : CGAattbase_1
{

}
public class STatt : SGAattbase_1
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CTatt card = _card as CTatt;
        MsgOnItem msg = _msg as MsgOnItem;

        Debug.Log(msg.op);
    }
}