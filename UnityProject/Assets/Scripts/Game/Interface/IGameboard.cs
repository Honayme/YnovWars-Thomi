/// <summary>
/// 
/// </summary>
public interface IGameboard
{
    int NeutralTeamId { get; }

    /// <summary>
    /// Returns all the existing Homes
    /// </summary>
    IHome[] Homes { get; }

    /// <summary>
    /// Selects Homes with the same TeamId
    /// </summary>
    /// <param name="teamId"></param>
    /// <param name="belongToTeam">Says if we want the Home of our Team or the ones from others</param>
    /// <returns></returns>
    IHome[] GetHomes(int teamId, bool belongToTeam);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    IBoldi[] GetBoldies(int teamId);
    
    /// <summary>
    /// Helps to know the move speed of a boldy (units per seconds)
    /// </summary>
    float BoldiSpeed { get; }
}