using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMzys_9 : Cmagicbase_17
{

}
public class DMzys_9 : DataBase
{
    public float range;
    public float heal;
}
public class SMzys_9 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMzys_9 card = _card as CMzys_9;
        DMzys_9 config = basicConfig as DMzys_9;

        MsgBullet msgBullet = new MsgBullet(msg);
        Ccolheal_39 cheal = CreateCard<Ccolheal_39>();cheal.heal = config.heal * Shealth_4.GetAttf(card, BasicAttID.healthMax);
        msgBullet.addCards.Add(cheal);
        if (TryGetCobj(msg.live, out var obj))
            msgBullet.initPos = obj.obj.transform.position;
        msgBullet.exScale = MyTool.Vector3Multiply(msgBullet.exScale, new Vector3(config.range * 2, 1, config.range * 2));
        Sbullet_10.GiveBullet(GetTypeId(typeof(CBzys_6)), msgBullet);
    }
}