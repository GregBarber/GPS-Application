using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{

/// TODO: create filter to reduce track data evaluating entopy?


    public class Filter
    {
        private double Gaussian(double mu, double sigma, double x)
        {
            return 1.0 / Math.Sqrt(2 * Math.PI * sigma * sigma) * Math.Exp(-0.5 * (x - mu) * (x - mu) / (sigma * sigma));
        }
    }

    
}
