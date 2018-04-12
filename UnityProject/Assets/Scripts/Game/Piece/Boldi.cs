using System.Collections.Generic;
using UnityEngine;
using XKTools;

/// <summary>
/// 
/// </summary>
public class Boldi : Piece
{
    #region Members
    
    static Dictionary<int, Material>        s_Materials         = new Dictionary<int, Material>();

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
    protected override void OnSetTeamId()
    {
        base.OnSetTeamId();

        SetMaterial(s_Materials);
    }

    #endregion


    #region Private Manipulators
    #endregion
}