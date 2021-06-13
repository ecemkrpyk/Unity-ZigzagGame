using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMaker : MonoBehaviour
{
    Transform player { get { return FindObjectOfType<PlayerController>().transform; } }
    public GameObject wallPrefab;

    public Transform lastBlock; //son block bizim ilk blockumuz olcak
    float offset = 0.69f;
    Vector3 lastBlockPosition;

    Camera cam;
    
    void Start()
    {
        cam = Camera.main;
        lastBlockPosition = lastBlock.position; //son blokun konumunu aldık

        InvokeRepeating("Makewall",0,0.25f); //saniyede 4 kere duvarı oluşturur
    }

    
    void Makewall() //yeni blocklar oluşturucaz otomatik
    {
        //karakter ile son block arasındaki farka bakıcaz, ihtiyac kadar blok oluşturucaz
        float distance = Vector3.Distance(player.position, lastBlockPosition);

        if (distance > cam.orthographicSize * 2)//cameranın boyutunu alır yarısı kadar old. dan 2 ile çarptık
            return;


        Vector3 newPos;
        int chance = Random.Range(1, 11); //duvarı nereye öreceği
        if (chance>5) //%50 ihtimalle duvarı ya arkaya yada yana örecek rastgele yani
        {
            newPos = new Vector3(lastBlockPosition.x + offset, lastBlockPosition.y, lastBlockPosition.z + offset); //arkaya
        }
        else //yan tarafına örer
        {
            newPos = new Vector3(lastBlockPosition.x - offset, lastBlockPosition.y, lastBlockPosition.z + offset); //yana
        }
        bool enableCrystal= chance % 3 == 2; //kristal oluşturma

        
        var newBlock = Instantiate(wallPrefab,transform); //dönme için
        newBlock.transform.position = newPos;

        //getchild ile childını aldık kübün childı kristal
        newBlock.transform.GetChild(0).gameObject.SetActive(enableCrystal);
        lastBlockPosition = newBlock.transform.position; //üzerine ekleyecek
    }
}
