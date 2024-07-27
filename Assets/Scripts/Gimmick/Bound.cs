using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//上に乗ったプレイヤーを跳ねさせる
public class Bound : MonoBehaviour
{
    [SerializeField] private Vector3 boundPower; //指定した方向にプレイヤーを飛ばす
    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip se; //効果音を指定
    private Rigidbody rb;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = player.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤーが当たったら
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(se);
            rb.AddForce(boundPower, ForceMode.Impulse);
        }
    }
}
