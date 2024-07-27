using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//ゴールした時の処理
public class Goal : MonoBehaviour
{
    [SerializeField] private AudioClip se; //ゴール時の効果音
    [SerializeField] GameObject[] goalEffect; //ゴール時に出すエフェクト
    static bool goalFlag  = false;
    private AudioSource audioSource;
    void Start()
    {
        foreach(GameObject obj in goalEffect)
        {
            obj.SetActive(false);
        }
        goalFlag = false;
        audioSource = GetComponent<AudioSource>();
    }
    static public bool IsGoal()
    {
        return goalFlag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            goalFlag = true;
            audioSource.PlayOneShot(se);
            //ゴールしたらエフェクトをオンにする
            foreach (GameObject obj in goalEffect)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            goalFlag = false;
        }
    }
}
