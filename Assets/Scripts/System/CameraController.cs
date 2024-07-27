using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//プレイヤーとカメラの間にあるオブジェクトを透過させる
public class CameraController : MonoBehaviour
{
    // 被写体を指定
    [SerializeField] private Transform subject;
    // 遮蔽物のレイヤー名のリスト
    [SerializeField] private List<string> coverLayerNameList;
    // 遮蔽物とするレイヤーマスク
    private int layerMask;
    // 今回の Update で検出された遮蔽物の Renderer コンポーネント
    public List<Renderer> rendererHitsList = new List<Renderer>();
    // 前回の Update で検出された遮蔽物の Renderer コンポーネント。
    // 今回の Update で該当しない場合は、遮蔽物ではなくなったので Renderer コンポーネントを有効にする
    public Renderer[] rendererHitsPrevs;

    void Start()
    {
        // 遮蔽物のレイヤーマスクを、レイヤー名のリストから合成する。
        layerMask = 0;
        foreach(string layerName in coverLayerNameList)
        {
            layerMask |= 1 << LayerMask.NameToLayer(layerName);
        }
    }
    void Update()
    {
        // カメラと被写体を結ぶ ray を作成
        Vector3 difference = (subject.transform.position - this.transform.position);
        Vector3 direction = difference.normalized;
        Ray ray = new Ray(this.transform.position, direction);
        // 前回の結果を退避してから、Raycast して今回の遮蔽物のリストを取得する
        RaycastHit[] hits = Physics.RaycastAll(ray, difference.magnitude, layerMask);

        rendererHitsPrevs = rendererHitsList.ToArray();
        rendererHitsList.Clear();
        // 遮蔽物は一時的にすべて描画機能を無効にする。
        foreach (RaycastHit hit in hits)
        {
            // 遮蔽物が被写体の場合は例外とする
            if (hit.collider.gameObject == subject)
            {
                continue;
            }
            // 遮蔽物の Renderer コンポーネントを無効にする
            Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                rendererHitsList.Add(renderer);
                renderer.enabled = false;
            }
        }
        // 前回まで対象で、今回対象でなくなったものは、表示を元に戻す。
        foreach (Renderer renderer in rendererHitsPrevs.Except<Renderer>(rendererHitsList))
        {
            // 遮蔽物でなくなった Renderer コンポーネントを有効にする
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }
    }
}
