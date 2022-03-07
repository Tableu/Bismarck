using System;
using UnityEngine;

namespace Systems.Movement
{
    public interface IDrawablePath
    {

        bool ClosedPath { get; }

        /// <summary>
        ///     Get points along a path
        /// </summary>
        /// <param name="t">A value between 0 and 1 that parameterize the path</param>
        /// <returns>The point on the path</returns>
        Vector2 PointOnPath(float t);

        /// <summary>
        ///     Invoked everytime the path has changed and must be redrawn
        /// </summary>
        event Action OnPathChanged;
    }
}
