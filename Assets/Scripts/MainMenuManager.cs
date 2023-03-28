using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Start(){
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void StartGame(){
        // AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/Scenes");
        // string[] scenePaths = myLoadedAssetBundle.GetAllScenePaths();
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame(){
        Application.Quit();
    }
}
