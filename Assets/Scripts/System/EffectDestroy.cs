using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//エフェクトを再生し終わったらこのオブジェクトを消す
public class EffectDestroy : MonoBehaviour
{
    new ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        //エフェクトが再生中かどうかを確認
        if (!particleSystem.isPlaying)
        {
            //再生し終わったらオブジェクトを消す
            Destroy(gameObject);
        }
    }
}
