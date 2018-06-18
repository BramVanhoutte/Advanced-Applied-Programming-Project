using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Globals
{
    public class Node
    {
        public int x1;
        public int y1;
        public int x2;
        public int y2;

        public Point corner1;
        public Point corner2;
        public Point corner3;
        public Point corner4;

        public int highestX;
        public int highestY;
        public int lowestX;
        public int lowestY;

        public double nodeX;
        public double nodeY;

        public double xWidth;
        public double yWidth;

        public GeometryModel3D node3D;

        public bool Visited { get; private set; }
        public bool Path { get; private set; }
        public bool End { get; private set; }

        public Node(int x1, int y1, int x2, int y2)
        {
            Visited = false;
            Path = false;
            End = false;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            CalculateCoordinates();
        }

        private void CalculateCoordinates()
        {
            corner1 = new Point(x1, y2);
            corner2 = new Point(x1, y2);
            corner3 = new Point(x2, y1);
            corner3 = new Point(x2, y2);


            xWidth = Math.Abs(x1 - x2);
            yWidth = Math.Abs(y1 - y2);
            highestX = x1 > x2 ? x1 : x2;
            highestY = y1 > y2 ? y1 : y2;
            lowestX = x1 < x2 ? x1 : x2;
            lowestY = y1 < y2 ? y1 : y2;

            nodeY = lowestY + yWidth / 2.0;
            nodeX = lowestX + xWidth / 2.0;
        }

        public void CreateShape()
        {
            double height = 1;
            if (this.End && this.Path)
            {
                height = 1.1;
            }
            else if (this.Path)
            {
                height = 0.9;
            }
            GeometryModel3D shape = new GeometryModel3D();
            MeshGeometry3D mesh = new MeshGeometry3D();
            Point3DCollection corners = new Point3DCollection
            {
                new Point3D(x1, y1, 0),
                new Point3D(x2, y1, 0),
                new Point3D(x1, y2, 0),
                new Point3D(x2, y2, 0),
                new Point3D(x1, y1, height),
                new Point3D(x2, y1, height),
                new Point3D(x1, y2, height),
                new Point3D(x2, y2, height)
            };
            mesh.Positions = corners;
            mesh.TriangleIndices = new Int32Collection(new int[] { 6, 5, 7,  6, 4, 5 });
            shape.Geometry = mesh;
            MaterialGroup materialGroup = new MaterialGroup();
            if (this.End && this.Path)
            {
                materialGroup.Children.Add(new DiffuseMaterial(new SolidColorBrush(System.Windows.Media.Color.FromRgb(249, 247, 107))));
            }
            else if (this.Path)
            {
                materialGroup.Children.Add(new DiffuseMaterial(new SolidColorBrush(System.Windows.Media.Color.FromRgb(144, 238, 144))));
            }
            else
            {
                materialGroup.Children.Add(new DiffuseMaterial(new SolidColorBrush(System.Windows.Media.Color.FromRgb(211, 211, 211))));
            }
            shape.Material = materialGroup;

            node3D = shape;
        }

        public void VisitedNode()
        {
            Visited = true;
        }

        public void PathNode()
        {
            Path = true;
        }

        public void EndNode()
        {
            End = true;
        }
    }
}
