using System;
namespace CMP.Grid
{
    public class GridProperties
    {
        private int _cellsNumber;
        private int _facesNumber;
        private int _internalFacesNumber;
        private int _pointsNumber;

        public GridProperties(int cellsNumber, int facesNumber, int internalFacesNumber, int pointsNumber)
        {
            _cellsNumber = cellsNumber;
            _facesNumber = facesNumber;
            _internalFacesNumber = internalFacesNumber;
            _pointsNumber = pointsNumber;
        }

        public int CellsNumber
        {
            get
            {
                return _cellsNumber;
            }
        }

        public int FacesNumber
        {
            get
            {
                return _facesNumber;
            }
        }

        public int InternalFacesNumber
        {
            get
            {
                return _internalFacesNumber;
            }
        }

        public int PointsNumber
        {
            get
            {
                return _pointsNumber;
            }
        }
    }
}
