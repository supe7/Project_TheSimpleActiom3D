using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//プレイヤーを追跡する弾
public class Horming : MonoBehaviour
{
    [SerializeField] private float speed = 1; //移動速度
    [SerializeField] private float dethTime = 5; //消えるまでの時間
    private Vector3 v = Vector3.zero; //一旦ベクトルを0にする
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * (speed / 2) * Time.deltaTime, ForceMode.Impulse);
    }

    void LateUpdate()
    {
        HormingMissile();
        //ゴールするか死んだりしたらこのオブジェクトを消す
        if (Goal.IsGoal()|| Checkpoint.IsDeath())
        {
            Destroy(gameObject);
        }
    }

    private void HormingMissile()
    {
        /*
        プレイヤーの座標ーこのオブジェクトの座標＝方向
        方向保ったまま長さを正規化して1にする
        常にブレーキをかけながらプレイヤーに向かう速度を加算
        設定した時間になると消える
         */
        var D = Player.GetPos() - transform.position;
        var d = D.normalized;
        v = speed * Time.deltaTime * d;
        transform.forward = rb.velocity;
        rb.AddForce(v - rb.velocity * 0.1f, ForceMode.Force);
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(dethTime);
        Destroy(gameObject);
    }
}
