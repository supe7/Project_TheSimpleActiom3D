using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//ゲーム全体の処理を管理する
public class Gamemanager : MonoBehaviour
{
    static Gamemanager I;
    [SerializeField] Checkpoint checkpoint;
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] GameObject player;//プレイヤー
    [SerializeField] GameObject panel; //パネル
    [SerializeField] Text goalText; //ゴールのテキスト
    [SerializeField] Text timerText;//タイマーのテキスト
    [SerializeField] Text lifeText; //残機のテキスト
    [SerializeField] Text checkText; //チェックポイント接触時のテキスト
    [SerializeField] Image keyboardImage; //キーボード操作の画像
    [SerializeField] Image controllerImage; //コントローラー操作の画像
    [SerializeField] private int timeLimit; //タイムリミットを設定
    [SerializeField] private string sceneName; //切り換え先のシーン名
    [SerializeField] private float changeTime = 3; //シーンの切り替え時間
    [SerializeField] private Vector3 startPosition = new Vector3(0, 1, 0);//プレイヤーの初期位置
    private float time;//経過時間
    private int remainingTime;//整数化した現在の時間
    
    void Awake()
    {
        Application.targetFrameRate = 80; //フレーム数を固定
        QualitySettings.vSyncCount = 0; //ビルド時に品質を下げないようにする
    }
    void Start()
    {
        I = this;
        checkpoint.enabled = true;
        playerCamera.enabled = true;
        goalText.enabled = false;
        checkText.enabled = false;
        player.transform.position = startPosition; //プレイヤーを指定した初期位置に
        panel.SetActive(false);
    }
    void Update()
    {
        Timer();
        lifeText.text = "LIFE:" + Checkpoint.Life();
        if (Goal.IsGoal())
        {
            GameClear();
        }
        //ライフが0以下になるか制限時間が0以下になれば
        if (Checkpoint.Life() <= 0 || remainingTime <= 0)
        {
            GameOver();
        }
        if(Checkpoint.IsCheck())
        {
            checkText.enabled = true;
        }
        else
        {
            checkText.enabled = false;
        }
    }

    //制限時間を引いていき表示する
    private void Timer()
    {
        time -= Time.deltaTime;
        int remaining = timeLimit + (int)time;
        timerText.text = "TIME:" + remaining.ToString("D4");
        remainingTime = remaining;
    }

    public static int ReturnTime()
    {
        return I.remainingTime;
    }

    //ゲームオーバー時の処理
    private void GameOver()
    {
        panel.SetActive(true);
        player.SetActive(false);
        timerText.enabled = false;
        lifeText.enabled = false;
        keyboardImage.enabled = false;
        controllerImage.enabled = false;
        playerCamera.enabled = false;
    }
    //ゲームクリア時の処理
    private void GameClear()
    {
        checkpoint.enabled = false;
        goalText.enabled = true;
        timerText.enabled = false;
        lifeText.enabled = false;
        keyboardImage.enabled = false;
        controllerImage.enabled = false;
        time = 0;
        StartCoroutine(ChangeScene());
    }
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(changeTime);
        SceneManager.LoadScene(sceneName);
    }
}
