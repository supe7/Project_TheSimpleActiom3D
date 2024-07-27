using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//大砲から出る弾
public class Missile : MonoBehaviour
{
    [SerializeField] private float speed = 1; //移動速度
    [SerializeField] private float dethTime = 5; //消えるまでの時間
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
        //ゴールするか死んだりしたらこのオブジェクトを消す
        if (Goal.IsGoal() || Checkpoint.IsDeath())
        {
            Destroy(gameObject);
        }
        StartCoroutine(Explosion());
    }
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(dethTime);
        Destroy(gameObject);
    }
    
}
