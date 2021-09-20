using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace CMP.Grid
{
    public class Grid
    {
        private static Cell[] _cells;
        private static CellFace[] _faces;

        public Grid(GridProperties gridProperties, int[] ownerCells, int[] neighbourCells, Vector[] points, Vector[] faces)
        {

        }

        public static Cell[] Cells
        {
            get { return _cells; }
        }

        public static CellFace[] Faces
        {
            get { return _faces; }
        }
    }
}
