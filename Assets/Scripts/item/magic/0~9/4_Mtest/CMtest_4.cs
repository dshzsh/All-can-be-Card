using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMtest_4 : Cmagicbase_17
{

}
public class DMtest_4 : DataBase
{
    
}
public class SMtest_4 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMtest_4 card = _card as CMtest_4;
        DMtest_4 config = basicConfig as DMtest_4;

        SGby_50.AddPermanentAtt(card,
            new AttAndRevise(BasicAttID.atk, new BasicAtt(1)),
            new AttAndRevise(BasicAttID.healthMax, new BasicAtt(1)));
    }
    private void GiveThing(CMtest_4 card, MsgMagicUse msg)
    {
        CLTkndsj_4 thing = CreateCard<CLTkndsj_4>();
        AddToWorld(thing);
        thing.obj.transform.position = msg.pos;
    }
    private void GiveEnemy(CMtest_4 card, MsgMagicUse msg)
    {
        SLFfightBase_6.GiveEnemyCsm(SCmap_45.nowFloor, GetTypeId(typeof(CEzft_1)));
    }
}