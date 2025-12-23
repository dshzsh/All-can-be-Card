using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;
using static UnityEditor.Progress;

public class CMqjtp_49 : Cmagicbase_17
{


}
public class DMqjtp_49 : DataBase
{
    public List<DbasicAtt.AttAndReviseData> attDatas;

    public List<AttAndRevise> atts;
    public override void Init(int id)
    {
        atts = new List<AttAndRevise>();
        foreach (var att in attDatas) 
            atts.Add(att.ToRevise());
    }
}
public class SMqjtp_49 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CMqjtp_49 card = _card as CMqjtp_49;
        MsgOnItem msg = _msg as MsgOnItem;
        DMqjtp_49 config = basicConfig as DMqjtp_49;

        foreach(var att in config.atts)
        {
            att.WithPow(card.pow).UseOnLive(card, msg.op);
        }
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMqjtp_49 card = _card as CMqjtp_49;
        DMqjtp_49 config = basicConfig as DMqjtp_49;

        
    }
}