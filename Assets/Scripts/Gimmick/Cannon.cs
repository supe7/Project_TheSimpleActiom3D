using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//設定した弾を一定間隔で発射する
public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject missile; //発射する弾
    [SerializeField] private GameObject effect; //発射時の煙
    [SerializeField] private GameObject generator; //missileを生成する座標とするオブジェクト
    [SerializeField] private float coolTime = 3; //待機時間
    [SerializeField] private AudioClip se; //発射時の効果音
    private bool playerFlag = false; //プレイヤーがいるかどうか
    private bool shotFlag = true; //発射フラグ
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerStay(Collider other)
    {
        //プレイヤーがトリガーの範囲内にいれば発射する
        if (other.gameObject.CompareTag("Player"))
        {
            playerFlag = true;
            if((playerFlag && shotFlag) && !Checkpoint.IsDeath()) 
            {
                StartCoroutine(Fire());
            }
            else
            {
                playerFlag = false;
            }
        }
    }
    IEnumerator Fire()
    {
        shotFlag = false;
        if (shotFlag == false)
        {
            yield return new WaitForSeconds(coolTime);
            shotFlag = true;
        }
        //generatorの座標を代入
        Vector3 newPos = generator.transform.position;
        //プレハブからゲームオブジェクトを作って
        audioSource.PlayOneShot(se);
        Instantiate(missile, newPos, transform.rotation);
        Instantiate(effect, newPos, transform.rotation);
    }
}
