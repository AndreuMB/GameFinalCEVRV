using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationEvent : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject enemyObject = GameObject.FindGameObjectWithTag("Boss"); // Encuentra el objeto del enemigo por la etiqueta "Enemy"
        BossLogic enemyController = enemyObject.GetComponent<BossLogic>(); // Obtén la referencia al script EnemyController del objeto del enemigo

        if (enemyController != null)
        {
            enemyController.AttackEvent(); // Llama al método AttackEvent() en el script EnemyController
        }
    }

    
}
