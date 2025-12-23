using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMgun_30 : Cmagicbase_17
{
    public float damage;
    public float distance;
    public float scatterAngle;
}
public class DMgun_30 : DataBase
{
    public string gunName;
    public CMgun_30 gunData;

    public GameObject gunPrefab;
    public override void Init(int id)
    {
        gunPrefab = DataManager.LoadResource<GameObject>(id, gunName);
    }
}
public class SMgun_30 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CMgun_30 card = _card as CMgun_30;
        DMgun_30 config = basicConfig as DMgun_30;
        AddComponent(card, CreateCard<CGQcasf_28>());
        
        card.scatterAngle = config.gunData.scatterAngle;
        card.distance = config.gunData.distance;
        card.damage = config.gunData.damage;
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CMgun_30 card = _card as CMgun_30;
        MsgUpdate msg = _msg as MsgUpdate;

        
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMgun_30 card = _card as CMgun_30;
        DMgun_30 config = basicConfig as DMgun_30;

        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.dir = MyTool.RandScatter(bmsg.dir, card.scatterAngle);
        bmsg.damage = card.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        bmsg.time = card.distance / SBgun_17.gunBulletSpeed;
        Sbullet_10.GiveBullet(SBgun_17.bid, bmsg);
    }
}