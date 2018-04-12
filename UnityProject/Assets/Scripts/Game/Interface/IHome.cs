/// <summary>
/// 
/// </summary>
public interface IHome : IPiece
{
    int BoldiCount { get; }
    void LaunchBoldies(IHome destination, EAmount amount);
}