using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCfl_42 : Cbuffbase_36
{
    public float timePer;
    public float flPow;
    public float cnt;
    public float scatter, posShift;
}
public class DGQCfl_42 : DataBase
{

}
public class SGQCfl_42 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CGQCfl_42 card = _card as CGQCfl_42;
        MsgUpdate msg = _msg as MsgUpdate;

        if (!TryGetCobj(card, out var cobj)) return;
        if (cobj is not Cbullet_10 cbullet) return;
        if (cbullet.timeDes == null) return;

        if (!(cbullet.timeDes.timeRes <= cbullet.timeDes.timeMax * card.timePer))
            return;

        // 这个额外效果被销毁
        card.buffInfo.time = 0f;
        float size = Sbullet_10.GetBulletSize(cbullet);

        for(int i=0;i<card.cnt;i++)
        {
            MsgBullet bmsg = new MsgBullet(cbullet);
            bmsg.MulPow(card.flPow);
            bmsg.dir = MyTool.RandScatter(bmsg.dir, 10);
            bmsg.initPos += size * card.posShift * Random.insideUnitSphere;
            Sbullet_10.GiveBullet(cbullet.id, bmsg);
        }

        DestroyCard(cbullet);
    }
}