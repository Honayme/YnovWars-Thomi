using XKTools;

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


    #region Callback(s)

    /// <summary>
    /// Called when an ai launches bodies (my self included)
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public virtual void OnBoldiLaunch(IHome from, IHome to)
    {

    }

    #endregion
}