using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーのベクトルをリセットする
public class VelocityReset : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Rigidbody rb;
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //当たったプレイヤーのベクトルを0にする
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector3.zero; 
        }
    }
}
