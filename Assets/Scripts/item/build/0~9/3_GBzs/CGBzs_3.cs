using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGBzs_3 : CGBbase_1
{

}
public class DGBzs_3 : DataBase
{

}
public class SGBzs_3 : SGBbase_1
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGBzs_3 card = _card as CGBzs_3;
        MsgOnItem msg = _msg as MsgOnItem;
        DGBzs_3 config = basicConfig as DGBzs_3;

    }
}