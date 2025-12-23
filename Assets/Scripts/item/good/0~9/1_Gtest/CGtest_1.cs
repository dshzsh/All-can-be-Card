using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGtest_1 : Citem_33
{

}
public class DGtest_1 : DataBase
{

}
public class SGtest_1 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.MyInteractItem, MyInteractItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGtest_1 card = _card as CGtest_1;
        MsgOnItem msg = _msg as MsgOnItem;
        DGtest_1 config = basicConfig as DGtest_1;
        
        //Debug.Log(msg.op);
    }
    void MyInteractItem(CardBase _card, MsgBase _msg)
    {
        CGtest_1 card = _card as CGtest_1;
        MsgInteractItem msg = _msg as MsgInteractItem;

        Debug.Log(msg.live);
    }
}