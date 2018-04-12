using System.Collections.Generic;
using UnityEngine;
using XKTools;

/// <summary>
/// 
/// </summary>
public class Boldi : Piece, IBoldi
{
    #region Members
    
    const float                             c_Speed             = 10.0f;
    static Dictionary<int, Material>        s_Materials         = new Dictionary<int, Material>();

    Home                                    m_StartHome         = null;
    Home                                    m_Destination       = null;

    #endregion


    #region Inherited Manipulators

    /// <summary>
    /// 
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

    }

    /// <summary>
    /// 
    /// </summary>
    public override void Update()
    {
        base.Update();

        UpdateMoveTo();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Reset()
    {
        base.Reset();

        m_Destination = null;
        m_StartHome = null;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Shutdown()
    {
        base.Shutdown();

        // nullify pointers
        m_Destination = null;
        m_StartHome = null;
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnSetTeamId()
    {
        base.OnSetTeamId();

        // update material color
        SetMaterial(s_Materials);
    }

    #endregion


    #region Private Manipulators

    void UpdateMoveTo()
    {
        Vector3 dir = m_Destination.Position - Position;

        // the boldi has arrived
        if (dir.sqrMagnitude < 1.0f)
        {
            m_Destination.OnHit(this);
            return;
        }

        // keep moving to destination
        SetPosition(Position + dir.normalized * Time.deltaTime * c_Speed);
    }

    #endregion


    #region Public Manipulators

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startHome"></param>
    /// <param name="destination"></param>
    public void MoveTo(Home startHome, Home destination)
    {
        m_StartHome = startHome;
        m_Destination = destination;
    }

    #endregion
}