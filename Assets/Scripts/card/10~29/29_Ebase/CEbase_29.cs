using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CEbase_29 : Clive_19
{
    public CardBase moveBrain;
    public CardBase magicBrain;
}
public class DenemyBrain : DataBase
{
    public int moveCon = 31;
    public int magicCon = 32;
}
public class SEbase_29 : Slive_19
{
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CEbase_29 card = _card as CEbase_29;
        card.team = Slive_19.Team.enemy;
        Dlive_19 config = DataManager.GetConfig<Dlive_19>(id);
        DenemyBrain brainConfig = DataManager.GetConfig<DenemyBrain>(id);
        if (brainConfig == null) brainConfig = new DenemyBrain();

        Cdiedes_27 die = CreateCard<Cdiedes_27>();die.liveDiePar = true;
        CardBase cgodie_28 = CreateCard<Cquickdie_34>();
        Cbrain_30 cbrain_30 = CreateCard<Cbrain_30>();
        CardBase bmove = CreateCard(brainConfig.moveCon);
        card.moveBrain = bmove;
        if(card.moveBrain is CEBmove_31 cbmove)
        {
            DenemyMoveBrain moveBrainConfig = DataManager.GetConfig<DenemyMoveBrain>(id);
            if(moveBrainConfig!=null)
            {
                cbmove.mbMinDis = moveBrainConfig.mbMinDis;
                cbmove.mbMaxDis = moveBrainConfig.mbMaxDis;
                cbmove.findLiveMaxDis = moveBrainConfig.findEnemyDis;
            }
        }
        CardBase bmagic = CreateCard(brainConfig.magicCon);
        card.magicBrain = bmagic;
        if (card.magicBrain is CEBmagic_32 cbmagive)
        {
            DenemyMoveBrain moveBrainConfig = DataManager.GetConfig<DenemyMoveBrain>(id);
            if (moveBrainConfig != null)
            {
                cbmagive.giveAtkDis = moveBrainConfig.atkDis;
                cbmagive.findLiveMaxDis = moveBrainConfig.findEnemyDis;
            }
        }

        AddComponent(cbrain_30, bmove, bmagic);

        AddComponent(cgodie_28, die);
        AddComponent(card, cgodie_28, cbrain_30);
    }
    public override void Init()
    {
        base.Init();
    }
    
}