using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFcm_14 : Cbuffbase_36
{
    public BuffBar buffBar;
}
public class DFcm_14 : DataBase
{
    public string buffBarName;

    public GameObject buffBar;
    public override void Init(int id)
    {
        base.Init(id);
        buffBar = DataManager.LoadResource<GameObject>(id, buffBarName);
    }
}
public class SFcm_14 : Sbuffbase_36
{
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        AddComponent(_card, CreateCard<CFwfsf_12>());
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFcm_14 card = _card as CFcm_14;
        MsgOnItem msg = _msg as MsgOnItem;
        DFcm_14 config = basicConfig as DFcm_14;

        SetBuffBar(card, msg, ref card.buffBar, config.buffBar);
    }
}