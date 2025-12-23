using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQjd_73 : CGqhsbase_11
{

}
public class DGQjd_73 : DataBase
{
    public DbasicAtt.AttAndReviseData att1Data, att2Data;

    public AttAndRevise att1, att2;
    public float time;
    public override void Init(int id)
    {
        base.Init(id);
        att1 = att1Data.ToRevise();
        att2 = att2Data.ToRevise();
    }
}
public class SGQjd_73 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, priorityQhstoneMagicUse);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQjd_73 card = _card as CGQjd_73;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQjd_73 config = basicConfig as DGQjd_73;

        CFattChange_10 buff = CreateCard<CFattChange_10>();
        buff.attAndRevise = config.att1;
        buff.attAndRevise2 = config.att2;
        buff.pow = card.pow;
        MsgBeBuff bmsg = new MsgBeBuff(buff, config.time, id, Sbuff_35.BeBuffMode.coverByBig);
        CardBase mk = ScolAddBuff_43.MakeCard(bmsg);
        msg.AddMk(MsgMagicUse.AddCardType.bullet, mk);
    }
}