using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//上に乗ったプレイヤーを加速させる
public class SpeedUp : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 speed; //指定した方向に加速度を加える
    private Rigidbody rb;

    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision) //プレイヤーが乗っていたら
    {
        //速度を加え続ける
        rb.AddForce(speed, ForceMode.Force);
    }
}
