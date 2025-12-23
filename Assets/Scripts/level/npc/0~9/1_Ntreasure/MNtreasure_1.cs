using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MNtreasure_1 : MonoBehaviour
{
    public ParticleSystem myPar;
    public GameObject lid;
    public Color dieColor;
    public float duration = 0.5f;

    public void SetParColor(Color color)
    {
        var main = myPar.main;
        main.startColor = color;
    }
    public void Open()
    {
        lid.transform.DOLocalRotate(new Vector3(-60, 0), duration);
    }
    public void Close()
    {
        lid.transform.DOLocalRotate(new Vector3(0, 0), duration);
    }
}
