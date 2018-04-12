using UnityEngine;

/// <summary>
/// 
/// </summary>
public interface IPiece
{
    int TeamId { get; }
    Vector3 Position { get; }
}