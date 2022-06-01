using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingState : StateMachineBehaviour
{
    //Variables del boss

    private BossController boss;

    //Accedemos a las variables del boss

    private void Awake()
    {
        boss = GameObject.Find("NaveJefe").gameObject.GetComponent<BossController>();
    }

    //Activamos los anillos cuando empieza la animación

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.gameObject.activeInHierarchy != false)
            boss.ActiveRing();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Si al boss le queda la mitad de la vida, disparamos además balas que apuntan al player

        if (boss.fase2 == true && boss.gameObject.activeInHierarchy != false)
            boss.StartCoroutine("FireFocus");
    }

    //Desactivamos los anillos al acabar la animación

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.gameObject.activeInHierarchy != false)
            boss.DisableRings();
    }
}
