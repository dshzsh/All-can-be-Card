using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CBbcj_12 : Cbullet_10
{

}
public class DBbcj_12 : DataBase
{
    public string dieObjName;

    public GameObject dieObj;
    public override void Init(int id)
    {
        dieObj = DataManager.LoadResource<GameObject>(id, dieObjName);
    }
}

public class SBbcj_12 : Sbullet_10
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.TrueDie, TrueDie);
    }
    public void TrueDie(CardBase _card, MsgBase _msg)
    {
        CBbcj_12 card = _card as CBbcj_12;
        DBbcj_12 config = basicConfig as DBbcj_12;

        GameObject.Instantiate(config.dieObj, card.obj.transform.position, card.obj.transform.rotation)
            .transform.localScale = card.obj.transform.localScale;
    }
}