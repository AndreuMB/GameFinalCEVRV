using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject restartMenu;
    bool isShowing;
    bool gameOverBool = false;
    [SerializeField] Text title;
    // Start is called before the first frame update
    void Start()
    {
        title.text = "PAUSE";
        GameManager.gameOverEv.AddListener(showGameOver);
        restartMenu.SetActive(false);
    }

    public void showGameOver(){
        gameOverBool = true;
        title.text = "GAME OVER";
        restartMenu.SetActive(true);
        GameObject.Find("Resume").GetComponent<Button>().interactable = false;
        // Cursor.lockState = CursorLockMode.Confined;
        // Cursor.visible = true;
    }

    // Update is called once per frame
    void Update() {
        // when gameover can't remove menu
        if (Input.GetKeyDown("escape") && !gameOverBool) {

            // hide other canvas if open
            Canvas[] canvasOvjects = GameObject.FindObjectsOfType<Canvas>();
            const string HUD = "HUD";
            foreach (var canvas in canvasOvjects)
            {
                // if the canvas is the HUD does nothing
                if (canvas.gameObject.tag == HUD) break;

                if (canvas.gameObject.activeInHierarchy){
                    canvas.gameObject.SetActive(false);
                    return;
                }
            }

            // show and hide game menu
            isShowing = !restartMenu.activeInHierarchy;
            restartMenu.SetActive(isShowing);
            if (isShowing) GameObject.Find("Restart").GetComponent<Button>().interactable = true;
        }
    }
}
