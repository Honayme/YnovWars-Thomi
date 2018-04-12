using UnityEngine;
using XKTools;

/// <summary>
/// 
/// </summary>
public class AITester : AIBase
{
    #region Members

    #endregion


    #region Inherited Manipulators

    /// <summary>
    /// 
    /// </summary>
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.A))
        {
            // find a home which is mine
            IHome[] homes = m_Gameboard.GetHomes(-1);

            // launch boldies
            if (homes.Length > 1)
                homes[0].LaunchBoldies(homes[1], EAmount.Half);
        }
    }

    #endregion


    #region Private Manipulators

    #endregion


    #region Public Accessors

    #endregion
}