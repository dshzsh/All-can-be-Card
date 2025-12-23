using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQtlzs_33 : CGqhsbase_11
{

}
public class DGQtlzs_33 : DataBase
{
    public float coin;
}
public class SGQtlzs_33 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.UseMagicBefore, UseMagicBefore);
    }
    void UseMagicBefore(CardBase _card, MsgBase _msg)
    {
        CGQtlzs_33 card = _card as CGQtlzs_33;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQtlzs_33 config = basicConfig as DGQtlzs_33;

        CGQCtlzs_34 mk = CreateCard<CGQCtlzs_34>();
        mk.coin = config.coin * card.pow;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, mk);
    }
}