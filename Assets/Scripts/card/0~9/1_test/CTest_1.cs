using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class CTest_1 : CardBase
{
    public float current;
}

public class STest_1 : SystemBase
{
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        AddComponent(_card, CreateCard<CMnulll_0>());
    }
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CTest_1 card = _card as CTest_1;
        MsgUpdate msg = _msg as MsgUpdate;

        if(Input.GetKeyDown(KeyCode.P)) { Time.timeScale = 1 - Time.timeScale; }

        if (!TryGetCobj(card, out var obj)) return;
        
        if(Input.GetKeyDown(KeyCode.T))
        {
            CardBase mainMagic = CreateCard<CMshot_1>();
            CardBase wfxx = CreateCard<CGQwfxx_15>();
            AddComponent(mainMagic, wfxx);
            Sbag_40.LiveGetItem(obj, mainMagic);
            CardBase qhs = CreateCard<CGwlqh_3>();
            CardBase wfxx2 = CreateCard<CGQwfxx_15>();
            AddComponent(qhs, wfxx2);
            Sbag_40.LiveGetItem(obj, qhs);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetAllItem(obj);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Time.timeScale /= 2;
        }
    }
    private void GetAllItem(CObj_2 live)
    {
        {
            CGwjhz_17 wjhz = CreateCard<CGwjhz_17>();
            wjhz.tag = MyTag.CardTag.good;
            wjhz.content = "道具";
            Sbag_40.LiveGetItem(live, wjhz);
        }
        {
            CGwjhz_17 wjhz = CreateCard<CGwjhz_17>();
            wjhz.tag = MyTag.CardTag.magic;
            wjhz.content = "魔法";
            Sbag_40.LiveGetItem(live, wjhz);
        }
        {
            CGwjhz_17 wjhz = CreateCard<CGwjhz_17>();
            wjhz.tag = MyTag.CardTag.qhstone;
            wjhz.content = "强化石";
            Sbag_40.LiveGetItem(live, wjhz);
        }
        {
            CGwjhz_17 wjhz = CreateCard<CGwjhz_17>();
            wjhz.tag = MyTag.CardTag.build;
            wjhz.content = "流派";
            Sbag_40.LiveGetItem(live, wjhz);
        }
        Sbag_40.LiveGetItem(live, CreateCard<CGcjsfq_36>());
        Sbag_40.LiveGetItem(live, CreateCard<CMxljr_36>());
    }
    private void ParseMagic(CardBase live)
    {
        CardBase mainMagic = CreateCard<CMshot_1>();
        CardBase mainQhc = Sqhc_38.QhcWithSlot(1);
        AddComponent(mainMagic, mainQhc);

        CardBase ccMagic = CreateCard<CMcc_10>();
        CardBase ccQhc = Sqhc_38.QhcWithSlot(1);
        AddComponent(ccMagic, ccQhc);

        CGcfqh_2 cfqh = CreateCard<CGcfqh_2>();
        SendMsg(cfqh, MsgType.SelfContainerGet, new MsgGetItem(ccMagic, 1, 0));
        SendMsg(mainQhc, MsgType.SelfContainerGet, new MsgGetItem(cfqh, 1, 0));

        CGcfqh_2 cfqh2 = CreateCard<CGcfqh_2>();
        SendMsg(cfqh2, MsgType.SelfContainerGet, new MsgGetItem(CreateCard<CMshot_1>(), 1, 0));
        SendMsg(ccQhc, MsgType.SelfContainerGet, new MsgGetItem(cfqh2, 1, 0));

        Sbag_40.LiveGetItem(live, mainMagic);
    }
    
}