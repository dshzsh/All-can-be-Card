using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGdqd_42 : Citem_33
{

}
public class DGdqd_42 : DataBase
{
    public float coin;
}
public class SGdqd_42 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(Sbag_40.mTMyFirstGetItem, MyFirstGetItem);
    }
    void MyFirstGetItem(CardBase _card, MsgBase _msg)
    {
        CGdqd_42 card = _card as CGdqd_42;
        MsgGetItem msg = _msg as MsgGetItem;
        DGdqd_42 config = basicConfig as DGdqd_42;

        SGqb_24.GetCoin(msg.container, config.coin * card.pow);
    }
}