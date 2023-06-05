using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationEvent : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject enemyObject = GameObject.FindGameObjectWithTag("Boss");
        if (!enemyObject) return;
        BossLogic enemyController = enemyObject.GetComponent<BossLogic>(); 

        if (enemyController != null)
        {
            enemyController.AttackEvent(); 
        }
    }

    
}
