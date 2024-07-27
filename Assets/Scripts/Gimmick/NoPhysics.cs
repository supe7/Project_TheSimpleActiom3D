using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//プレイヤーが壁に引っかからないようにする
public class NoPhysics : MonoBehaviour
{
    [SerializeField] private PhysicMaterial pm; //壁用のPhysicMaterial
    private Collider col;

    void Start()
    {
        //オブジェクトの角度がz90度or-90度ならPhysicMaterialを設定
        if(transform.eulerAngles.z == 90 || transform.eulerAngles.z == -90)
        {
            col = GetComponent<Collider>();
            col.material = pm;
        }
    }
}
