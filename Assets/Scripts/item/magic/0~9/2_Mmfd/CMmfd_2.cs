using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMmfd_2 : Cmagicbase_17
{

}
public class DMmfd_2 : DataBase
{
    public float effect;
}
public class SMmfd_2 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMmfd_2 card = _card as CMmfd_2;
        DMmfd_2 config = DataManager.GetConfig<DMmfd_2>(id);

        MsgBullet bmsg = new MsgBullet(msg);
        Chealth_4 chealth_4 = CreateCard<Chealth_4>();
        chealth_4.nowHealth = chealth_4.GetAtt(BasicAttID.healthMax).value = config.effect * msg.pow * Shealth_4.GetAtt(card, BasicAttID.healthMax).GetValue();

        Cquickdie_34 cquickdie_34 = CreateCard<Cquickdie_34>();
        bmsg.addCards.Add(chealth_4); bmsg.addCards.Add(cquickdie_34);
        bmsg.initPos += bmsg.dir;
        Sbullet_10.GiveBullet(GetTypeId(typeof(CBmfhd_1)), bmsg);
    }
}