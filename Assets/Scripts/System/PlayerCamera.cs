using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//メインカメラの操作
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target; //被写体を指定
    [SerializeField] private float distanceToPlayer = 2.0f; //カメラとプレイヤーとの距離
    [SerializeField] private float slideDistance; //カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ
    [SerializeField] private float height = 1.2f; //注視点の高さ
    [SerializeField] private float sensitivity = 10f; //感度
    [SerializeField] private float downLimit = 0.3f; //下方向の上限値
    [SerializeField] private float upLimit = -0.8f; //上方向の上限値
    private GameObject targetObj;
    private Vector3 targetPos;

    //InputActionsに設定したキーの情報
    private InputAction lookAction, resetActionR, resetActionL;
    private Vector2 look;

    void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        var actionMap = playerInput.currentActionMap;
        lookAction = actionMap["Look"];
        resetActionR = actionMap["CameraResetRight"];
        resetActionL = actionMap["CameraResetLeft"];

        if (target == null)
        {
            Debug.LogError("ターゲットが設定されていない");
            Application.Quit();
        }
        targetObj = GameObject.Find("Player");
        targetPos = targetObj.transform.position;
    }
    void Update()
    {
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;
    }

    private void FixedUpdate()
    {
        look = lookAction.ReadValue<Vector2>();
        var rotX = look.x * Time.deltaTime * sensitivity;
        var rotY = look.y * Time.deltaTime * sensitivity;
        var lookAt = target.position + Vector3.up * height;

        //回転
        transform.RotateAround(lookAt, Vector3.up, rotX);
        //カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
        if (transform.forward.y > downLimit && rotY < 0)
        {
            rotY = 0;
        }
        if (transform.forward.y < upLimit && rotY > 0)
        {
            rotY = 0;
        }
        transform.RotateAround(lookAt, transform.right, rotY);

        //カメラとプレイヤーとの間の距離を調整
        transform.position = lookAt - transform.forward * distanceToPlayer;
        //注視点の設定
        transform.LookAt(lookAt);
        //カメラを横にずらして中央を開ける
        transform.position = transform.position + transform.right * slideDistance;

        
    }

    private void LateUpdate()
    {
        if ((resetActionR.IsPressed() && resetActionL.IsPressed()))
        {
            ResetCamera();
        }
    }

    public void ResetCamera()
    {
        //カメラの向きをプレイヤーの正面方向に向かせる
        Vector3 direction = target.forward;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
