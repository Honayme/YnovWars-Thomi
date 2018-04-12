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

        m_Gameboard = ComponentContainer.AddXKComponent<Gameboard>();
    }

    #endregion


    #region Public Manipulators

    /// <summary>
    /// 
    /// </summary>
    public Gameboard Gameboard
    {
        get { return m_Gameboard; }
    }

    #endregion
}