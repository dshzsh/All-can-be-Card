using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMll_54 : Cmagicbase_17
{

}
public class DMll_54 : DataBase
{
    public float cnt, damage, delay;
}
public class SMll_54 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMll_54 card = _card as CMll_54;
        DMll_54 config = basicConfig as DMll_54;

        if (!TryGetCobj(msg.live, out var cobj)) return;

        for (int i = 0; i < MyTool.FloatToIntRandomly(config.cnt * msg.pow); i++)
        {
            List<Clive_19> mbs = Slive_19.GetLives(cobj, findLiveMaxDis: msg.mdata.useDis);
            Vector3 pos = msg.pos;
            Clive_19 mb = MyRandom.RandomInList(mbs);
            if (mb != default) pos = mb.obj.Center;

            MsgBullet bmsg = new MsgBullet(msg);
            bmsg.initPos = pos;
            bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk) / msg.pow;
            Sbullet_10.GiveBullet(SBlj_2.bid, bmsg, i * config.delay);
        }
    }
}