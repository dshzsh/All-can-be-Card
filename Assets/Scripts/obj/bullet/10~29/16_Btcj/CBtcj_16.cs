using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBtcj_16 : Cbullet_10
{
    public CcolAddBuff_43 ccolAdd;
}
public class DBtcj_16 : DataBase
{
    public string dieObjName;

    public GameObject dieObj;
    public override void Init(int id)
    {
        dieObj = DataManager.LoadResource<GameObject>(id, dieObjName);
    }
}
public class SBtcj_16 : Sbullet_10
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.TrueDie, TrueDie);
    }
    public void TrueDie(CardBase _card, MsgBase _msg)
    {
        CBtcj_16 card = _card as CBtcj_16;
        DBtcj_16 config = basicConfig as DBtcj_16;

        GameObject.Instantiate(config.dieObj, card.obj.transform.position, card.obj.transform.rotation)
            .transform.localScale = card.obj.transform.localScale;
    }
}