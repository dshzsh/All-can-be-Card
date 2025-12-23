using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGjmz_37 : Citem_33
{

}
public class DGjmz_37 : DataBase
{
    public float buffTime;
    public float damAdd;
}
public class SGjmz_37 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(SLFfightBase_6.mTSummonEnemy, SummonEnemy);
        AddHandle(MsgType.GiveDamageBefore, GiveDamage);
    }
    void GiveDamage(CardBase _card, MsgBase _msg)
    {
        CGjmz_37 card = _card as CGjmz_37;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGjmz_37 config = basicConfig as DGjmz_37;

        if (SFwfsf_12.InWfsf(msg.to))
            msg.damage *= config.damAdd + 1;
    }
    void SummonEnemy(CardBase _card, MsgBase _msg)
    {
        CGjmz_37 card = _card as CGjmz_37;
        SLFfightBase_6.MsgSummonEnemy msg = _msg as SLFfightBase_6.MsgSummonEnemy;
        DGjmz_37 config = basicConfig as DGjmz_37;

        Sbuff_35.GiveBuff(GetTop(card), msg.enemy, new MsgBeBuff(CreateCard<CFwfsf_12>(), 
            config.buffTime * card.pow, 0, Sbuff_35.BeBuffMode.coverByBig));
    }
}