using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra
{
    public class Vector
    {
        private int _diminsionality;
        private double[] _values;

        public Vector(int diminsionality)
        {
            _diminsionality = diminsionality;
            _values = new double[_diminsionality];
        }

        public Vector(double x, double y, double z)
        {
            _diminsionality = 3;
            _values = new double[3] { x, y, z };
        }

        public Vector(double[] values)
        {
#if WITH_ARGUMENT_CHECKS
            if (values == null)
            {
                throw new ArgumentException("Values can't be null");
            }
#endif
            _diminsionality = values.Length;
            _values = new double[_diminsionality];
            Array.Copy(values, _values, _diminsionality);
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
            if (_diminsionality == 3)
            {
                result[0] = _values[1] * a._values[2] - _values[2] * a._values[1];
                result[1] = _values[2] * a._values[0] - _values[0] * a._values[2];
                result[2] = _values[0] * a._values[1] - _values[1] * a._values[0];
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
                result += a[i] * _values[i];
            }
            return result;
        }

        public double Magnitude()
        {
            double summ = 0.0;
            for (int i = 0; i < _diminsionality; i++)
            {
                summ += _values[i] * _values[i];
            }
            return Math.Sqrt(summ);
        }

        public double this[int index]
        {
            get
            {
                return _values[index];
            }
            set
            {
                _values[index] = value;
            }
        }

        public int Diminsionality
        {
            get => _diminsionality;
        }

        public double[] Values_
        {
            get => _values;
            set
            {
                _values = value;
                _diminsionality = _values.Length;
            }

        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < _diminsionality; i++)
            {
                result += _values[i].ToString() + " ";
            }

            return result;
        }
    }
}
