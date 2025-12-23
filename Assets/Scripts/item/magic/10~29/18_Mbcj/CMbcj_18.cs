using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMbcj_18 : Cmagicbase_17
{

}
public class DMbcj_18 : DataBase
{
    public float damage;
    public float cold;
    public int cnt;
    public float intervalDis;
    public float intervalTime;
    public float initScale;
    public float addScale;
}
public class SMbcj_18 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();

        bid = GetTypeId(typeof(CBbcj_12));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMbcj_18 card = _card as CMbcj_18;
        DMbcj_18 config = basicConfig as DMbcj_18;
        Vector3 pos = Vector3.zero;
        Vector3 dir = Vector3.zero;
        if (TryGetCobj(card, out var cobj))
        {
            pos = cobj.obj.transform.position;
            dir = MyMath.V3zeroYNor(msg.pos - pos);
        }

        //添加寒冷
        CFcold_6 buff = CreateCard<CFcold_6>(); buff.pow = config.cold;
        MsgBeBuff bebuff = new MsgBeBuff(buff, Sbuff_35.BuffSpeTime.Inf, 0, Sbuff_35.BeBuffMode.stackPow);
        CcolAddBuff_43 colbuff = CreateCard<CcolAddBuff_43>();
        colbuff.bebuff = bebuff;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, colbuff);

        for (int i = 0; i < config.cnt; i++)
        {
            MsgBullet bmsg = new MsgBullet(msg);
            float sc = config.initScale + i * config.addScale;
            pos += config.intervalDis * sc * dir;
            bmsg.initPos = pos;
            bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
            bmsg.exScale *= sc;

            Sbullet_10.GiveBullet(bid, bmsg, i * config.intervalTime);
        }
    }
}