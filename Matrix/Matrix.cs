using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    class Matrix
    {
        int xDim;
        int yDim;
        double[,] values;

        public Matrix(double[,] values)
        {
            this.xDim = values.GetLength(0);
            this.yDim = values.GetLength(1);
            this.values = values;
        }

        public Matrix Zero(int dimX, int dimY)
        {
            double[,] v = new double[dimX, dimY];
            return new Matrix(v);
        }
    
        public Matrix Identity(int dimension)
        {
            double[,] v = new double[dimension, dimension];

            for (int i = 0; i < dimension; i++)
                v[i, i] = 1;

            return new Matrix(v);
        }

        public Matrix Add(Matrix a, Matrix b)
        {
            double[,] v = new double[a.xDim, a.yDim];
            for (int i = 0; i < a.xDim; i++)
                for (int j = 0; j < a.yDim; j++)
                    v[i, j] = a[i, j] + b[i, j];

            return new Matrix(v);
        }

        public Matrix Subtract(Matrix a, Matrix b)
        {
            double[,] v = new double[a.xDim, a.yDim];
            for (int i = 0; i < a.xDim; i++)
                for (int j = 0; j < a.yDim; j++)
                    v[i, j] = a[i, j] - b[i, j];

            return new Matrix(v);
        }

        public Matrix Multiply(Matrix a, Matrix b)
        {
            double[,] v = new double[a.xDim, b.yDim];
            for (int i = 0; i < a.xDim; i++)
                for (int j = 0; j < b.yDim; j++)
                    for (int k = 0; k < b.yDim; k++)
                        v[i,j] += a[i,k] * b[k,j];

            return new Matrix(v);
            /*
                for j in range(other.dimy):
                    for k in range(self.dimy):
                        res.value[i][j] += self.value[i][k] * other.value[k][j]
             * */
        }

        public Matrix Transpose(Matrix a)
        {
            double[,] v = new double[a.yDim, a.xDim];
            for (int i = 0; i < a.xDim; i++)
                for (int j = 0; j < a.yDim; j++)
                    v[j, i] = a[i, j];

            return new Matrix(v);
        }

        private double[,] Cholesky(Matrix a)
        {
            //     def Cholesky(self, ztol=1.0e-5):
            //# Computes the upper triangular Cholesky factorization of
            //# a positive definite matrix.
            //res = matrix([[]])
            //res.zero(self.dimx, self.dimx)
            double[,] v = new double[a.xDim, a.yDim];

            for (int i = 0; i < a.xDim; i++)
            {
                /*
                 *  for i in range(self.dimx):
            S = sum([(res.value[k][i])**2 for k in range(i)])
            d = self.value[i][i] - S
            if abs(d) < ztol:
                res.value[i][i] = 0.0
            else:
                if d < 0.0:
                    raise ValueError, "Matrix not positive-definite"
                res.value[i][i] = sqrt(d)
                 * */
                double sum = 0;
                for (int k = 0; k < i; k++)
                    sum += a[k, i];

                double d = a[i, i] - sum;
                if (Math.Abs(d) < 0.00001)
                    v[i, i] = 0;
                else
                {
                    if (d < 0)
                        throw new Exception("Matrix not positive-definite");
                    v[i, i] = Math.Sqrt(d);
                }

                for (int j = i + 1; j < a.xDim; j++)
                {
                    double sum2 = 0;
                    for (int k = 0; k < a.xDim; k++)
                    {
                        sum2 += v[k, i] * v[k, j];
                    }
                    if (sum2 < 0.0001)
                        sum2 = 0;
                    v[i, j] = (a[i, j] - sum2) / v[i, i];
                }
            }
                return v;
                /*
                     for j in range(i+1, self.dimx):
                         S = sum([res.value[k][i] * res.value[k][j] for k in range(self.dimx)])
                         if abs(S) < ztol:
                             S = 0.0
                         res.value[i][j] = (self.value[i][j] - S)/res.value[i][i]
                 return res
                 * */
            
        }

        private double[,] CholeskyInverse(Matrix a)
        {
            /*  def CholeskyInverse(self):
          # Computes inverse of matrix given its Cholesky upper Triangular
          # decomposition of matrix.
          res = matrix([[]])
          res.zero(self.dimx, self.dimx)
             * */
            double[,] v = new double[a.xDim, a.xDim];

            for (int j = a.xDim; j > 0; j--)
            {
                double val = a[j, j];

                double sum = 0;

                for (int k = j + 1; k < a.xDim; k++)
                    sum += a[j, k] * v[j, k];

                v[j, j] = 1.0 / (val * val) - sum / val;

                for (int i = j; i > 0; i--)
                {
                    double sum2 = 0;

                    for (int k = i + 1; k < a.xDim; k++)
                    {
                        sum2 -= (a[i, k] * v[k, j]) / a[i, i];
                    }
                    v[i, j] = sum2;
                    v[j, i] = v[i, j];
                }
            }
                /*
        # Backward step for inverse.
        for j in reversed(range(self.dimx)):
            tjj = self.value[j][j]
            S = sum([self.value[j][k]*res.value[j][k] for k in range(j+1, self.dimx)])
            res.value[j][j] = 1.0/tjj**2 - S/tjj
            for i in reversed(range(j)):
                res.value[j][i] = res.value[i][j] = -sum([self.value[i][k]*res.value[k][j] for k in range(i+1, self.dimx)])/self.value[i][i]
        
                 * */
            return v;
        }

        public Matrix Inverse(Matrix a)
        {
            double[,] v;// = a.Cholesky(a);
            Matrix m = new Matrix(a.Cholesky(a));
            v = CholeskyInverse(m);

            return new Matrix(v);
        }

        public double this[int i, int j]
        {get { return this.values[i,j]; }}
    }
  /*  
    
    # Thanks to Ernesto P. Adorio for use of Cholesky and CholeskyInverse functions
    
    def Cholesky(self, ztol=1.0e-5):
        # Computes the upper triangular Cholesky factorization of
        # a positive definite matrix.
        res = matrix([[]])
        res.zero(self.dimx, self.dimx)
        
        for i in range(self.dimx):
            S = sum([(res.value[k][i])**2 for k in range(i)])
            d = self.value[i][i] - S
            if abs(d) < ztol:
                res.value[i][i] = 0.0
            else:
                if d < 0.0:
                    raise ValueError, "Matrix not positive-definite"
                res.value[i][i] = sqrt(d)
            for j in range(i+1, self.dimx):
                S = sum([res.value[k][i] * res.value[k][j] for k in range(self.dimx)])
                if abs(S) < ztol:
                    S = 0.0
                res.value[i][j] = (self.value[i][j] - S)/res.value[i][i]
        return res
    
    def CholeskyInverse(self):
        # Computes inverse of matrix given its Cholesky upper Triangular
        # decomposition of matrix.
        res = matrix([[]])
        res.zero(self.dimx, self.dimx)
        
        # Backward step for inverse.
        for j in reversed(range(self.dimx)):
            tjj = self.value[j][j]
            S = sum([self.value[j][k]*res.value[j][k] for k in range(j+1, self.dimx)])
            res.value[j][j] = 1.0/tjj**2 - S/tjj
            for i in reversed(range(j)):
                res.value[j][i] = res.value[i][j] = -sum([self.value[i][k]*res.value[k][j] for k in range(i+1, self.dimx)])/self.value[i][i]
        return res
    
    def inverse(self):
        aux = self.Cholesky()
        res = aux.CholeskyInverse()
        return res
    
    def __repr__(self):
        return repr(self.value)
   * */
}
