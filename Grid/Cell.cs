using System;
using LinearAlgebra;

namespace CMP.Grid
{
    public class Cell
    {
        private Vector _center;
        private double _volume;
        private CellFace[] _faces;

        public Cell()
        {
        }
    }
}
