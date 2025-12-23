using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class CGQCyc_53 : Cbuffbase_36
{
    public BasicAtt powAdd;
    public float timeDelay;
    public Color color = Color.red;
}
public class DGQCyc_53 : DataBase
{

}
public class SGQCyc_53 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BulletStart, BulletStart);
    }
    void BulletStart(CardBase _card, MsgBase _msg)
    {
        CGQCyc_53 card = _card as CGQCyc_53;
        MsgOnItem msg = _msg as MsgOnItem;

        if (!TryGetCobj(card, out var cobj)) return;
        if (cobj is not Cbullet_10 cbullet) return;

        // 这个额外效果被销毁
        card.buffInfo.time = 0f;

        // 看上去很蠢，实际上是保持格式统一……大概？
        for (int i = 0; i < 1; i++)
        {
            MsgBullet bmsg = new MsgBullet(cbullet);
            bmsg.MulPow(card.powAdd.UseAttTo(1, 1));
            Sbullet_10.GiveBullet(cbullet.id, bmsg, card.timeDelay);

            MGQCyc_53 magicCircle = GameObject.Instantiate(DGQyc_52.magicCirclePrefab).GetComponent<MGQCyc_53>();
            magicCircle.transform.position = bmsg.initPos;
            magicCircle.transform.forward = bmsg.dir;
            magicCircle.transform.localScale = cbullet.obj.transform.localScale;

            magicCircle.Set(card.timeDelay, card.color);
        }

        DestroyCard(cbullet);
    }
}