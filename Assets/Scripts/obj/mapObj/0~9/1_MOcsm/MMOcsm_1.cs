using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MMOcsm_1 : MonoBehaviour
{
    public ParticleSystem mainPar, openPar;
    public TextMeshPro text;

    public void SetColor(Color color, string csmName = "")
    {
        var main = mainPar.main;main.startColor = color;
        var open = openPar.main;open.startColor = color;
        text.text = csmName;
        text.DOColor(MyRGB.ToFadeColor(color, 0.3f), 2);
    }
}
