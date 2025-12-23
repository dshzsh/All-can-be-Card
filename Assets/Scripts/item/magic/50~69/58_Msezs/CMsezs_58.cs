using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMsezs_58 : Cmagicbase_17
{

}
public class DMsezs_58 : DataBase
{
    public float bdPow;
}
public class SMsezs_58 : Smagicbase_17
{
    private List<int> onlyBdPool = new();
    public override void Init()
    {
        base.Init();
        AddHandle(Sysw_26.mTSummonYsw, SummonYsw);
        AddHandle(Sbag_40.mTMyFirstGetItem, MyFirstGetItem);

        // 制造符合条件的道具池
        Ditem myDitem = DataManager.GetConfig<Ditem>(id);
        var pool = MyTag.GetPoolWithTags(MyTag.CardTag.magic);
        pool.ExceptWith(Ditem.CantGetPool);
        List<int> list = new List<int>(pool);
        foreach(int i in list)
        {
            Ditem ditem = DataManager.GetConfig<Ditem>(i);
            if (ditem == null) continue;
            if (ditem.rare >= myDitem.rare) continue;
            if (DataManager.GetDescirbe(i).Contains("[主动]")) continue;
            onlyBdPool.Add(i);
        }
    }
    void MyFirstGetItem(CardBase _card, MsgBase _msg)
    {
        CMsezs_58 card = _card as CMsezs_58;
        MsgGetItem msg = _msg as MsgGetItem;
        DMsezs_58 config = basicConfig as DMsezs_58;

        Citem_33 magic = CreateCard(MyRandom.RandomInList(onlyBdPool)) as Citem_33;
        magic.pow = card.pow;
        Sbag_40.LiveGetItem(msg.container, magic);
    }
    void SummonYsw(CardBase _card, MsgBase _msg)
    {
        CMsezs_58 card = _card as CMsezs_58;
        Sysw_26.MsgSummonYsw msg = _msg as Sysw_26.MsgSummonYsw;
        DMsezs_58 config = basicConfig as DMsezs_58;

        if (!TryGetClive(card, out var clive)) return;
        if (clive.myMagic == null) return;

        foreach(CardBase magic in Smagic_14.GetAllMagics(clive.myMagic))
        {
            CardBase buff = NewCopy(magic);
            if (buff is Citem_33 citem)
                citem.pow *= config.bdPow * card.pow;
            msg.AddCard(buff);
        }
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMsezs_58 card = _card as CMsezs_58;
        DMsezs_58 config = basicConfig as DMsezs_58;

        
    }
}