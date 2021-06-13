using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    static BackgroundMusic instance;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this; //şuan çalıştırılan obje demek
        }
        else if(instance !=this) //bir sonraki sahnede yüklenen music bu değilse onu yok edecek, önceki kalacak
        {
            Destroy(this.gameObject); //bir sonraki sahnedeki music yok edecek, önceki yüklenen bg music kalmış olacak yani
        }
        //oyun yeniden başlarsa bu ojeyi yok etme,music tekrar tekrar başladığı için kullandık
        DontDestroyOnLoad(this.gameObject);
    }

   
}
