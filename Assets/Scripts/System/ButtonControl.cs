using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
//ボタン関連の処理
public class ButtonControl : MonoBehaviour
{
    [SerializeField] private GameObject activeObj = null; //表示したいオブジェクト
    [SerializeField] private GameObject negativeObj = null; //非表示にしたいオブジェクト
    [SerializeField] private GameObject button; //イベントシステムで操作するボタン
    [SerializeField] private GameObject audioObj; //音を鳴らす用のオブジェクト
    [SerializeField] private string changeSceneName = null; //切り替え先のシーン名
    [SerializeField] private float changeTime = 0.5f; //切り替えまでの時間
    [SerializeField] private AudioClip se;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnObj()
    {
        //押したらオブジェクトを表示する
        audioSource.PlayOneShot(se);
        activeObj.SetActive(true);
        Instantiate(audioObj);
    }

    public void OffObj()
    {
        //押したらオブジェクトを非表示にする
        EventSystem.current.SetSelectedGameObject(button);
        audioSource.PlayOneShot(se);
        negativeObj.SetActive(false);
        Instantiate(audioObj);
    }

    public void ChangeScene()
    {
        //シーンの切り替え
        audioSource.PlayOneShot(se);
        StartCoroutine(Change());
    }
    IEnumerator Change()
    {
        yield return new WaitForSecondsRealtime(changeTime);
        SceneManager.LoadScene(changeSceneName);
    }

    public void QuitGame()
    {
        //ゲームをやめる
        Application.Quit();
    }
}
