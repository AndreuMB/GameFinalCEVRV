using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject restartMenu;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.gameOverEv.AddListener(showGameOver);
        restartMenu.SetActive(false);
    }

    public void showGameOver(){
        restartMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
