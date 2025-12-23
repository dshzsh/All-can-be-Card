using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class CFcold_6 : Cbuffbase_36
{
    [JsonIgnore]
    public BarValue coldBar = new();
    [JsonIgnore]
    public BasicBar coldBarObj;
}
public class DFcold_6 : DataBase
{
    public float speedDown;
    public float coldMax;
    public string coldBarName;

    public GameObject coldBarObj;
    public override void Init(int id)
    {
        base.Init(id);
        coldBarObj = DataManager.LoadResource<GameObject>(id, coldBarName);
    }
}
public class SFcold_6 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFcold_6 card = _card as CFcold_6;
        MsgOnItem msg = _msg as MsgOnItem;
        DFcold_6 config = basicConfig as DFcold_6;

        if (TryGetCobj(card, out var cobj))
        {
            //todo: cobj.bspeed.DirectMul -= msg.op * card.pow * config.speedDown;
            card.coldBar.cur = card.pow;
            card.coldBar.max = config.coldMax;

            if(msg.op==1)
            {
                if (card.coldBarObj == null)
                {
                    card.coldBarObj = UIBasic.GiveLiveUpUI(config.coldBarObj, cobj).GetComponent<BasicBar>();
                }
                else if (card.coldBarObj.valid == false)
                {
                    card.coldBarObj.valid = true;
                }
                
                card.coldBarObj.Set(card.coldBar);
            }
            else
            {
                if (card.coldBarObj != null)
                {
                    card.coldBarObj.valid = false;
                }
            }
            
        }
    }
}