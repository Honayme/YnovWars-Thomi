/// <summary>
/// 
/// </summary>
public interface IHome : IPiece
{
    int BoldiCount { get; }
    float GrowRate { get; }
    bool LaunchBoldies(IHome destination, EAmount amount, AIBase ai);
}