using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQcolor_39 : CGqhsbase_11
{
    public Color color;
}
public class DGQcolor_39 : DataBase
{

}
public class SGQcolor_39 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGQcolor_39 card = _card as CGQcolor_39;
        card.color = MyRandom.RandColor();
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQcolor_39 card = _card as CGQcolor_39;
        MsgMagicUse msg = _msg as MsgMagicUse;

        CGQCcolor_40 buff = CreateCard<CGQCcolor_40>();
        buff.color = card.color;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, buff);
    }
}