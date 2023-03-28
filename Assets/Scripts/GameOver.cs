using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static UnityEvent gameOverEv = new UnityEvent();

    void OnEnable(){
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    void OnDisable(){
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public static void gameOver(){
        gameOverEv.Invoke();
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Resume(){
        gameObject.SetActive(false);
    }

    public void ExitMainMenu(){
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
