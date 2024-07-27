using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//オブジェクトの往復や回転
public class ForeverMove : MonoBehaviour
{
    [SerializeField] private Vector3 angle; //回転角度を指定
    [SerializeField] private Vector3 speed; //移動する速度
    [SerializeField] private float second = 1f;//往復するまでにかかる時間
    private float time = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        RoundTripMove();
        Rotation();
    }

    private void RoundTripMove()
    {
        //往復移動
        time += Time.fixedDeltaTime;
        float s = Mathf.Sin(time * Mathf.PI / second);
        Vector3 moveVelocity = speed * s * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + moveVelocity);
    }

    private void Rotation()
    {
        //回転
        Quaternion deltaRotation = Quaternion.Euler(angle * Time.fixedDeltaTime);
        rb.MoveRotation(transform.rotation * deltaRotation);
    }
}
