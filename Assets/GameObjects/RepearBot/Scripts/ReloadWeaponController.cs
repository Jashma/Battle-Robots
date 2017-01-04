using UnityEngine;
using System.Collections;

public class ReloadWeaponController : SupportBotController
{
    public int reloadPower = 10;
    private int processAction = 0;

    public override void ProcessAction(BotController botController)
    {
        if (botController.botAction == action)
        {
            targetPosition = botController.thisTransform.position;

            if (readyAction == true)
            {
                base.ProcessAction();

                processAction = 10;

                botController.health += reloadPower * Time.deltaTime;

                foreach (ModulBasys modul in botController.modulController)
                {
                    RepearModul(modul, reloadPower);
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
}
