using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//一定周期で上下に動く
public class SinMove : MonoBehaviour
{
    [SerializeField] private float roundTripSpeed = 0.5f; //往復速度
    [SerializeField] private float rotateSpeed = 90f; //回転速度
    private Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
    }
    private void FixedUpdate()
    {
        //一定速度で往復し、回転させる
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time) * roundTripSpeed;
        transform.Rotate(transform.up * rotateSpeed * Time.fixedDeltaTime);
    }
}
