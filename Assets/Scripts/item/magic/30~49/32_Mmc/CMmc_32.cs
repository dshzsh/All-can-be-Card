using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMmc_32 : Cmagicbase_17
{

}
public class DMmc_32 : DataBase
{
    public float damage;
    public float buffTime;
}
public class SMmc_32 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMmc_32 card = _card as CMmc_32;
        DMmc_32 config = basicConfig as DMmc_32;

        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        ScolAddBuff_43.AddBuff<CFcm_14>(bmsg, config.buffTime, Sbuff_35.BeBuffMode.coverByBig);
        Sbullet_10.GiveBullet(GetTypeId(typeof(CBmc_18)), bmsg);
    }
}