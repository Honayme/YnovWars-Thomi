﻿using UnityEngine;
using XKTools;

/// <summary>
/// 
/// </summary>
public class YnovWars : XKBehaviour
{
    #region Members

    Gameboard           m_Gameboard         = null;

    [SerializeField]
    int                 m_Seed              = -1;

    #endregion


    #region Inherited Manipulators

    /// <summary>
    /// 
    /// </summary>
    protected override void Start()
    {
        base.Start();

        EnableLogs();
        InitSeed();
        CreateGameboard();
        CreateAI();
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void Update()
    {
        base.Update();

        if (m_Gameboard != null && !m_Gameboard.XKActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DeleteGameboard();
                CreateGameboard();
                CreateAI();
            }
        }
    }

    #endregion


    #region Private Manipulators

    void InitSeed()
    {
        // set seed for easy tests
        if (m_Seed > 0)
            Lehmer.Seed = m_Seed;
        XKLog.LogWithContext("Info", "YnovWars.Seed: " + Lehmer.Seed, this);
    }

    void EnableLogs()
    {
        XKLog.EnableLogType("Error", true);
        XKLog.EnableLogType("Info", true);
    }

    void CreateGameboard()
    {
        m_Gameboard = ComponentContainer.AddXKComponent<Gameboard>();
    }

    void DeleteGameboard()
    {
        if (m_Gameboard != null)
            ComponentContainer.RemoveXKComponent(ref m_Gameboard);
    }

    void CreateAI()
    {
        if (m_Gameboard == null)
            return;

        if (m_Gameboard.Homes.Length >= 4)
        {
            m_Gameboard.CreateAI<YW.NicoJ.AITester>();
            m_Gameboard.CreateAI<YW.NicoJ.AITester>();
            m_Gameboard.CreateAI<YW.NicoJ.AITester>();
            m_Gameboard.CreateAI<YW.NicoJ.AITester>();
        }
    }

    #endregion


    #region Public Accessors

    /// <summary>
    /// 
    /// </summary>
    public Gameboard Gameboard
    {
        get { return m_Gameboard; }
    }

    #endregion
}