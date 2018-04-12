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
        CreateAI();
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

    void CreateAI()
    {
        if (m_Gameboard == null)
            return;
        m_Gameboard.CreateAI<AITester>();
        m_Gameboard.CreateAI<AITester>();
        m_Gameboard.CreateAI<AITester>();
        m_Gameboard.CreateAI<AITester>();
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