using System;
using System.Collections.Generic;
using UnityEngine;
using XKTools;

/// <summary>
/// 
/// </summary>
public class Piece : GameboardComp
{
    #region Members
    
    static Dictionary<Type, int>        s_CreatedObjectCounter          = new Dictionary<Type, int>();

    Transform                           m_Root                          = null;

    #endregion


    #region Inherited Manipulators

    /// <summary>
    /// 
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        
        CreateRenderer();
    }

    #endregion


    #region Protected Manipulators
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected GameObject LoadGameObject()
    {
        string path = string.Format("Game/Piece/{0}", GetType().ToString().ClassNameClean());
        GameObject source = Resources.Load<GameObject>(path);
        if (source.IsValid("Piece.LoadGameObject() - " + path))
        {
            GameObject res = GameObject.Instantiate(source);
            res.name = source.name + "_" + GetCreatedObjectCount();
            return res;
        }

        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void CreateRenderer()
    {
        GameObject obj = LoadGameObject();
        if (obj != null)
            m_Root = obj.transform;
    }
    
    #endregion


    #region Private Manipulators

    int GetCreatedObjectCount(bool increment = true)
    {
        if (!s_CreatedObjectCounter.ContainsKey(GetType()))
            s_CreatedObjectCounter[GetType()] = 0;

        if (increment)
            s_CreatedObjectCounter[GetType()]++;

        return s_CreatedObjectCounter[GetType()];
    }

    #endregion


    #region Public Accessors

    /// <summary>
    /// 
    /// </summary>
    public Transform Root
    {
        get { return m_Root; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent"></param>
    public void SetParent(Transform parent)
    {
        if (m_Root != null)
            m_Root.parent = parent;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    public void SetPosition(Vector3 position)
    {
        if (m_Root != null)
            m_Root.position = position;
    }

    #endregion
}