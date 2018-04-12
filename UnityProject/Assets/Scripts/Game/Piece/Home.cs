using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XKTools;

/// <summary>
/// 
/// </summary>
public class Home : Piece, IHome
{
    #region Members

    const float                             c_ScalePerBoldi         = 0.05f;
    const float                             c_GrowPerBoldi          = 0.01f;
    const float                             c_DelayPerBoldi         = 0.1f;

    static Dictionary<int, Material>        s_Materials             = new Dictionary<int, Material>();

    int                                     m_Id                    = -1;
    int                                     m_BoldiCount            = 0;
    Text                                    m_BoldiCountText        = null;
    float                                   m_GrowRate              = 1.0f;
    XKTimer                                 m_GrowTimer             = null;
    XKTimer                                 m_LaunchTimer           = null;

    Dictionary<Home, int>                   m_ToLaunch              = new Dictionary<Home, int>();

    #endregion


    #region Inherited Manipulators

    /// <summary>
    /// 
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        CreateTimers();
        InitProps();
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnSetTeamId()
    {
        base.OnSetTeamId();

        SetMaterial(s_Materials);
        m_ToLaunch.Clear();
    }

    #endregion


    #region Public Manipulators

    /// <summary>
    /// 
    /// </summary>
    /// <param name="boldi"></param>
    public void OnHit(Boldi boldi)
    {
        if (TeamId != boldi.TeamId)
        {
            if (m_BoldiCount > 0)
            {
                SetBoldiCount(m_BoldiCount - 1);
            }
            else
            {
                TeamId = boldi.TeamId;
            }
        }
        else
        {
            SetBoldiCount(m_BoldiCount + 1);
        }

        m_Gameboard.Pool.ReturnBoldi(boldi);
    }

    #endregion


    #region Private Manipulators

    void CreateTimers()
    {
        m_GrowTimer = AddXKComponent<XKTimer>();
        m_GrowTimer.OnEnd += OnEndGrowTimer;

        m_LaunchTimer = AddXKComponent<XKTimer>();
        m_LaunchTimer.OnEnd += OnEndLaunchTimer;
        m_LaunchTimer.StartTimer(c_DelayPerBoldi);
    }

    void OnEndGrowTimer()
    {
        // update boldi count if it belongs to a player
        if (TeamId != -1)
            SetBoldiCount(m_BoldiCount + 1);

        // restart timer
        m_GrowTimer.StartTimer(1.0f / m_GrowRate);
    }

    void OnEndLaunchTimer()
    {
        // update boldi count
        LaunchBoldies();

        // restart timer
        m_LaunchTimer.StartTimer(c_DelayPerBoldi);
    }

    void InitProps()
    {
        // init BoldiCount
        SetBoldiCount(Lehmer.Range(0, 50));

        // scale according to start BoldiCount
        SetScale(Vector3.one * (1.0f + m_BoldiCount * c_ScalePerBoldi));

        // manage grow rate
        m_GrowRate = 1.0f + m_BoldiCount * c_GrowPerBoldi;
        m_GrowTimer.StartTimer(1.0f / m_GrowRate);
    }

    void SetBoldiCount(int count)
    {
        m_BoldiCount = count;
        if (m_BoldiCountText != null)
            m_BoldiCountText.text = count.ToString();
    }

    void LaunchBoldies()
    {
        Boldi boldi;
        Pool pool = m_Gameboard.Pool;
        Dictionary<Home, int> tmp = new Dictionary<Home, int>();

        // launch boldies
        foreach (Home home in m_ToLaunch.Keys)
        {
            if (m_ToLaunch[home] > 0)
            {
                if (m_BoldiCount > 0)
                {
                    boldi = pool.GetBoldi(TeamId);
                    boldi.SetPosition(Position);
                    boldi.MoveTo(this, home);
                    SetBoldiCount(m_BoldiCount - 1);

                    // store new value
                    tmp[home] = m_ToLaunch[home] - 1;
                }
                else
                    tmp[home] = 0;
            }
        }

        // report new values
        foreach (Home home in tmp.Keys)
        {
            m_ToLaunch[home] = tmp[home];
        }
    }

    #endregion


    #region Public Accessors

    /// <summary>
    /// 
    /// </summary>
    public int Id
    {
        get { return m_Id; }
        set { m_Id = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetBoldiCountText(Text text)
    {
        m_BoldiCountText = text;
        SetBoldiCount(m_BoldiCount);
    }

    #endregion


    #region IHome Implementation

    int IHome.BoldiCount
    {
        get { return m_BoldiCount; }
    }

    float IHome.GrowRate
    {
        get { return m_GrowRate; }
    }

    void IHome.LaunchBoldies(IHome to, EAmount amount)
    {
        if (TeamId < 0)
        {
            XKLog.Log("Error", "LaunchBodies() failed - home does not belong to any team");
            return;
        }

        // compute boldiCount to launch
        int boldiCount = m_BoldiCount;
        switch (amount)
        {
            case EAmount.Quarter:
                boldiCount = (int)(m_BoldiCount * 0.25f);
                break;
            case EAmount.Half:
                boldiCount = (int)(m_BoldiCount * 0.5f);
                break;
            case EAmount.ThreeQuarter:
                boldiCount = (int)(m_BoldiCount * 0.75f);
                break;
        }

        // launch them
        if (!m_ToLaunch.ContainsKey((Home)to))
            m_ToLaunch.Add((Home)to, boldiCount);
        else
            m_ToLaunch[(Home)to] += boldiCount;
    }

    #endregion
}