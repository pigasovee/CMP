using System;
using LinearAlgebra;

namespace Mixture
{
    public class MixtureCellParameters
    {
        double[] _pressure;
        double[] _density;
        double[] _totalEnergy;
        Vector[] _velocity;

        public MixtureCellParameters()
        {
        }
    }
}
