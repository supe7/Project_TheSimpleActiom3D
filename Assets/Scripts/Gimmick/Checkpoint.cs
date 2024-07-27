using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//復活地点を記録する
public class Checkpoint : MonoBehaviour
{
    static Checkpoint I;
    [SerializeField] GameObject player;
    [SerializeField] List<GameObject> checkPoints; //チェックポイントにするオブジェクトを登録
    [SerializeField] private int lifePoint = 3; //残機の数
    [SerializeField] private Vector3 vectorPoint; //復活地点
    [SerializeField] private float dead; //死ぬ高さ
    [SerializeField] private float offTime = 3f; //テキストを非表示にするまでの時間
    [SerializeField] private AudioClip checkSe; //チェックポイントに触れた時の効果音
    [SerializeField] private AudioClip deathSe; //死んでライフが減る時に鳴る効果音
    private bool deathFlag = false;//死んだかどうか
    private bool hitFlag = false; //敵に当たったか
    private bool checkFlag = false; //チェックポイントに触れたかどうか
    private Rigidbody rb;
    private AudioSource audioSource;
    private PlayerCamera playerCamera;
    void Start()
    {
        I = this;
        rb = player.GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        playerCamera = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
    }
    public static bool IsDeath()
    {
        return I.deathFlag;
    }
    public static bool IsCheck()
    {
        return I.checkFlag;
    }
    public static int Life()
    {
        return I.lifePoint;
    }
    //復活するときに起こる処理
    private void Revival()
    {
        rb.velocity = Vector3.zero; //ベクトルを0にする
        player.transform.position = vectorPoint; //プレイヤーを復活地点に戻す
        player.transform.rotation = Quaternion.identity; //プレイヤーを正面に向かす
        playerCamera.ResetCamera(); //カメラの向きを正面にする
        lifePoint--; //ライフを減らす
        audioSource.PlayOneShot(deathSe);
    }
    
    void Update()
    {
        if(deathFlag)
        {
            Revival();
            deathFlag = false;
        }
        if(player.transform.position.y < -dead || hitFlag)            
        {
            deathFlag = true;
            hitFlag = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //チェックポイントに触れたら
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            vectorPoint = player.transform.position; //復活地点を更新
            lifePoint++; //ライフを増やす
            checkFlag = true;
            audioSource.PlayOneShot(checkSe);
            if (checkFlag)
            {
                StartCoroutine(CheckTextOff());
            }
        }
        //敵に当たったら
        if (collision.gameObject.CompareTag("Enemy"))
        {
            hitFlag = true;
        }
    }

    IEnumerator CheckTextOff()
    {
        yield return new WaitForSeconds(offTime);
        checkFlag = false;
    }
}
