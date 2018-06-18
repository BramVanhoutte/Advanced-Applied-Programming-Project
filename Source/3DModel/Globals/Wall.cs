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
    public class Wall
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

        public double wallX;
        public double wallY;

        public double xWidth;
        public double yWidth;

        public GeometryModel3D wall3D;

        public Wall(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            CalculateCoordinates();
            CreateShape();
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

            wallY = lowestY + yWidth / 2.0;
            wallX = lowestX + xWidth / 2.0;
        }

        private void CreateShape()
        {
            GeometryModel3D shape = new GeometryModel3D();
            MeshGeometry3D mesh = new MeshGeometry3D();
            Point3DCollection corners = new Point3DCollection();
            corners.Add(new Point3D(x1, y1, 1));
            corners.Add(new Point3D(x2, y1, 1));
            corners.Add(new Point3D(x1, y1, 3));
            corners.Add(new Point3D(x2, y1, 3));
            corners.Add(new Point3D(x1, y2, 1));
            corners.Add(new Point3D(x2, y2, 1));
            corners.Add(new Point3D(x1, y2, 3));
            corners.Add(new Point3D(x2, y2, 3));
            mesh.Positions = corners;
            mesh.TriangleIndices = new Int32Collection(new int[] { 0, 1, 2, 3, 2, 1, 1, 5, 3, 5, 7, 3, 7, 5, 4, 4, 6, 7, 6, 2, 3, 3, 7, 6, 0, 4, 1, 1, 4, 5, 0, 2, 4, 2, 6, 4 });
            shape.Geometry = mesh;
            MaterialGroup materialGroup = new MaterialGroup();
            materialGroup.Children.Add(new DiffuseMaterial(new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 75, 150))));
            shape.Material = materialGroup;

            wall3D = shape;
        }
    }
}
