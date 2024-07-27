using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//チェックポイントの色を変え、当たり判定を消す処理
public class CollarChange : MonoBehaviour
{
    [SerializeField] private Material mat = null; //マテリアルを指定
    private Collider col;
    void Start()
    {
        col = GetComponent<Collider>();
        mat = GetComponent<MeshRenderer>().sharedMaterial;
        mat.color = new Color(1.0f, 0.92f, 0.016f, 1.0f); //黄色に設定する
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            mat.color = new Color(1.0f, 0.0f, 0.0f, 0.7f); //半透明な赤色になる
            col.enabled = false; //コライダーをオフにする
        }
    }
}
