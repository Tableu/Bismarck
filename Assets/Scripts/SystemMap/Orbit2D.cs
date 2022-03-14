using System;
using UnityEngine;

namespace SystemMap
{
    public class Orbit2D
    {
        private CartesianElements _cartesianElements;
        private KeplerianElements _keplerianElements;

        private double _mu;
        private ValidElements _valid;

        public Orbit2D(KeplerianElements keplerianElements, double mu)
        {
            _keplerianElements = keplerianElements;
            _mu = mu;
            _valid = ValidElements.Keplerian;
        }

        public Orbit2D(CartesianElements cartesianElements, double mu)
        {
            _cartesianElements = cartesianElements;
            _mu = mu;
            _valid = ValidElements.Cartesian;
        }

        public CartesianElements Cartesian
        {
            get
            {
                if ((_valid & ValidElements.Cartesian) == ValidElements.None)
                {
                    UpdateCartesian();
                }
                return _cartesianElements;
            }
        }

        public KeplerianElements Keplerian
        {
            get
            {
                if ((_valid & ValidElements.Keplerian) == ValidElements.None)
                {
                    UpdateKeplerian();
                }
                return _keplerianElements;
            }
        }

        public void UpdateFromElements(KeplerianElements keplerianElements)
        {
            _keplerianElements = keplerianElements;
            _valid = ValidElements.Keplerian;
        }

        public void UpdateFromElements(CartesianElements cartesianElements)
        {
            _cartesianElements = cartesianElements;
            _valid = ValidElements.Cartesian;
        }

        public void GetPointsOnOrbit(Vector3[] points, int numPoints)
        {
            if ((_valid & ValidElements.Keplerian) == ValidElements.None)
            {
                UpdateCartesian();
            }
            var inc = 2f * Mathf.PI / numPoints;
            float eccentricAnomaly = 0;

            for (int i = 0; i < numPoints; i++)
            {
                eccentricAnomaly += inc;
                var v = ComputeTrueAnomaly(eccentricAnomaly, _keplerianElements.eccentricity);
                var r = _keplerianElements.semimajorAxis * (1 - _keplerianElements.eccentricity * Math.Cos(eccentricAnomaly));

                points[i] = new Vector3(
                    (float)(r * Math.Cos(v + _keplerianElements.argumentOfPeriapsis)),
                    (float)(r * Math.Sin(v + _keplerianElements.argumentOfPeriapsis)),
                    0);
            }
        }
        private void UpdateCartesian()
        {

            var e = _keplerianElements.eccentricity;
            var a = _keplerianElements.semimajorAxis;
            var w = _keplerianElements.argumentOfPeriapsis;

            var E = KeplerSolver(_keplerianElements.meanAnomaly, e);
            var trueAnomaly = ComputeTrueAnomaly(E, e);

            var rc = a * (1 - e * Math.Cos(E));

            var vx = Math.Sqrt(_mu * a) / rc * -1 * Math.Sin(E);
            var vy = Math.Sqrt(_mu * a) / rc * Math.Sqrt(1 - e * e) * Math.Cos(E);


            _cartesianElements = new CartesianElements
            {
                x = rc * Math.Cos(trueAnomaly + w),
                y = rc * Math.Sin(trueAnomaly + w),
                vx = vx * Math.Cos(w) - vy * Math.Sin(w),
                vy = vx * Math.Sin(w) + vy * Math.Cos(w),
            };
            _valid |= ValidElements.Cartesian;
        }

        private void UpdateKeplerian()
        {
            double h = _cartesianElements.x * _cartesianElements.vy - _cartesianElements.y * _cartesianElements.vx;

            double rmag = Math.Sqrt(_cartesianElements.x * _cartesianElements.x + _cartesianElements.y * _cartesianElements.y);
            double ex = (h * _cartesianElements.vy) / _mu - _cartesianElements.x / rmag;
            double ey = (-h * _cartesianElements.vx) / _mu - _cartesianElements.y / rmag;
            var emag = Math.Sqrt(ex * ex + ey * ey);

            double v;
            if (emag != 0)
            {
                v = Math.Acos((ex * _cartesianElements.x + ey * _cartesianElements.y) / (emag * rmag));
            }
            else
            {
                v = Math.Acos(_cartesianElements.x / rmag);
            }
            var w = Math.Atan2(ey, ex);
            if (_cartesianElements.x * _cartesianElements.vx + _cartesianElements.y * _cartesianElements.vy < 0)
            {
                v = 2 * Math.PI - v;
                w = 2 * Math.PI - w;
            }

            var E = 2 * Math.Atan(Math.Tan(v / 2) / Math.Sqrt((1 + emag) / (1 - emag)));
            var M = E - emag * Math.Sin(E);
            var vmag_sqr = _cartesianElements.vy * _cartesianElements.vy + _cartesianElements.vx * _cartesianElements.vx;
            var a = 1 / (2 / rmag - vmag_sqr / _mu);

            _keplerianElements = new KeplerianElements
            {
                eccentricity = emag,
                meanAnomaly = M,
                semimajorAxis = a,
                argumentOfPeriapsis = w
            };
            _valid |= ValidElements.Keplerian;
        }
        private static double KeplerSolver(double M, double e, double accuracy = 1e-6, int maxIter = 100)
        {
            double E = M;
            double Enext;

            double delta = 1;
            int count = 0;
            while (delta > accuracy && count < maxIter)
            {
                Enext = E - (E - e * Math.Sin(E) - M) / (1 - e * Math.Cos(E));
                delta = Math.Abs(E - Enext);
                E = Enext;
                count++;
            }
            Debug.Log($"Took {count} iterations");

            return E;
        }
        private static double ComputeTrueAnomaly(double eccentricAnomaly, double eccentricity)
        {
            return 2 * Math.Atan2(
                Math.Sqrt(1 + eccentricity) * Math.Sin(eccentricAnomaly / 2),
                Math.Sqrt(1 - eccentricity) * Math.Cos(eccentricAnomaly / 2)
            );
        }

        [Serializable]
        public struct KeplerianElements
        {
            public double eccentricity;
            public double semimajorAxis;
            public double argumentOfPeriapsis;
            public double meanAnomaly;
        }

        [Serializable]
        public struct CartesianElements
        {
            public double x;
            public double y;
            public double vx;
            public double vy;
        }

        [Flags]
        private enum ValidElements
        {
            None = 0,
            Keplerian = 1 << 0,
            Cartesian = 1 << 1
        }
    }
}
