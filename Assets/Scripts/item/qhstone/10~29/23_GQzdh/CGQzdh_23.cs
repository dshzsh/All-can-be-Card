using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQzdh_23 : CGqhsbase_11
{
    public float time;
}
public class DGQzdh_23 : DataBase
{
    public float powRate;
    public float minInterval;
}
public class SGQzdh_23 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CGQzdh_23 card = _card as CGQzdh_23;
        MsgUpdate msg = _msg as MsgUpdate;
        DGQzdh_23 config = basicConfig as DGQzdh_23;

        if(Sqhc_38.GetQhMagic(card) is Cmagicbase_17 magic)
        {
            float cd = Mathf.Max(config.minInterval, magic.mdata.cd);
        
            if(MyTool.IntervalTime(cd, ref card.time, msg.time))
            {
                MsgMagicUse umsg = new MsgMagicUse(msg.cobj, magic, SShoulderCamera_37.GetLookPos(card));
                umsg.ToNoCost();
                umsg.pow *= card.pow * config.powRate;
                Smagic_14.UseMagicWithBA(umsg);
            }
        }
    }
}