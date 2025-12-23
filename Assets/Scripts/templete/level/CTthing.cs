using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CTthing : CLTthingbase_1
{

}
public class DTthing : DataBase
{

}

public class STthing : SLTthingbase_1
{
    public override void ChatCallBack(CLTthingbase_1 _card, int nowChat, int choose, CardBase live)
    {
        CTthing card = _card as CTthing;
        DTthing config = basicConfig as DTthing;

        if (nowChat == 0)
        {
            switch (choose)
            {
                case 0:
                    {

                        break;
                    }
                case 1:
                    {

                        break;
                    }
            }
            EndChat(card);
        }
        else
        {
            EndChat(card);
        }
    }
}