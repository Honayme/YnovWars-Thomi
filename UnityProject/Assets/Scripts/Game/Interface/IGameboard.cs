/// <summary>
/// 
/// </summary>
public interface IGameboard
{
    IHome[] Homes { get; }
    IHome[] GetHomes(int teamId);
}