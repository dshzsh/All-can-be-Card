using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemObj : MonoBehaviour
{
    public TextMeshPro textF;
    public TextMeshPro textB;
    public MeshRenderer mr;
    public ParticleSystem par;

    public void SetCard(CardBase good)
    {
        textF.text = textB.text = DataManager.GetName(good.id);
        mr.material.color = CardManager.GetCardColor(good);
        var pmain = par.main;
        Color color = Sitem_33.RareColor.GetRareColor(good);
        pmain.startColor = color;
    }
}
