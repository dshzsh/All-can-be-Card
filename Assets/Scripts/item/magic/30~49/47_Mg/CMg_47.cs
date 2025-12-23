using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMg_47 : Cmagicbase_17
{

}
public class DMg_47 : DataBase
{
    public float chargeTime, damage;
    public float damageUp, speedUp;
}
public class SMg_47 : Smagicbase_17
{
    public int bid;
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        DMg_47 config = basicConfig as DMg_47;
        SGxlmk_12.AddXl(_card, config.chargeTime);
    }
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBgj_28));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMg_47 card = _card as CMg_47;
        DMg_47 config = basicConfig as DMg_47;

        float chargeTime = 0f;
        if (msg.TryGetTag<CchargeTime_44>(out var cc))
            chargeTime = cc.time;
        float damageUp = config.damageUp * chargeTime + 1;

        MsgBullet bmsg = new MsgBullet(msg);

        bmsg.damage = config.damage * damageUp * Shealth_4.GetAttf(card, BasicAttID.atk);
        bmsg.AddInitShift(card);

        BasicAtt speedChange = new BasicAtt();speedChange.finalMul = 1 + config.speedUp * chargeTime;
        speedChange.UseAttTo(bmsg.speed, 1);

        Sbullet_10.GiveBullet(bid, bmsg);
    }
}