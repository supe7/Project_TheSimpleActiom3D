using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
//プレイヤーの移動などに関する処理
public class Player : MonoBehaviour
{
    static Player I;
    [SerializeField] private float speed = 475f; //移動速度
    [SerializeField] private float dashSpeed = 675f; //ダッシュしたときの速さ
    [SerializeField] private float jumpPower = 420f; //ジャンプ力
    [SerializeField] private float rotateSpeed = 90f; //回転速度
    [SerializeField] private AudioClip se; //効果音を指定
    [SerializeField] private GameObject jumpEffect; //ジャンプ時生成するエフェクト
    [SerializeField] private GameObject effectGenerator; //エフェクトを生成する位置
    [SerializeField] private GameObject panel; //ポーズ画面にするパネルを指定
    [SerializeField] private GameObject pauseButton; //ポーズ時に操作できるボタン
    [SerializeField] private GameObject mainButton; //ゲームオーバー時のボタン
    [SerializeField] private PhysicMaterial pm; //プレイヤー用のPhysicMaterial
    private CapsuleCollider cCol;

    //InputActionsに設定したキーの情報
    private InputAction jumpAction,
                        dashAction,
                        pauseAction,
                        stickMoveAction;

    private bool jumpFlag = false; //ジャンプ状態かどうか
    private bool dashFlag = false; //ダッシュ状態かどうか
    private bool pushPauseFlag = false; //ポーズボタンが押されたかどうか
    private bool pauseFlag = false; //ポーズ画面を開いているか
    private bool groundFlag = false; //地面に触れているかどうか

    private Transform mainCam; //メインカメラのTransformを取得
    private Rigidbody rb;
    private AudioSource audioSource;
    //自分の座標を取得する
    static public Vector3 GetPos()
    {
        if (I == null)
        {
            return Vector3.zero;
        }
        return I.transform.position;
    }
    private void Awake()
    {
        //InputActionsに登録されている入力情報を取得
        var playerInput = GetComponent<PlayerInput>();
        var actionMap = playerInput.currentActionMap;
        jumpAction = actionMap["Jump"];
        dashAction = actionMap["Dash"];
        pauseAction = actionMap["Pause"];
        stickMoveAction = actionMap["Move"];
    }
    void Start()
    {
        Time.timeScale = 1;
        I = this;
        mainCam = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        cCol = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        //入力を受け取り、フラグを操作する
        InputDash();
        Pause();
        if(!pauseFlag)
        {
            Jump();
        }
        //接地してなければPhysicMaterialを設定
        if (!groundFlag)
        {
            cCol.material = pm;
        }
        else
        {
            cCol.material = null;
        }
    }

    private void FixedUpdate()
    {
        //プレイヤーの挙動処理（実際に動くところ）
        Move();
    }
    private void InputDash()
    {
        //ダッシュの入力
        dashFlag = dashAction.IsPressed();
    }
    private void Move()
    {
        //左スティックの入力情報を取得
        Vector2 ls = stickMoveAction.ReadValue<Vector2>();

        //カメラの方向を取得し、Y軸の回転を無視
        Vector3 cameraForward = mainCam.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize(); //カメラの向きを正規化

        //プレイヤーをカメラの向いている方向に移動させる
        Vector3 movement = cameraForward * ls.y + mainCam.right * ls.x;
        movement.Normalize();

        rb.AddForce(movement * (dashFlag ? dashSpeed : speed) * Time.fixedDeltaTime, ForceMode.Force);
        //ゼロでない移動がある場合
        if (movement.magnitude > 0.1f)
        {
            //カメラの向きに基づいて移動方向を設定
            Vector3 lookDirection = Vector3.ProjectOnPlane(movement, Vector3.up);
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed);
            }
        }

    }

    private void Jump()
    {
        jumpFlag = jumpAction.triggered;
        if (jumpFlag && groundFlag)
        {
            //生成用のオブジェクトの座標を代入しエフェクトを生成する
            Vector3 newPos = effectGenerator.transform.position;
            jumpFlag = false;
            audioSource.PlayOneShot(se);
            Instantiate(jumpEffect, newPos, jumpEffect.transform.rotation);
            rb.AddForce(transform.up * jumpPower * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }
    private void Pause()
    {
        //ポーズ画面を開きもう一度押したら閉じる
        pushPauseFlag = pauseAction.triggered;
        if (!pauseFlag && pushPauseFlag)
        {
            pauseFlag = true;
            EventSystem.current.SetSelectedGameObject(pauseButton);
            Time.timeScale = 0;
            panel.SetActive(true);
        }
        else if (pauseFlag && pushPauseFlag)
        {
            pauseFlag = false;
            Time.timeScale = 1;
            EventSystem.current.SetSelectedGameObject(mainButton);
            panel.SetActive(false);
        }

    }
    private void OnTriggerStay(Collider other) 
    {
        //地面に触れたら接地フラグをtrueにする
        if (other.gameObject.CompareTag("Ground"))
        {
            groundFlag = true;
        }

    }
    private void OnTriggerExit(Collider other) 
    {
        //何も触れなかったら接地フラグをfalseにする
        groundFlag = false;
    }
}
