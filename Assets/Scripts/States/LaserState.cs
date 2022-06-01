using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserState : StateMachineBehaviour
{
    //Variables del boss

    private BossController boss;

    //Accedemos a las variables del boss

    private void Awake()
    {
        boss = GameObject.Find("NaveJefe").gameObject.GetComponent<BossController>();
    }

    //Activamos el laser al inicio de la animación

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.gameObject.activeInHierarchy != false)
        boss.StartCoroutine("ActiveLaser");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Si al boss le queda la mitad de la vida, disparamos además balas que apuntan al player

        if (boss.fase2 == true && boss.gameObject.activeInHierarchy != false)
            boss.StartCoroutine("FireFocus");
    }

    //Desactivamos el laser al acabar la animación

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.gameObject.activeInHierarchy != false)
            boss.StartCoroutine("QuitLaser");
    }
}
