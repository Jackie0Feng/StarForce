using StarForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMenuForm : UGuiForm
{
    public void OnStartButtonClick()
    {
        (GameEntry.Procedure.CurrentProcedure as ProcedureMyMenu).StartGame();
    }

}
