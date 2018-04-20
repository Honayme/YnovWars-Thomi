
/// <summary>
/// 
/// </summary>
public class AIBase : GameboardCompInterfaced
{
    #region Public Accessors

    /// <summary>
    /// 
    /// </summary>
    public int TeamId { get; set; }

    #endregion


    #region Protected Manipulators

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="amount"></param>
    protected void LaunchBoldies(IHome from, IHome to, EAmount amount)
    {
        from.LaunchBoldies(to, amount, this);
    }

    #endregion


    #region Callback(s)

    /// <summary>
    /// Called when an ai launches bodies (my self included)
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public virtual void OnBoldiLaunch(IHome from, IHome to)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="home"></param>
    /// <param name="formerTeamId"></param>
    public virtual void OnHomeChangedOwner(IHome home, int formerTeamId)
    {
    }

    #endregion
}