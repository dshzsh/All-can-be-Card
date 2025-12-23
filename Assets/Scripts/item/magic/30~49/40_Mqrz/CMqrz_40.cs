using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMqrz_40 : Cmagicbase_17
{

}
public class DMqrz_40 : DataBase
{
    public float damage;
    public int cnt;
    public float distance;
    public float dirShift;
    public float interval;
    public DbasicAtt.AttAndValueData attAndValue;

    public AttAndRevise attAndRevise;
    public override void Init(int id)
    {
        base.Init(id);
        attAndRevise = attAndValue.ToRevise();
    }
}
public class SMqrz_40 : Smagicbase_17
{
    public static int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBqrz_22));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMqrz_40 card = _card as CMqrz_40;
        DMqrz_40 config = basicConfig as DMqrz_40;

        Vector3 pos = msg.pos;
        if (TryGetCobj(card, out var cobj))
        {
            pos = cobj.obj.Center;
        }

        for(int i = 0;i<config.cnt;i++)
        {
            MsgBullet bmsg = new MsgBullet(msg);
            bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);

            // 在一个大球的边缘向中心斩击
            bmsg.initPos = Random.insideUnitSphere.normalized * config.distance + pos;
            bmsg.dir = (pos - bmsg.initPos).normalized;
            bmsg.dir = MyTool.RandScatter(bmsg.dir, config.dirShift, true);

            // 添加额外效果
            CFattChange_10 buff = CreateCard<CFattChange_10>();
            buff.attAndRevise = config.attAndRevise;

            MsgBeBuff msgBeBuff = new MsgBeBuff(buff, Sbuff_35.BuffSpeTime.ToFightEnd, id, Sbuff_35.BeBuffMode.stackPow);
            ScolAddBuff_43.AddBuff(bmsg, msgBeBuff);

            Sbullet_10.GiveBullet(bid, bmsg, i * config.interval);
        }
    }
}