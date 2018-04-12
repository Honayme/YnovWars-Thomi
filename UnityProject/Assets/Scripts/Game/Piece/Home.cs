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

    static Dictionary<int, Material>        s_Materials             = new Dictionary<int, Material>();

    int                                     m_Id                    = -1;
    int                                     m_BoldiCount            = 0;
    Text                                    m_BoldiCountText        = null;
    float                                   m_GrowRate              = 1.0f;
    XKTimer                                 m_GrowTimer             = null;

    #endregion


    #region Inherited Manipulators

    /// <summary>
    /// 
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        CreateTimer();
        InitProps();
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnSetTeamId()
    {
        base.OnSetTeamId();

        SetMaterial(s_Materials);
    }

    #endregion


    #region Private Manipulators

    void CreateTimer()
    {
        m_GrowTimer = AddXKComponent<XKTimer>();
        m_GrowTimer.OnEnd += OnEndTimer;
    }

    void OnEndTimer()
    {
        // update boldi count
        SetBoldiCount(m_BoldiCount + 1);

        // restart timer
        m_GrowTimer.StartTimer(1.0f / m_GrowRate);
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
        //Home toHome = (Home)to;
    }

    #endregion
}