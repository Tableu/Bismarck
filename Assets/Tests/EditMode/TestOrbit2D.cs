using System;
using NUnit.Framework;
using SystemMap;

namespace Tests.EditMode
{
    public class TestOrbit2D
    {

        [Test]
        public void CartesianFromKeplerianElements()
        {
            var orbit = new Orbit2D(new Orbit2D.KeplerianElements
            {
                eccentricity = 0,
                meanAnomaly = 0,
                semimajorAxis = 1,
                argumentOfPeriapsis = 0
            }, 1);

            var expected = new Orbit2D.CartesianElements
            {
                x = 1,
                y = 0,
                vx = 0,
                vy = 1
            };
            var actual = orbit.Cartesian;
            Assert.AreEqual(expected.x, actual.x);
            Assert.AreEqual(expected.y, actual.y);
            Assert.AreEqual(expected.vx, actual.vx);
            Assert.AreEqual(expected.vy, actual.vy);
        }

        [Test]
        public void KeplerianFromCartesianElements()
        {
            var orbit = new Orbit2D(new Orbit2D.CartesianElements
            {
                x = 1,
                y = 0,
                vx = 0,
                vy = 1
            }, 1);

            var expected = new Orbit2D.KeplerianElements
            {
                eccentricity = 0,
                meanAnomaly = 0,
                semimajorAxis = 1,
                argumentOfPeriapsis = 0
            };

            var actual = orbit.Keplerian;
            Assert.AreEqual(expected.eccentricity, actual.eccentricity);
            Assert.AreEqual(expected.meanAnomaly, actual.meanAnomaly);
            Assert.AreEqual(expected.semimajorAxis, actual.semimajorAxis);
            Assert.AreEqual(expected.argumentOfPeriapsis, actual.argumentOfPeriapsis);
        }

        [TestCase(0, 1, 0, 0)]
        [TestCase(0.85, 25, 0, 0)]
        [TestCase(0.85, 25, Math.PI / 4, 0)]
        [TestCase(0.85, 25, Math.PI / 4, Math.PI / 4)]
        public void ConvertBetweenKeplerianAndCartesian(double e, double a, double w, double M)
        {
            var orbit = new Orbit2D(new Orbit2D.KeplerianElements
            {
                eccentricity = e,
                meanAnomaly = M,
                semimajorAxis = a,
                argumentOfPeriapsis = w
            }, 1);

            var cartesian = orbit.Cartesian;
            orbit.UpdateFromElements(cartesian);
            var actual = orbit.Keplerian;
            Assert.That(actual.eccentricity, Is.EqualTo(e).Within(1e-4));
            Assert.That(actual.meanAnomaly, Is.EqualTo(M).Within(1e-4));
            Assert.That(actual.semimajorAxis, Is.EqualTo(a).Within(1e-4));
            Assert.That(actual.argumentOfPeriapsis, Is.EqualTo(w).Within(1e-4));
        }
    }
}
