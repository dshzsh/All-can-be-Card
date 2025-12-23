using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Cplayer_8 : Clive_19
{
    
}

public class Splayer_8 : Slive_19
{
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        Cplayer_8 card = _card as Cplayer_8;

        CardBase camera = CreateCard<CShoulderCamera_37>();//camera.depth = 0;
        //Cjump_11 jump = CreateCard<Cjump_11>();jump.force = 5f;

        Cbrain_30 brain = CreateCard<Cbrain_30>();

        CControl_3 control = CreateCard<CControl_3>();
        Cmagiccon_15 cmc= CreateCard<Cmagiccon_15>();
        CardBase cshow = CreateCard<Cshowui_42>();

        AddComponent(brain, control, cmc, camera, cshow);
        AddComponent(card, brain);

        CardBase bag = CreateCard<Cbag_40>();
        AddComponent(card, bag);
    }

}