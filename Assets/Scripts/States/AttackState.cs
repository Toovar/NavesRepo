using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    //Variables del boss

    private BossController boss;

    //Accedemos a las variables del boss

    private void Awake()
    {
        boss = GameObject.Find("NaveJefe").gameObject.GetComponent<BossController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Disparamos balas mientras se ejecuta la animación

        boss.StartCoroutine("Fire");

        //Si al boss le queda la mitad de la vida, disparamos además balas que apuntan al player

        if (boss.fase2 == true)
            boss.StartCoroutine("FireFocus");
    }
}
