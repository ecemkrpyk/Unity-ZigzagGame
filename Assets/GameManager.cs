using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameStarted;
 
    void Start()
    {
        StartGame();
        
        Invoke("StartGame", 1f); //oyun başladıktan 1 saniye bekler sonra koşmaya başlar
    }
    public void StartGame()
    {
        gameStarted = true; //oyun başladı
    }
    public void RestartGame() //düştüğünde oyun tekrar başlaması lazım
    {
        Invoke("Restart", 1f); //gecikmeli başlatır
    }
    private void Restart()
    {
        SceneManager.LoadScene(0); //düştüğünde sahneyi tekrar yükler, tek bir sahne var
    }
}
