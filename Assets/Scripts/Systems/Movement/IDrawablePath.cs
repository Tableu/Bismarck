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
        Vector2 Evaluate(float t);
        Vector2 EvaluateVelocity(float t);

        (float start, float end)[] IntervalsInBounds(Bounds bounds);

        float ClosestPointOnPath(Vector2 p);

        float CurrentTime { get; }

        /// <summary>
        ///     Invoked everytime the path has changed and must be redrawn
        /// </summary>
        event Action OnPathChanged;
    }
}
