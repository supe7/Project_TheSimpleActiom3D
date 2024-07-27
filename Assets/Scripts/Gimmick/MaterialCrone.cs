using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//シェーダーの画像を乱れないようにする
public class MaterialCrone : MonoBehaviour
{
    private Material mat; //マテリアルを取得
    private MeshRenderer mesh; //メッシュを取得
    [SerializeField] private Vector2 Tlim = new Vector2(1, 1); //トリミングする数値

    void Start()
    {
        //マテリアルのクローンを生成し、指定した数値でトリミングする
        mesh = GetComponent<MeshRenderer>();
        mat = Instantiate(mesh.material);
        mat.SetVector("_Tlim", Tlim);
        mat.SetVector("_Scroll", mat.GetVector("_Scroll") / Tlim.x);
        mesh.material = mat;
    }
}
