using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text hscoreText, scoreText;
    int score;
    int hscore;

    public ParticleSystem effectPrefab;
    GameManager gameManager { get { return FindObjectOfType<GameManager>(); } }
    public Animator animController { get { return GetComponent<Animator>(); } }

    public float moveSpped = 2;
    public Transform rayOrigin;
    delegate void TurnDelegate();
    TurnDelegate turn; //platforma göre değiştirceğiz

    bool lookingRight = true; //başlangıçta sağa bakar
   
    void Start()
    {
        //oyun açılır açılmaz hscorun gözükmesi lazım
        hscore = PlayerPrefs.GetInt("z_hscore");
        hscoreText.text = hscore.ToString();
        gameManager.StartGame();
    }


    void Update()
    {
        if (gameManager.gameStarted)
        {
            //koşulların değişmesini istediğimizden tetikleyici set edilmeli
            animController.SetTrigger("gameStarted"); //idle den runninge geçer
            Move();

            //platform kontrolü
            #if UNITY_EDITOR
                  turn = TurnWithKeyboard;
            #endif

            #if UNITY_ANDROID
                  turn = TurnWithFinger;
            #endif

            turn();
            
            CheckFalling();


        }
    }
    float freq = 0.5f; //saniyede 2 kere çağıracak, ışın gönderme maliyetli oldugundan method daha az çağırılmış olur
    float elapsedTime = 0; //geçen süre

    private void CheckFalling() //düşmesini kontrol edicez, ışın göndercez
    {
        if ((elapsedTime+=Time.deltaTime)>=freq)
        {
            // ! herhangi bir şeyi kesmiyorsa demek 
            if (!Physics.Raycast(rayOrigin.position, Vector3.down))//rayorigin konumu, aşağı yönü kesmezse  
            {
                animController.SetTrigger("falling"); //düşme animasyonu başlattık
                gameManager.RestartGame();
            }
            elapsedTime = 0; //her seferinde sıfırlamak gerekir
        }


       
    }

    private void TurnWithKeyboard()//sağa sola dönmesi için, bilgisayar platformunda
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Turn();
        }

    }
    private void TurnWithFinger()//sağa sola dönmesi için, telefon platformunda
    {
        float firstTouchX=0; //ilk dokunma ve son dokunma
        float lastTouchX = 0;

        if (Input.touchCount>0) //ekrana temas var
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began: //başladığı zaman değeri alır
                    firstTouchX = Input.GetTouch(0).position.x;
                    break;
                case TouchPhase.Moved: //hareket boyunca
                    lastTouchX = Input.GetTouch(0).position.x;
                    break;
          
                case TouchPhase.Ended: 
                    if (Mathf.Abs(lastTouchX - firstTouchX) > 80) Turn();
                   
                    break;
                
            }
        }     
    }

    private void Turn()//her iki platform içinde geçerli
    {
        moveSpped += Time.deltaTime * 5;
        if (lookingRight) //sağa bakar

            //sola döner
            transform.Rotate(new Vector3(0, -90, 0)); //y ekseninde 90 derece döndürürüz, başlangıçta 45 derece dönük olduğundan -45 derece daha döndürmemiz gerekicez

        else 

            transform.Rotate(new Vector3(0, 90, 0)); //sağa bakmıyorsa sağa dönecek

        //spaceye basınca artık sağa bakmıyordur
        lookingRight = !lookingRight;
    }

    private void Move()
    {
        //playerı hareket ettiricez
        //transform.position += transform.forward * moveSpped * Time.deltaTime;
        transform.Translate(Vector3.forward * moveSpped * Time.deltaTime);
    }

    Vector3 crystalPos;
    //cristalin içinden geçebilmesi için
    private void OnTriggerEnter(Collider other) //other cristal
    {
        if (other.tag.Equals("crystal"))  //cristale çarptığında score yapacak
        {
            MakeScore();
            crystalPos = other.transform.position;
            other.gameObject.SetActive(false); //cristal kaybolur 
            makeEffect();

        }
    }
    //geride kalan blokları yok etmek için:
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject, 2f); //playerın çarptığı her objeyi 2sn sonra yok et
    }


    private void makeEffect()
    {
       var effect= Instantiate(effectPrefab, crystalPos, Quaternion.identity); //rotation: quaternion
        //effect.Play();
        Destroy(effect.gameObject, 1f); //1 sn sonra effect yok edilir
    }

    private void MakeScore()
    {
        score++;
        scoreText.text = score.ToString();
        if (score>hscore)
        {
            hscore = score;
            hscoreText.text = hscore.ToString();
            PlayerPrefs.SetInt("z_hscore", hscore); //kaydetmek için
        }
    }
}
