using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Canvas creditsCanvas;
    [SerializeField] Canvas mainMenu;
    void Start(){
        creditsCanvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void StartGame(){
        SceneManager.LoadScene("GameScene");
    }

    public void ToggleCredits(){
        creditsCanvas.gameObject.SetActive(!creditsCanvas.gameObject.activeInHierarchy);
        mainMenu.gameObject.SetActive(!creditsCanvas.gameObject.activeInHierarchy);
    }

    public void ExitGame(){
        Application.Quit();
    }
}
