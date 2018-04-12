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
            LaunchRandom();
        }
    }

    #endregion


    #region Private Manipulators

    void LaunchRandom()
    {
        // find a home which is mine
        IHome[] myHomes = m_Gameboard.GetHomes(TeamId, true);
        IHome[] theirHomes = m_Gameboard.GetHomes(TeamId, false);

        // launch boldies
        if (myHomes.Length > 0 && theirHomes.Length > 0)
            myHomes[Random.Range(0, myHomes.Length)].LaunchBoldies(theirHomes[0], EAmount.Half);
    }

    #endregion


    #region Public Accessors

    #endregion
}