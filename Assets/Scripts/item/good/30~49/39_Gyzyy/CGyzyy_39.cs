using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGyzyy_39 : CGattItemBase_40
{
    public int cen = 0;
    public float time;
}
public class DGyzyy_39 : DataBase
{
    public DbasicAtt.AttAndValueData atkAddData;
    public DbasicAtt.AttAndValueData defAddData;

    public AttAndRevise atkAdd, defAdd;
    public override void Init(int id)
    {
        base.Init(id);
        atkAdd = atkAddData.ToRevise();
        defAdd = defAddData.ToRevise();
    }
}
public class SGyzyy_39 : SGattItemBase_40
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
        AddHandle(SLFfightBase_6.mTFightEnd, FightEnd);
    }
    void FightEnd(CardBase _card, MsgBase _msg)
    {
        CGyzyy_39 card = _card as CGyzyy_39;

        card.cen = 0;
        ChangeAtt(card);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CGyzyy_39 card = _card as CGyzyy_39;
        MsgUpdate msg = _msg as MsgUpdate;
        DGyzyy_39 config = basicConfig as DGyzyy_39;

        if (!SLFfightBase_6.InFight()) return;

        if(MyTool.IntervalTime(1, ref card.time, msg.time))
        {
            card.cen += 1;

            ChangeAtt(card, config.atkAdd.WithPow(card.cen * card.pow), config.defAdd.WithPow(card.cen * card.pow));
        }
    }
}