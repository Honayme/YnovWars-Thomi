using UnityEngine;

/// <summary>
/// 
/// </summary>
public interface IPiece
{
    /// <summary>The Id of the owner team</summary>
    int TeamId { get; }
    /// <summary>The current world position of the Piece</summary>
    Vector3 Position { get; }
}