using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;
using static UnityEditor.Progress;

public class Ctimedes_7 : CardBase
{
    public float timeRes = 0f;
    public float timeMax = 0f;

    public void SetTime(float timeRes)
    {
        this.timeRes = timeRes;
        this.timeMax = timeRes;
    }
}

public class Stimedes_7 : SystemBase
{
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        Ctimedes_7 card = _card as Ctimedes_7;

        if (card.timeRes > card.timeMax)
            card.timeMax = card.timeRes;
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        Ctimedes_7 card = _card as Ctimedes_7;
        MsgUpdate msg = _msg as MsgUpdate;

        if (card.timeRes > card.timeMax)
            card.timeMax = card.timeRes;

        card.timeRes -= msg.time;
        if(card.timeRes < 0f)
        {
            SendMsg(card.parent, MsgType.TrueDie, new MsgDie());
        }
    }
}