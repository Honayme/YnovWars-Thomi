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

    static Dictionary<int, Material>        s_Materials             = new Dictionary<int, Material>();

    int                                     m_Id                    = -1;
    int                                     m_BoldiCount            = 0;
    Text                                    m_BoldiCountText        = null;

    #endregion


    #region Inherited Manipulators

    /// <summary>
    /// 
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

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

    void InitProps()
    {
        // init BoldiCount
        SetBoldiCount(Lehmer.Range(0, 50));

        // scale according to start BoldiCount
        SetScale(Vector3.one * (1.0f + m_BoldiCount * c_ScalePerBoldi));
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

    #endregion
}