using System;
using System.Collections.Generic;
using System.IO;
using CMP.Grid;
using LinearAlgebra;

namespace ImportExportTools
{
    public class OpenFOAMGridReader
    {
        private string _meshDirectory;

        public OpenFOAMGridReader(string meshDirectory)
        {
            _meshDirectory = meshDirectory;
            if (!Directory.Exists(_meshDirectory))
            {
                throw new DirectoryNotFoundException($"Mesh directory {_meshDirectory} not found");
            }
        }

        public string MeshDirectory
        {
            get
            {
                return _meshDirectory;
            }
        }

        public int[] ReadOwnerCells(int facesNumber)
        {
            int[] ownerCells = new int[facesNumber];
            string auxStr = "";

            using (StreamReader reader = new StreamReader(Path.Combine(_meshDirectory, OpenFOAMFileNames.Subdirectory, OpenFOAMFileNames.OwnerFileName)))
            {
                while (!reader.EndOfStream)
                {
                    auxStr = reader.ReadLine();
                    if (auxStr == facesNumber.ToString())
                    {
                        reader.ReadLine();
                        break;
                    }
                }
                for (int i = 0; i < facesNumber; i++)
                {
                    if (!reader.EndOfStream)
                    {
                        auxStr = reader.ReadLine();
                        ownerCells[i] = Convert.ToInt32(auxStr);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return ownerCells;
        }

        public int[] ReadNeighbourCells(int internalFacesNumber)
        {
            int[] neighbourCells = new int[internalFacesNumber];
            string auxStr = "";

            using (StreamReader reader = new StreamReader(Path.Combine(_meshDirectory, OpenFOAMFileNames.Subdirectory, OpenFOAMFileNames.NeighbourFileName)))
            {
                while (!reader.EndOfStream)
                {
                    auxStr = reader.ReadLine();
                    if (auxStr == internalFacesNumber.ToString())
                    {
                        reader.ReadLine();
                        break;
                    }
                }
                for (int i = 0; i < internalFacesNumber; i++)
                {
                    if (!reader.EndOfStream)
                    {
                        auxStr = reader.ReadLine();
                        neighbourCells[i] = Convert.ToInt32(auxStr);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return neighbourCells;
        }

        public Vector[] ReadPoints(int pointsNumber, double xScale = 1, double yScale = 1, double zScale = 1)
        {
            Vector[] points = new Vector[pointsNumber];
            int firstSpaceIndex = 0;
            int secondSpaceIndex = 0;
            int closingBracketIndex = 0;
            string auxStr = "";

            using (StreamReader reader = new StreamReader(Path.Combine(_meshDirectory, OpenFOAMFileNames.Subdirectory, OpenFOAMFileNames.PointsFileName)))
            {
                while (!reader.EndOfStream)
                {
                    auxStr = reader.ReadLine();
                    if (auxStr != pointsNumber.ToString()) continue;
                    reader.ReadLine();
                    break;
                }
                for (int i = 0; i < pointsNumber; i++)
                {
                    if (!reader.EndOfStream)
                    {
                        auxStr = reader.ReadLine();
                        if (auxStr == null) continue;
                        firstSpaceIndex = auxStr.IndexOf(" ", StringComparison.Ordinal);
                        secondSpaceIndex = auxStr.IndexOf(" ", firstSpaceIndex + 1, StringComparison.Ordinal);
                        closingBracketIndex = auxStr.IndexOf(")", StringComparison.Ordinal);
                        //TODO: Thrashhold???
                        points[i] = new Vector(xScale * Double.Parse(auxStr.Substring(1, firstSpaceIndex - 1), System.Globalization.CultureInfo.InvariantCulture), yScale * Double.Parse(auxStr.Substring(firstSpaceIndex, secondSpaceIndex - firstSpaceIndex), System.Globalization.CultureInfo.InvariantCulture), zScale * Double.Parse(auxStr.Substring(secondSpaceIndex, closingBracketIndex - secondSpaceIndex), System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return points;
        }

        public Vector[] ReadFaces(int facesNumber)
        {
            Vector[] faces = new Vector[facesNumber];
            int closingBracketIndex = 0;
            string auxStr = "";

            using (StreamReader reader = new StreamReader(Path.Combine(_meshDirectory, OpenFOAMFileNames.Subdirectory, OpenFOAMFileNames.FacesFileName)))
            {
                while (!reader.EndOfStream)
                {
                    auxStr = reader.ReadLine();
                    if (auxStr == facesNumber.ToString())
                    {
                        reader.ReadLine();
                        break;
                    }
                }
                for (int i = 0; i < facesNumber; i++)
                {
                    if (!reader.EndOfStream)
                    {
                        auxStr = reader.ReadLine();
                        int pointsCount = Convert.ToInt32(auxStr[0] + " ");
                        int[] spaceIndexes = new int[pointsCount - 1];
                        double[] pointNumbers = new double[pointsCount];
                        spaceIndexes[0] = auxStr.IndexOf(" ");

                        for (int j = 1; j < pointsCount - 1; j++)
                        {
                            spaceIndexes[j] = auxStr.IndexOf(" ", spaceIndexes[j - 1] + 1);
                        }

                        closingBracketIndex = auxStr.IndexOf(")");
                        pointNumbers[0] = Convert.ToInt32(auxStr.Substring(2, spaceIndexes[0] - 2));

                        for (int j = 1; j < pointsCount - 1; j++)
                        {
                            pointNumbers[j] = Convert.ToInt32(auxStr.Substring(spaceIndexes[j - 1], spaceIndexes[j] - spaceIndexes[j - 1]));
                        }

                        pointNumbers[pointsCount - 1] = Convert.ToInt32(auxStr.Substring(spaceIndexes[pointsCount - 1 - 1], closingBracketIndex - spaceIndexes[pointsCount - 1 - 1]));
                        faces[i] = new Vector(pointNumbers);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return faces;
        }

        public void ReadPatches(out List<string> patchNames, out List<PatchTypes> patchTypes, out List<int> patchStartFaceNumbers, out List<int> patchNumberOfFaces)
        {
            patchNames = new List<string>();
            patchTypes = new List<PatchTypes>();
            patchStartFaceNumbers = new List<int>();
            patchNumberOfFaces = new List<int>();
            int closingBracketIndex = 0;
            string auxStr = "";
            string auxStr2 = "";
            int patchesNumber = 0;

            using (StreamReader reader = new StreamReader(Path.Combine(_meshDirectory, OpenFOAMFileNames.Subdirectory, OpenFOAMFileNames.BoundaryFileName)))
            {
                while (!reader.EndOfStream)
                {
                    auxStr2 = auxStr;
                    auxStr = reader.ReadLine();
                    if (auxStr == "(")
                    {
                        patchesNumber = Convert.ToInt32(auxStr2);
                        break;
                    }
                }
                for (int i = 0; i < patchesNumber; i++)
                {
                    int semicolonIndex;
                    int valueIndex;
                    patchNames.Add(reader.ReadLine().Replace(" ", ""));
                    if (!reader.EndOfStream)
                    {
                        auxStr = reader.ReadLine();
                        if(auxStr.Contains("{"))
                        {
                            auxStr = reader.ReadLine();
                            while (!auxStr.Contains("}"))
                            {
                                if (!reader.EndOfStream)
                                {
                                    if(auxStr.Contains("type"))
                                    {
                                        semicolonIndex = auxStr.IndexOf(";");
                                        valueIndex = auxStr.LastIndexOf(" ", StringComparison.Ordinal) + 1;
                                        string pt = auxStr.Substring(valueIndex, semicolonIndex - valueIndex);
                                        patchTypes.Add((PatchTypes)Enum.Parse(typeof(PatchTypes), pt));
                                    }

                                    if(auxStr.Contains("nFaces"))
                                    {
                                        semicolonIndex = auxStr.IndexOf(";");
                                        valueIndex = auxStr.LastIndexOf(" ", StringComparison.Ordinal) + 1;
                                        patchNumberOfFaces.Add(Convert.ToInt32(auxStr.Substring(valueIndex, semicolonIndex - valueIndex)));
                                    }

                                    if (auxStr.Contains("startFace"))
                                    {
                                        semicolonIndex = auxStr.IndexOf(";");
                                        valueIndex = auxStr.LastIndexOf(" ", StringComparison.Ordinal) + 1;
                                        patchStartFaceNumbers.Add(Convert.ToInt32(auxStr.Substring(valueIndex, semicolonIndex - valueIndex)));
                                    }

                                    auxStr = reader.ReadLine();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public GridProperties ReadGridProperties()
        {
            byte[] ar = new byte[5];
            int pointsIndex = 0;
            int cellsIndex = 0;
            int facesIndex = 0;
            int internalFacesIndex = 0;
            int pointsNumber = 0;
            int cellsNumber = 0;
            int facesNumber = 0;
            int internalFacesNumber = 0;
            string auxStr = "";
            using (StreamReader reader=new StreamReader(Path.Combine(_meshDirectory, OpenFOAMFileNames.Subdirectory, OpenFOAMFileNames.OwnerFileName)))
            {
                while (!reader.EndOfStream)
                {
                    auxStr = reader.ReadLine();
                    if (auxStr.Contains("note"))
                    {
                        int semicolonIndex = auxStr.IndexOf("\";");
                        pointsIndex = auxStr.IndexOf("nPoints");
                        cellsIndex = auxStr.IndexOf("nCells");
                        facesIndex = auxStr.IndexOf("nFaces");
                        internalFacesIndex = auxStr.IndexOf("nInternalFaces");
                        pointsNumber = Convert.ToInt32(auxStr.Substring(pointsIndex + "nPoints".Length + 1, cellsIndex - (pointsIndex + "nPoints".Length + 1)));
                        cellsNumber = Convert.ToInt32(auxStr.Substring(cellsIndex + "nCells".Length + 1, facesIndex - (cellsIndex + "nCells".Length + 1)));
                        facesNumber = Convert.ToInt32(auxStr.Substring(facesIndex + "nFaces".Length + 1, internalFacesIndex - (facesIndex + "nFaces".Length + 1)));
                        internalFacesNumber = Convert.ToInt32(auxStr.Substring(internalFacesIndex + "nInternalFaces".Length + 1, semicolonIndex - (internalFacesIndex + "nInternalFaces".Length + 1)));
                        break;
                    }
                }
            }
            return new GridProperties(cellsNumber, facesNumber, internalFacesNumber, pointsNumber);
        }

        public Grid ReadGrid(double xScale=1, double yScale=1, double zScale=1)
        {
            GridProperties gridProperties = ReadGridProperties();
            int cellNumber = gridProperties.CellsNumber;
            int[] ownerCells = ReadOwnerCells(gridProperties.FacesNumber);
            int[] neighbourCells = ReadNeighbourCells(gridProperties.InternalFacesNumber);
            Vector[] points = ReadPoints(gridProperties.PointsNumber, xScale, yScale, zScale);
            Vector[] faces = ReadFaces(gridProperties.FacesNumber);

            return new Grid(gridProperties, ownerCells, neighbourCells, points, faces);
        }
    }
}
