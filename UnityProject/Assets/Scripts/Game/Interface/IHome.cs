/// <summary>
/// 
/// </summary>
public interface IHome : IPiece
{
    int BoldiCount { get; }
    float GrowRate { get; }
    void LaunchBoldies(IHome destination, EAmount amount);
}