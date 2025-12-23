using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;
using DG.Tweening;

public class CMzc_15 : Cmagicbase_17
{
}
public class DMzc_15 : DataBase
{
    public float damage;
    public float rangeUp;
    public float damageUp;
    public float stunTime;
    public float stunTimeUp;

    public string hammerName;
    [JsonIgnore]
    public GameObject hammer;
    public override void Init(int id)
    {
        hammer = DataManager.LoadResource<GameObject>(id, hammerName);
    }
}
public class SMzc_15 : Smagicbase_17
{
    public static int bid;
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGxlmk_12 charge = CreateCard<CGxlmk_12>();
        CardBase cnull = new CNull_0();
        DMcharge config = DataManager.GetConfig<DMcharge>(id);
        charge.chargeAnim = config.chargeAnim;
        charge.charge.max = config.chargeTime;
        AddComponent(_card, cnull);
        AddComponent(cnull, charge);
    }

    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyMagicBegin, MagicBegin);
        bid = GetTypeId(typeof(CBzc_9));
    }
    
    void MagicBegin(CardBase _card, MsgBase _msg)
    {
        CMzc_15 card = _card as CMzc_15;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DMzc_15 config = basicConfig as DMzc_15;

        if (msg.magic != card) return;

        if (!TryGetCobj(card, out var cobj) || cobj.obj.animator == null) return;

        float chargeTime = 0f;
        if (msg.TryGetTag<CchargeTime_44>(out var cc))
            chargeTime = cc.time;
        
        float windUp = msg.mdata.windUp / msg.instantAtkSpeed;

        GameObject obj = GameObject.Instantiate(config.hammer, cobj.obj.transform);
        obj.transform.position += cobj.centerOffset;
        obj.transform.forward = MyMath.V3zeroYNor(msg.pos - cobj.obj.Center);
        obj.transform.localScale *= chargeTime * config.rangeUp + 1;

        obj.transform.DORotate(new Vector3(90f, 0f, 0f), windUp, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InQuad);

        GameObject.Destroy(obj, windUp + 0.01f);
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMzc_15 card = _card as CMzc_15;
        DMzc_15 config = basicConfig as DMzc_15;

        float chargeTime = 0f;
        if (msg.TryGetTag<CchargeTime_44>(out var cc))
            chargeTime = cc.time;
        float rangeUp = config.rangeUp * chargeTime + 1;

        MsgBullet bmsg = new MsgBullet(msg);
        // 碰撞附加眩晕
        ScolAddBuff_43.AddBuff<CFyx_11>(bmsg, config.stunTime * (1 + config.stunTimeUp * chargeTime), Sbuff_35.BeBuffMode.coverByBig);
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk) * (1 + chargeTime * config.damageUp);
        bmsg.exScale *= rangeUp;
        if (TryGetCobj(msg.live, out var obj))
        {
            bmsg.initPos = obj.obj.transform.position + bmsg.dir * rangeUp * 2;
        }
        Sbullet_10.GiveBullet(bid, bmsg);
    }
}