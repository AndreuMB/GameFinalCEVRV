using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject restartMenu;
    bool isShowing;
    bool gameOverBool = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameOverEv.AddListener(showGameOver);
        restartMenu.SetActive(false);
    }

    public void showGameOver(){
        gameOverBool = true;
        restartMenu.SetActive(true);
        GameObject.Find("Resume").GetComponent<Button>().interactable = false;
        // Cursor.lockState = CursorLockMode.Confined;
        // Cursor.visible = true;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("escape") && !gameOverBool) {
            Canvas[] canvasOvjects = GameObject.FindObjectsOfType<Canvas>();
            // const string HUD = "HUD";
            // foreach (var canvas in canvasOvjects)
            // {
            //     if (canvas.gameObject.tag == HUD) break;

            //     if (canvas.gameObject.activeInHierarchy){
            //         canvas.gameObject.SetActive(false);
            //         return;
            //     }
            // }

            isShowing = !restartMenu.activeInHierarchy;
            restartMenu.SetActive(isShowing);
            if (isShowing) GameObject.Find("Restart").GetComponent<Button>().interactable = true;
        }
    }
}
