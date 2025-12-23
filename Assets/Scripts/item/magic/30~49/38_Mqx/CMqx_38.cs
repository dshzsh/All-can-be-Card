using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class CMqx_38 : Cmagicbase_17
{
    public int NowNum 
    { 
        get 
        {
            DMqx_38 config = DataManager.GetConfig<DMqx_38>(id);
            int num = config.initNum;
            var bif = Sbuff_35.GetBuff(this, SFqx_13.bid);
            if (bif != null) num += (int)((bif.buff as Citem_33).pow + MyMath.SmallFloat);
            return num;
        }
    }
}
public class DMqx_38 : DataBase
{
    public float damage;
    public int initNum, addNum;
    public float intervalTime;
}
public class SMqx_38 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBqx_21));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMqx_38 card = _card as CMqx_38;
        DMqx_38 config = basicConfig as DMqx_38;

        int num = config.initNum;
        var bif = Sbuff_35.GetBuff(card, SFqx_13.bid);
        if (bif != null) num += (int)((bif.buff as Citem_33).pow + MyMath.SmallFloat);

        // 密集指数，随着数量上升，发射间隔缩小
        float dense = Mathf.Sqrt(num) / Mathf.Sqrt(config.initNum);
        for (int i = 0; i < num; i++)
        {
            MsgBullet bmsg = new MsgBullet(msg);
            // 以原始的位置为圆心，dir为法线，产生的子弹在那个大圆上随机num越靠后范围可能越大
            float maxR = Mathf.Sqrt(i) / 5;
            bmsg.initPos = MyMath.GetRandomPointOnCircle(bmsg.initPos, bmsg.dir, Random.Range(0, maxR));
            bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk) / config.damage;
            bmsg.bulletPow *= config.damage;
            Sbullet_10.GiveBullet(bid, bmsg, config.intervalTime / dense * i);
        }

        // 只有在战斗中才会叠加buff
        if(!SLFfightBase_6.InFight())
        {
            CFqx_13 buff = CreateCard<CFqx_13>();buff.pow = config.addNum;
            Sbuff_35.GiveBuff(msg.live, msg.live, new MsgBeBuff(buff, Sbuff_35.BuffSpeTime.Inf,
                beBuffMode: Sbuff_35.BeBuffMode.stackPow));
        }
    }
}