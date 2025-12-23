using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static CardManager;
using static SGkndsj_44;
using static SystemManager;

public class CLTkndsj_4 : CLTthingbase_1
{
    public int[] conditionChoice = new int[3];
    public int[] attChoice = new int[3];

    public int att;
    public int condition;

    public string cText0 { get { return SGkndsj_44.idToDes[conditionChoice[0]].describe; } }
    public string cText1 { get { return SGkndsj_44.idToDes[conditionChoice[1]].describe; } }
    public string cText2 { get { return SGkndsj_44.idToDes[conditionChoice[2]].describe; } }
    public string aText0 { get { return BasicAttID.ColoredAttText(attChoice[0]); } }
    public string aText1 { get { return BasicAttID.ColoredAttText(attChoice[1]); } }
    public string aText2 { get { return BasicAttID.ColoredAttText(attChoice[2]); } }
}
public class DLTkndsj_4 : DataBase
{

}

public class SLTkndsj_4 : SLTthingbase_1
{
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CLTkndsj_4 card = _card as CLTkndsj_4;

        List<int> cons = MyRandom.NoRepRandom(3, ConditionAndValue.conditionIDCnt);
        List<int> atts = MyRandom.NoRepRandomInList(attList, 3);
        for(int i=0;i<3;i++)
        {
            card.conditionChoice[i] = cons[i];
            card.attChoice[i] = atts[i];
        }
        
    }
    public override void ChatCallBack(CLTthingbase_1 _card, int nowChat, int choose, CardBase live)
    {
        CLTkndsj_4 card = _card as CLTkndsj_4;
        DLTkndsj_4 config = basicConfig as DLTkndsj_4;

        if (nowChat == 0)
        {
            card.condition = card.conditionChoice[choose];
            ToChat(card, 1);
        }
        else if(nowChat==1)
        {
            card.att = card.attChoice[choose];

            CGkndsj_44 item = CreateCard<CGkndsj_44>();
            item.attID = card.att;
            item.conditionID = card.condition;
            SMOitemObj_4.GiveItemObj(item, card.obj.transform.position + Vector3.up, card.obj.transform.rotation);

            EndChat(card);
        }
        else
        {
            EndChat(card);
        }
    }
}