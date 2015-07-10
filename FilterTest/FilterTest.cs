using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Filters;

namespace FilterTest
{
    [TestClass]
    public class FilterTest
    {
        [TestMethod]
        public void TestMerge()
        {
            Gaussian g = Gaussian.MergeGaussian(new Gaussian(10, 8), new Gaussian(13, 2));
            Assert.AreEqual(12.4, g.Mu, 0.0000001);
            Assert.AreEqual(1.6, g.Sigma, 0.00001);
        }

        [TestMethod]
        public void TestMerge2()
        {
            Gaussian g = Gaussian.MergeGaussian(new Gaussian(10, 4), new Gaussian(12, 4));
            Assert.AreEqual(11.0, g.Mu, 0.0000001);
            Assert.AreEqual(2.0, g.Sigma, 0.00001);
        }

        [TestMethod]
        public void TestEvaluate()
        {
            double v = new Gaussian(10, 2).Evaluate(8);
            Assert.AreEqual(0.121, v, 0.001);
        }

        [TestMethod]
        public void TestAdd()
        {
            Gaussian g = Gaussian.Add(new Gaussian(8, 4), 10, 6); 
            Assert.AreEqual(18, g.Mu, 0.001);
            Assert.AreEqual(10, g.Sigma, 0.001);
        }

        [TestMethod]
        public void TestKalman()
        {
            double[] measurements = new double[] {5, 6, 7, 9 ,10};
            double[] motion = new double[] {1, 1, 2, 1, 1};

            double mu = 0;
            double sigma = 1000;

            double measureSigma = 4.0; 
            double motionSig = 2.0;

            double[] expectedUpdateMu     = new double[] { 4.98, 5.992, 6.996, 8.998, 9.999};
            double[] expectedPredictMu    = new double[] { 5.98, 6.992, 8.996, 9.998, 10.999};
            double[] expectedUpdateSigma  = new double[] { 3.984, 2.397, 2.095, 2.023, 2.005};
            double[] expectedPredictSigma = new double[] { 5.984, 4.397, 4.095, 4.023, 4.006 };

            KalmanFilter k = new KalmanFilter(new Gaussian(mu, sigma));

            for (int i = 0; i < measurements.Length; i++)
            {
                k.Update(measurements[i], measureSigma);
                Assert.AreEqual(expectedUpdateMu[i], k.Mu, 0.001);
                Assert.AreEqual(expectedUpdateSigma[i], k.Sigma, 0.001);

                k.Predict(motion[i], motionSig);
                Assert.AreEqual(expectedPredictMu[i], k.Mu, 0.001);
                Assert.AreEqual(expectedPredictSigma[i], k.Sigma, 0.001);
            }
        }


    }
}
