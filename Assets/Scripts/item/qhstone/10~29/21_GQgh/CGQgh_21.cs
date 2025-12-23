using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQgh_21 : CGqhsbase_11
{
    public Color color;
}
public class DGQgh_21 : DataBase
{
    public float intensity;
}
public class SGQgh_21 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGQgh_21 card = _card as CGQgh_21;
        card.color = MyRGB.ToFadeColor(MyRandom.RandColor(), 0.5f);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQgh_21 card = _card as CGQgh_21;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQgh_21 config = basicConfig as DGQgh_21;

        CGQCgh_22 add = CreateCard<CGQCgh_22>();
        add.color = card.color;
        add.intensity = config.intensity * card.pow;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, add);
    }
}