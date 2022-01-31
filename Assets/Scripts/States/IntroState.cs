using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroState : StateMachineBehaviour
{
    //Variables del boss

    private BossController boss;

    //Accedemos a las variables del boss

    private void Awake()
    {
        boss = GameObject.Find("NaveJefe").gameObject.GetComponent<BossController>();
    }

    //Quitamos la invencibilidad al boss cuando termine la animación

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.DisableInvincible();
    }
}
