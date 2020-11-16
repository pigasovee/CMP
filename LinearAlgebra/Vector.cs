using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra
{
    public class Vector
    {
        private int diminsionality_;
        private double[] values_;

        public Vector(int diminsionality)
        {
            diminsionality_ = diminsionality;
            values_ = new double[diminsionality_];
        }

        public Vector(double[] values)
        {
#if WITH_ARGUMENT_CHECKS
            if (values == null)
            {
                throw new ArgumentException("Values can't be null");
            }
#endif
            diminsionality_ = values.Length;
            values_ = new double[diminsionality_];
            Array.Copy(values, values_, diminsionality_);
        }

        public static Vector operator +(Vector a, Vector b)
        {
#if WITH_ARGUMENT_CHECKS
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }
            if (a.Diminsionality != b.Diminsionality)
            {
                throw new ArgumentException("Vectors must have the same length!");
            }
#endif

            double[] result = new double[a.Diminsionality];
            for (int i = 0; i < a.Diminsionality; i++)
            {
                result[i] = a[i] + b[i];
            }

            return new Vector(result);
        }

        public static Vector operator -(Vector a)
        {
            return -1 * a;
        }

        public static Vector operator -(Vector a, Vector b)
        {
#if WITH_ARGUMENT_CHECKS
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }
            if (a.Diminsionality != b.Diminsionality)
            {
                throw new ArgumentException("Vectors must have the same length!");
            }
#endif
            double[] result = new double[a.Diminsionality];
            for (int i = 0; i < a.Diminsionality; i++)
            {
                result[i] = a[i] - b[i];
            }

            return new Vector(result);
        }

        public static Vector operator *(double value, Vector a)
        {
#if WITH_ARGUMENT_CHECKS
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }
#endif
            double[] result = new double[a.Diminsionality];
            for (int i = 0; i < a.Diminsionality; i++)
            {
                result[i] = a[i] * value;
            }

            return new Vector(result);
        }

        public static Vector operator *(Vector a, double value)
        {
#if WITH_ARGUMENT_CHECKS
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }
#endif
            return value * a;
        }

        public static Vector operator /(Vector a, double value)
        {
#if WITH_ARGUMENT_CHECKS
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (value == 0)
            {
                throw new ArgumentException("Division by zero!");
            }
#endif
            return (1.0 / value) * a;
        }

        public Vector CrossProduction(Vector a)
        {
#if WITH_ARGUMENT_CHECKS
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (a.Diminsionality != diminsionality_)
            {
                throw new ArgumentException("Vectors must have the same length!");
            }
#endif
            double[] result = new double[a.Diminsionality];
            if (diminsionality_ == 3)
            {
                result[0] = values_[1] * a.values_[2] - values_[2] * a.values_[1];
                result[1] = values_[2] * a.values_[0] - values_[0] * a.values_[2];
                result[2] = values_[0] * a.values_[1] - values_[1] * a.values_[0];
            }
            else
            {
                throw new NotImplementedException();
                for (int i = 0; i < a.Diminsionality; i++)
                {
                    //TODO: implement this
                    //result += a[i] * b[i];
                }
            }
            return new Vector(result);
        }

        private double LeviChevitaSymbol()
        {
            //TODO: Implement this
            return 0;
        }

        public double DotProduction(Vector a)
        {
#if WITH_ARGUMENT_CHECKS
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }
#endif
            double result = 0.0;
            for (int i = 0;i< a.Diminsionality;i++)
            {
                result += a[i] * values_[i];
            }
            return result;
        }

        public double Magnitude()
        {
            double summ = 0.0;
            for (int i = 0; i < diminsionality_; i++)
            {
                summ += values_[i] * values_[i];
            }
            return Math.Sqrt(summ);
        }

        public double this[int index]
        {
            get
            {
                return values_[index];
            }
            set
            {
                values_[index] = value;
            }
        }

        public int Diminsionality
        {
            get => diminsionality_;
        }

        public double[] Values_
        {
            get => values_;
            set
            {
                values_ = value;
                diminsionality_ = values_.Length;
            }

        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < diminsionality_; i++)
            {
                result += values_[i].ToString() + " ";
            }

            return result;
        }
    }
}
