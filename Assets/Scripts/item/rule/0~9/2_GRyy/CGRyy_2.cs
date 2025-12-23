using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGRyy_2 : CGRbase_1
{
    public CGRcountedItem_3 yin, yang;

    public GameObject leftUpUI;
}
public class DGRyy_2 : DataBase
{
    public string leftUpUI;

    public GameObject leftUpUIPrefab;
    public override void Init(int id)
    {
        leftUpUIPrefab = DataManager.LoadResource<GameObject>(id, leftUpUI);
    }
}
public class SGRyy_2 : SGRbase_1
{
    public static int yinID, yangID;
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGRyy_2 card = _card as CGRyy_2;

        card.yin = CreateCard<CGRcountedItem_3>(); card.yin.itemId = yinID;
        card.yang = CreateCard<CGRcountedItem_3>(); card.yang.itemId = yangID;
        ActiveComponent(card, card.yin);
        ActiveComponent(card, card.yang);
    }

    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnShowUI, OnShowUI);
        yinID = id; yangID = id + 1;
    }
    
    void OnShowUI(CardBase _card, MsgBase _msg)
    {
        CGRyy_2 card = _card as CGRyy_2;
        MsgOnShowUI msg = _msg as MsgOnShowUI;
        DGRyy_2 config = basicConfig as DGRyy_2;

        if (msg.op == 1)
        {
            card.leftUpUI = UIBasic.GiveLeftUpUI(config.leftUpUIPrefab);
            card.leftUpUI.GetComponent<MGRyy_2_UI>().Set(card);
        }
        else
        {
            if (card.leftUpUI != null)
            {
                GameObject.Destroy(card.leftUpUI);
            }
        }
    }
}