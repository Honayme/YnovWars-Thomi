using XKTools;

/// <summary>
/// 
/// </summary>
public class YnovWars : XKBehaviour
{
    #region Members

    Gameboard       m_Gameboard         = null;

    #endregion


    #region Inherited Manipulators

    /// <summary>
    /// 
    /// </summary>
    protected override void Start()
    {
        base.Start();

        EnableLogs();
        CreateGameboard();
    }

    #endregion


    #region Private Manipulators

    void EnableLogs()
    {
        XKLog.EnableLogType("Error", true);
    }

    void CreateGameboard()
    {
        m_Gameboard = ComponentContainer.AddXKComponent<Gameboard>();
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