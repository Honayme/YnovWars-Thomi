using UnityEngine;
using XKTools;

/// <summary>
/// 
/// </summary>
public class AITester : AIBase
{
    #region Members

    XKTimer             m_Timer             = null;

    #endregion


    #region Inherited Manipulators

    /// <summary>
    /// 
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        m_Timer = AddXKComponent<XKTimer>();
        m_Timer.OnEnd = OnEndTimer;

        StartTimer();
    }

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

    void StartTimer()
    {
        m_Timer.StartTimer(Random.Range(2.0f, 15.0f));
    }

    void OnEndTimer()
    {
        LaunchRandom();
        StartTimer();
    }

    void LaunchRandom()
    {
        // find a home which is mine
        IHome[] myHomes = m_Gameboard.GetHomes(TeamId, true);
        IHome[] theirHomes = m_Gameboard.GetHomes(TeamId, false);

        // launch boldies
        if (myHomes.Length > 0 && theirHomes.Length > 0)
            myHomes[Random.Range(0, myHomes.Length)].LaunchBoldies(theirHomes[Random.Range(0, theirHomes.Length)], (EAmount)Random.Range(0, (int)EAmount.Count));
    }

    #endregion


    #region Public Accessors

    #endregion
}