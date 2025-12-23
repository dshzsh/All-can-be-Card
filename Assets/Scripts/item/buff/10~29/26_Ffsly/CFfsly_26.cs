using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFfsly_26 : CFlyxg_25
{
    public AttAndRevise att;
    public float heal;
}
public class DFfsly_26 : DataBase
{

}
public class SFfsly_26 : SFlyxg_25
{
    public override void Init()
    {
        base.Init();
    }
    private void AddBuff(CFfsly_26 card, float bulletPow, CObj_2 obj)
    {
        CFattChange_10 buff = CreateCard<CFattChange_10>();
        buff.attAndRevise = card.att;
        buff.pow = bulletPow;
        // Debug.Log(card.uid);

        Sbuff_35.GiveBuff(Sysw_26.GetYsw(card), obj, new MsgBeBuff(buff,
            card.interval * buffIntervalScale, card.uid, Sbuff_35.BeBuffMode.coverByNew));
    }
    public override void EnterField(CardBase _card, float bulletPow, CObj_2 obj, int op, MsgCollision msg)
    {
        CFfsly_26 card = _card as CFfsly_26;
        if (!Slive_19.TeamSatisfy(msg.cobj.team, obj.team, Slive_19.FindLiveMode.friend)) return;

        AddBuff(card, bulletPow, obj);
    }
    public override void UpdateField(CardBase _card, float bulletPow, CObj_2 obj, MsgUpdate msg)
    {
        CFfsly_26 card = _card as CFfsly_26;
        if (!Slive_19.TeamSatisfy(msg.cobj.team, obj.team, Slive_19.FindLiveMode.friend)) return;

        CardBase from = Sysw_26.GetYsw(card);
        Shealth_4.GiveHeal(from, obj, new MsgBeHeal(card.heal * bulletPow));

        AddBuff(card, bulletPow, obj);
    }
}