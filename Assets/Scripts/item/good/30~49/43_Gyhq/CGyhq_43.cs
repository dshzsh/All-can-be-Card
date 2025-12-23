using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGyhq_43 : Citem_33
{

}
public class DGyhq_43 : DataBase
{
    public float costReduce;
}
public class SGyhq_43 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(SNshop_3.mTShopStart, ShopStart);
    }
    void ShopStart(CardBase _card, MsgBase _msg)
    {
        CGyhq_43 card = _card as CGyhq_43;
        SNshop_3.MsgShopStart msg = _msg as SNshop_3.MsgShopStart;
        DGyhq_43 config = basicConfig as DGyhq_43;

        msg.shop.cost = (int)(msg.shop.cost * (1 + config.costReduce));
    }
}