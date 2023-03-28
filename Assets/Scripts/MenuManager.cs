using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject restartMenu;
    bool isShowing;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameOverEv.AddListener(showGameOver);
        restartMenu.SetActive(false);
    }

    public void showGameOver(){
        restartMenu.SetActive(true);
        GameObject.Find("Resume").GetComponent<Button>().interactable = false;
        // Cursor.lockState = CursorLockMode.Confined;
        // Cursor.visible = true;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("escape")) {
            isShowing = !restartMenu.activeInHierarchy;
            restartMenu.SetActive(isShowing);
            if (isShowing) GameObject.Find("Restart").GetComponent<Button>().interactable = true;
        }
    }
}
