using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XKTools;

/// <summary>
/// 
/// </summary>
public class Gameboard : XKObject
{
    #region Members

    const int           c_GridSize          = 5;
    Camera              m_Camera            = null;

    Pool                m_Pool              = null;

    Transform           m_Root              = null;
    Transform           m_HomeRoot          = null;
    Transform           m_BoldiRoot         = null;
    List<Home>          m_Homes             = new List<Home>();
    List<Boldi>         m_Boldies           = new List<Boldi>();

    #endregion


    #region Inherited Manipulators

    /// <summary>
    /// 
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        // set seed for easy tests
        Lehmer.Seed = 2;

        // create gameboard elements
        FindCamera();
        CreateRoots();
        CreatePool();
        CreateMap();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Shutdown()
    {
        base.Shutdown();

        // nullify pointers
        m_Pool = null;
    }

    #endregion


    #region Private Manipulators

    void FindCamera()
    {
        m_Camera = Camera.main;
    }

    void CreateRoots()
    {
        m_Root = new GameObject("Gameboard").transform;
        CreateRoot(ref m_HomeRoot, "Homes"); 
        CreateRoot(ref m_BoldiRoot, "Boldies");
    }

    void CreateRoot(ref Transform root, string rootName)
    {
        root = new GameObject(rootName).transform;
        root.parent = m_Root;
    }

    void CreatePool()
    {
        m_Pool = AddXKComponent<Pool>();
    }

    void CreateMap()
    {
        if (m_Camera.orthographic)
        {
            Vector3 bounds = Vector3.zero;
            bounds.z = m_Camera.orthographicSize;
            bounds.x = bounds.z * Screen.width / Screen.height;
            bounds.x *= 0.85f; // keep 15 % off
            bounds.z *= 0.8f; // keep 20 % off

            CreateHomes(bounds);
        }
        else
        {
            XKLog.Log("Error", "Gameboard.CreateMap() failed - camera is not orthographic");
        }
    }

    void CreateHomes(Vector3 bounds)
    {
        // create random buffer
        IntBufferedRandom rnd = new IntBufferedRandom();
        rnd.AddValueRange(0, c_GridSize * c_GridSize);
        rnd.Range = Lehmer.Range;

        // create homes
        int homeCount = Lehmer.Range(3, c_GridSize * c_GridSize);
        for (int i = 0; i < homeCount; ++i)
            CreateHome(GetPosition(rnd.DrawValue(), bounds));
    }

    Vector3 GetPosition(int idx, Vector3 bounds)
    {
        Vector3 res = Vector3.zero;

        int h = idx / c_GridSize;
        int w = idx - h * c_GridSize;

        res.x = -bounds.x + w * bounds.x * 2.0f / (c_GridSize - 1);
        res.z = -bounds.z + h * bounds.z * 2.0f / (c_GridSize - 1);

        return res;
    }
    
    #endregion


    #region Public Manipulators

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xkActive"></param>
    /// <returns></returns>
    public T CreatePiece<T>(bool xkActive = true)
        where T : Piece, new()
    {
        T res = AddXKComponent<T>(xkActive);
        return res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Home CreateHome(Vector3 position)
    {
        Home res = CreatePiece<Home>();
        m_Homes.Add(res);

        res.SetParent(m_HomeRoot);
        res.SetPosition(position);
        res.TeamId = -1;

        return res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Boldi CreateBoldi()
    {
        Boldi res = CreatePiece<Boldi>();
        m_Boldies.Add(res);

        res.SetParent(m_BoldiRoot);

        return res;
    }

    #endregion


    #region Public Accessors

    /// <summary>
    /// 
    /// </summary>
    public Camera Camera
    {
        get { return m_Camera; }
    }

    /// <summary>
    /// 
    /// </summary>
    public Pool Pool
    {
        get { return m_Pool; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public Color GetColor(int teamId)
    {
        switch (teamId)
        {
            case 0:
                return Color.red;
            case 1:
                return Color.green;
            case 2:
                return Color.blue;
            case 3:
                return Color.yellow;
            case 4:
                return Color.magenta;
            case 5:
                return Color.cyan;
            case 6:
                return Color.white;
        }

        return Color.grey;
    }

    #endregion
}