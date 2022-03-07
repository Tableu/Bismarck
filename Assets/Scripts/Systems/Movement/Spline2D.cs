using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Systems.Movement
{
    public class Spline2D
    {
        private List<Polynomial> _segments = new List<Polynomial>();

        public Spline2D(Vector2 p0, Vector2 v0, Vector2 a0)
        {
            _segments.Add(new Polynomial
            {
                t0 = 0,
                a = 0.5f * a0,
                b = v0,
                c = p0
            });
        }

        public void AddSegment(float t0, Vector2 a)
        {
            var i = FindSegmentIndex(t0);
            if (Math.Abs(t0 - _segments[i].t0) < 1e-6)
            {
                Debug.Assert(i != 0, "Can't insert at or before start of spline");
                _segments.RemoveAt(i);
            }

            var tprev = _segments[i].t0;
            var delta = t0 - tprev;
            var aprev = _segments[i].a;
            var bprev = _segments[i].b;
            var cprev = _segments[i].c;
            _segments.Insert(i + 1, new Polynomial
            {
                t0 = t0,
                a = 0.5f * a,
                b = 2 * aprev * delta + bprev,
                c = aprev * delta * delta + bprev * delta + cprev
            });

        }

        public Vector2 Evaluate(float t)
        {
            var i = FindSegmentIndex(t);
            return _segments[i].Evaluate(t);
        }

        public Vector2 ClosestPointOnSpline(Vector2 p)
        {
            var minDist = float.PositiveInfinity;
            var globalMin = 0f;
            for (int i = 0; i < _segments.Count; i++)
            {
                float tf = float.PositiveInfinity;
                if (i != _segments.Count - 1) tf = _segments[i + 1].t0;

                var (min, dist) = _segments[i].ClosestPoint(p, tf);
                if (dist < minDist)
                {
                    minDist = dist;
                    globalMin = min;
                }
            }
            return Evaluate(globalMin);
        }

        private int FindSegmentIndex(float t)
        {
            int i;
            if (t < _segments[0].t0)
            {
                i = 0;
            }
            else if (t >= _segments.Last().t0)
            {
                i = _segments.Count - 1;
            }
            else
            {
                for (i = 0; i < _segments.Count - 1; i++)
                {
                    if (t >= _segments[i].t0 && t < _segments[i + 1].t0)
                    {
                        break;
                    }
                }
            }
            return i;
        }

        private struct Polynomial
        {
            public float t0;
            public Vector2 a;
            public Vector2 b;
            public Vector2 c;

            public Vector2 Evaluate(float t)
            {
                t -= t0;
                return a * t * t + b * t + c;
            }

            public (float, float) ClosestPoint(Vector2 d, float tf)
            {
                var cd = c - d;
                // todo: avoid heap allocation here
                var polynomial = new MathNet.Numerics.Polynomial(
                    2 * Vector2.Dot(b, cd),
                    4 * Vector2.Dot(a, cd) + 2 * Vector2.Dot(b, b),
                    6 * Vector2.Dot(a, b),
                    4 * Vector2.Dot(a, a)
                );
                var roots = polynomial.Roots();
                var secondDerivative = polynomial.Differentiate();
                float offset = t0;
                var realLocalMinima = roots.Where(root => root.Imaginary == 0 && secondDerivative.Evaluate(root).Real >= 0).Select(root => (float)root.Real + offset).ToList();
                realLocalMinima.Add(t0);
                if (!float.IsInfinity(tf))
                {
                    realLocalMinima.Add(tf);
                }

                var minDist = float.PositiveInfinity;
                var globalMin = t0;

                foreach (var minima in realLocalMinima)
                {
                    if (minima < t0 || minima > tf)
                    {
                        continue;
                    }
                    Debug.DrawLine(d, Evaluate(minima), Color.magenta);
                    var dist = (Evaluate(minima) - d).magnitude;
                    if (dist < minDist)
                    {
                        minDist = dist;
                        globalMin = minima;
                    }
                }

                return (globalMin, minDist);
            }
        }
    }
}
