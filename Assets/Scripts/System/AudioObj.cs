using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//オブジェクトが消えるときに音を鳴らすようにする
public class AudioObj : MonoBehaviour
{
    private AudioClip se;
    void Start()
    {
        se = GetComponent<AudioSource>().clip;
        Destroy(gameObject, se.length);
    }
}
