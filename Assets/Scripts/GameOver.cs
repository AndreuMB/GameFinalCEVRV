using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static UnityEvent gameOverEv = new UnityEvent();
    public static void gameOver(){
        Time.timeScale = 0;
        gameOverEv.Invoke();
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
