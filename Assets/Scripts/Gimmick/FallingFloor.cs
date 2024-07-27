using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//上にプレイヤーが乗ると落下する床
public class FallingFloor : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private float height; //元に戻る高度
    [SerializeField] private float startTime = 3f; //落下するまでの時間
    private Rigidbody rb;
    private Collider col;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        if(transform.position.y <= height)
        {
            //最初の状態に戻る
            rb.isKinematic = true;
            rb.useGravity = false;
            transform.position = startPos;
            col.enabled = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Falling());
        }
    }

    IEnumerator Falling()
    {
        //コライダーとkinematicをfalseにして落下する
        yield return new WaitForSeconds(startTime);
        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = false;
    }
}
