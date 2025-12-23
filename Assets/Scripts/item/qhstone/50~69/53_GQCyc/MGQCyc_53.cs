using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGQCyc_53 : MyParticleSystemControl
{
    public int outerCount = 20;
    public int innerCount = 12;

    public void Set(float time, Color color)
    {
        emitDuration = time;
        totalTime = emitDuration + endLifeTime + 1f;
        var psmain = ps.main;
        psmain.startColor = color;
    }
    public override void Emmit()
    {
        StartCoroutine(EmitCircleSequential(outerCount, 1));
        StartCoroutine(EmitCircleSequential(innerCount, 0.5f));
    }
}
