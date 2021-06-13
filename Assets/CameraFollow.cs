using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 diff; //offset için
    //playerın konumunu alabiliyoruz
    Transform player {get { return FindObjectOfType<PlayerController>().transform; } }
   
    void Start()
    {
        //player- camera
        diff = player.position - transform.position; //bu kod cameraya bağlı olduğundan direkt transform.position cameranın konumunu verir.

    }

    
    void LateUpdate() //lateupdate bütün updatelerden sonra çağırılır
    {
        transform.position = player.position - diff; //cameranın konumunu bulabiliriz

    }
}
