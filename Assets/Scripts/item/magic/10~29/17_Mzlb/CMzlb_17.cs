using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using static SMzlb_17;

public class CMzlb_17 : Cmagicbase_17
{
    public List<ConObj> conObjs = new();
}
public class DMzlb_17 : DataBase
{
    public float damage;
    public float time;
    public float exDamage;
    public float force;
    public float throwForce;
}
public class SMzlb_17 : Smagicbase_17
{
    public class ConObj
    {
        public Rigidbody rbody;
        public CObj_2 cobj;
        public float time = 0f;
        public GameObject link;
        public bool toPlayer = true;
    }
    public int bid;
    public int bidThrow;
    public override void Init()
    {
        base.Init();

        AddHandle(MsgType.FixedUpdate, Update);
        bid = GetTypeId(typeof(CBzlb_10));
        bidThrow = GetTypeId(typeof(CBzlbThrow_11));
    }
    public static void AddLink(CMzlb_17 magic, ConObj conObj)
    {
        GameObject linkPrefab = DataManager.GetConfig<DObj_2>(magic.id).gameObject;
        conObj.link = GameObject.Instantiate(linkPrefab, conObj.rbody.transform.position, Quaternion.identity);
        magic.conObjs.Add(conObj);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CMzlb_17 card = _card as CMzlb_17;
        MsgUpdate msg = _msg as MsgUpdate;
        DMzlb_17 config = basicConfig as DMzlb_17;

        if (!TryGetCobj(card, out var cobj)) return;

        //将所有被抓的敌人聚集在上方
        Vector3 targetPos = cobj.obj.Center + Vector3.up * (cobj.height + 0.5f);

        for (int i=0;i<card.conObjs.Count;i++)
        {
            ConObj conObj = card.conObjs[i];
            card.conObjs[i].time += msg.time;
            if (card.conObjs[i].rbody == null || card.conObjs[i].time >= config.time) 
            {
                if (card.conObjs[i].link!=null)
                {
                    GameObject.Destroy(card.conObjs[i].link);
                }
                card.conObjs.RemoveAt(i);
                i--;
                continue;
            }
            Rigidbody rbody = card.conObjs[i].rbody;
            if (Vector3.Distance(rbody.transform.position, targetPos) > 0.2f)
                MyPhysics.GiveForce(rbody, (targetPos - rbody.position).normalized, config.force);
            //有一个link在玩家和敌人间循环移动
            if(conObj.link != null)
            {
                Vector3 rpos = rbody.position;
                if (card.conObjs[i].cobj != null) rpos = card.conObjs[i].cobj.obj.Center;
                if (conObj.toPlayer) rpos = cobj.obj.Center;

                conObj.link.transform.position = Vector3.MoveTowards(conObj.link.transform.position, rpos, 100 * msg.time);
                if (Vector3.Distance(conObj.link.transform.position, rpos) <= MyTool.SmallFloat)
                {
                    conObj.toPlayer = !conObj.toPlayer;
                }
            }
        }
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMzlb_17 card = _card as CMzlb_17;
        DMzlb_17 config = basicConfig as DMzlb_17;

        //尝试丢人
        bool ok = false;
        for(int i=0;i<card.conObjs.Count;i++)
        {
            Rigidbody rbody = card.conObjs[i].rbody;
            if (rbody != null)
            {
                Vector3 rpos = rbody.position;
                if (card.conObjs[i].cobj != null) rpos = card.conObjs[i].cobj.obj.Center;
                Vector3 dir = (msg.pos - rpos).normalized;
                rbody.velocity = Vector3.zero;
                MyPhysics.GiveImpulseForce(rbody, dir, config.throwForce);
                MsgBullet bmsg2 = new MsgBullet(msg);
                bmsg2.damage = config.exDamage * Shealth_4.GetAttf(card, BasicAttID.atk);
                bmsg2.initPos = rpos;
                bmsg2.dir = dir;
                bmsg2.exScale = MyTool.Vector3Multiply(bmsg2.exScale, rbody.transform.localScale);
                CGQCsetParent_50 buff = CreateCard<CGQCsetParent_50>(); buff.transform = rbody.transform;
                bmsg2.AddCard(buff);
                Sbullet_10.GiveBullet(bidThrow, bmsg2);

                if (card.conObjs[i].link != null)
                {
                    GameObject.Destroy(card.conObjs[i].link);
                }
                card.conObjs.RemoveAt(i);
                i--;
                ok = true;
            }
        }
        if (ok) return;
        //没有人丢，发射抓取
        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        Sbullet_10.GiveBullet(bid, bmsg);
    }
}