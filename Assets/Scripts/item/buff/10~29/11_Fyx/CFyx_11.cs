using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFyx_11 : Cbuffbase_36
{
    public BuffBar buffBar;
}
public class DFyx_11 : DataBase
{
    public string buffBarName;

    public GameObject buffBar;
    public override void Init(int id)
    {
        buffBar = DataManager.LoadResource<GameObject>(id, buffBarName);
    }
}
public class SFyx_11 : Sbuffbase_36
{
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        AddComponent(_card, CreateCard<CFwfsf_12>());
        AddComponent(_card, CreateCard<CFwfyd_15>());
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFyx_11 card = _card as CFyx_11;
        MsgOnItem msg = _msg as MsgOnItem;
        DFyx_11 config = basicConfig as DFyx_11;

        SetBuffBar(card, msg, ref card.buffBar, config.buffBar);
    }
}