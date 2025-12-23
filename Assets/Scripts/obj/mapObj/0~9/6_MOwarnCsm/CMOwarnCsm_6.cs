using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SfloorBase_2;
using static SLFfightBase_6;
using static SystemManager;

public class CMOwarnCsm_6 : CObj_2
{
    public Clive_19 liveToSummon;

    public float warnTime = 0;
}
public class DMOwarnCsm_6 : DataBase
{

}
public class SMOwarnCsm_6 : SObj_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.Update, Update);
    }
    public void Update(CardBase _card, MsgBase _msg)
    {
        CMOwarnCsm_6 card = _card as CMOwarnCsm_6;
        MsgUpdate msg = _msg as MsgUpdate;

        card.warnTime -= msg.time;
        if(card.warnTime < 0)
        {
            card.warnTime = MyMath.BigFloat;
            if (card.liveToSummon != null)
            {
                AddToWorld(card.liveToSummon);
                card.liveToSummon.obj.transform.SetPositionAndRotation(card.obj.transform.position, card.obj.transform.rotation);

                SendMsgToPlayer(mTSummonEnemy, new MsgSummonEnemy(card.liveToSummon));
                card.liveToSummon = null;
            }
        }
    }
    public void OnItem(CardBase _card, MsgBase _msg)
    {
        CMOwarnCsm_6 card = _card as CMOwarnCsm_6;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op==1)
        {
            MMOwarnCsm_6_csm csm = card.obj.gameObject.GetComponent<MMOwarnCsm_6_csm>();
            csm.emitDuration = card.warnTime;
            csm.totalTime = csm.emitDuration + csm.endLifeTime + 1f;
            csm.loop = false;
        }
        else
        {

        }
    }
}