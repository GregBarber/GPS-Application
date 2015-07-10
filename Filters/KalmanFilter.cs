using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Filters
{
    public class KalmanFilter
    {
        Gaussian g;

        public KalmanFilter(Gaussian g)
        {
            this.g = g;
        }

        public void Update(double mu, double sigma)
        {
            this.g =  Gaussian.MergeGaussian(g, new Gaussian(mu, sigma));
        }

        public void Predict(double mu, double sigma)
        {
            this.g = Gaussian.Add(g, mu, sigma);
        }

        public double Sigma
        { get { return this.g.Sigma; } }

        public double Mu
        { get { return this.g.Mu; } }
    }

    public class Gaussian
    {
        private double sigma;
        private double mu;

        public Gaussian(double mu, double sigma)
        {
            this.sigma = sigma;
            this.mu = mu;
        }

        public double Evaluate(double x)
        {
            return 1.0 / Math.Sqrt(2 * Math.PI * sigma * sigma) * Math.Exp(-0.5 * (x - mu) * (x - mu) / (sigma * sigma));
        }

        public static Gaussian Add(Gaussian g, double mu, double sigma)
        {
            return new Gaussian(g.mu + mu, g.sigma + sigma);
        }

        public static Gaussian MergeGaussian(Gaussian g1, Gaussian g2)
        {
            double sigma = 1.0 / (1.0 / g1.sigma + 1.0 / g2.sigma);
            double mu = (g2.sigma * g1.mu + g1.sigma * g2.mu) / (g1.sigma + g2.sigma);

            return new Gaussian(mu, sigma);
        }

        public double Sigma
        { get { return this.sigma; } }

        public double Mu
        { get { return this.mu; } }
    }
}
