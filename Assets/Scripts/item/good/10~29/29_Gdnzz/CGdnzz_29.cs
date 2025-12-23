using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGdnzz_29 : Citem_33
{

}
public class DGdnzz_29 : DataBase
{
    public float damageReduce;
    public float speedMax;
    public float buffTime;
    public DbasicAtt.AttAndValueData speedUpData;

    public AttAndRevise speedUp;
    public override void Init(int id)
    {
        base.Init(id);
        speedUp = speedUpData.ToRevise();
    }
}
public class SGdnzz_29 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeDamageBefore, BeDamageBefore);
        AddHandle(MsgType.BeDamageAfter, BeDamageAfter);
    }
    void BeDamageBefore(CardBase _card, MsgBase _msg)
    {
        CGdnzz_29 card = _card as CGdnzz_29;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGdnzz_29 config = basicConfig as DGdnzz_29;

        if (!TryGetCobj(card, out var cobj, true)) return;

        float speed = cobj.obj.rbody.velocity.magnitude;
        float reduce = MyMath.LinerMap(0, config.speedMax, 0, config.damageReduce, speed);
        msg.damage = (1 - reduce) * msg.damage;
    }
    void BeDamageAfter(CardBase _card, MsgBase _msg)
    {
        CGdnzz_29 card = _card as CGdnzz_29;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGdnzz_29 config = basicConfig as DGdnzz_29;

        CFattChange_10 buff = CreateCard<CFattChange_10>();
        buff.attAndRevise = config.speedUp.WithPow(card.pow);
        Sbuff_35.GiveBuff(GetTop(card), GetTop(card), new MsgBeBuff(buff, config.buffTime, id, Sbuff_35.BeBuffMode.coverByBig));
    }
}