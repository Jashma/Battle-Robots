using UnityEngine;
using System.Collections;

public class RepearController : SupportBotController
{
    public int repearPower = 10;
    public Animator animator;
    private int processAction = 0;

    //Debug
    public Vector3 moveDebugPosition = Vector3.zero;

    void Start()
    {
        thisTransform = transform;

        float heightTerrain = Terrain.activeTerrain.SampleHeight(thisTransform.position);

        targetPosition = thisTransform.position - new Vector3(0, heightTerrain, 0);
        action = SM_BotAction.Repear;
    }

    void FixedUpdate()
    {
        if (moveDebugPosition != Vector3.zero) { targetPosition = moveDebugPosition; }

        ClearProcessAction();
        Move();
    }

    void ClearProcessAction()
    {
        if (processAction > 0)
        {
            animator.SetBool("Action", true);
            processAction--;

            if (processAction <= 0)
            {
                animator.SetBool("Action", false);
            }
        }
    }

    public override void ProcessAction(BotController botController)
    {
        if (botController.botAction == action)
        {
            targetPosition = botController.thisTransform.position;

            if (readyAction == true)
            {
                base.ProcessAction();

                processAction = 10;

                botController.health += repearPower * Time.deltaTime;

                foreach (ModulBasys modul in botController.modulController)
                {
                    RepearModul(modul, repearPower);
                    modul.ReloadWeapon();
                }

                if (botController.health >= 100)
                {
                    botController.health = 100;
                    botController.botAction = SM_BotAction.None;
                }
            }
        }
    }

    void RepearModul(ModulBasys modul, int repearValue = 10)
    {
        modul.healthModul += repearValue * Time.deltaTime;
        
        if (modul.healthModul >= 100)
        {
            modul.healthModul = 100;
        }
    }

    public override void Move()
    {
        base.Move();
    }
}
