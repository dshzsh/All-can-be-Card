using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGgzqh_4 : CGqhsbase_11
{

}
public class DGgzqh_4 : DataBase
{
    public float angleRate;
}
public class SGgzqh_4 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGgzqh_4 card = _card as CGgzqh_4;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGgzqh_4 config = basicConfig as DGgzqh_4;

        CGgzqhAdd_5 addmk = CreateCard<CGgzqhAdd_5>();
        addmk.angleRate = config.angleRate * card.pow;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, addmk);
    }
}