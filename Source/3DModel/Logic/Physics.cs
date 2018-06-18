using Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Logic
{
    public class Physics
    {
        public const int FPS = 100;

        public const int REFRESHRATE = 10;

        public const double DISTANCECONVERSION = 50;
        //32.6086956522

        public const double GRAVITATIONAL = 9.81;

        public const double BOUNCE = 2.5;

        public double yRotation = 0;

        public double xRotation = 0;

        private double speedY = 0;

        private double speedX = 0;

        public void CalculateBallMovement(ref double offsetX, ref double offsetY)
        {
            double aX = (GRAVITATIONAL * Math.Sin(yRotation * Math.PI / 180)) / 1.5;
            double aY = (GRAVITATIONAL * Math.Sin(-xRotation * Math.PI / 180)) / 1.5;


            offsetX = DISTANCECONVERSION * (speedX * (REFRESHRATE / 1000.0) + aX * Math.Pow((REFRESHRATE / 1000.0), 2) / 2);
            offsetY = DISTANCECONVERSION * (speedY * (REFRESHRATE / 1000.0) + aY * Math.Pow((REFRESHRATE / 1000.0), 2) / 2);
            speedY = speedY + aY * (REFRESHRATE / 1000.0);
            speedX = speedX + aX * (REFRESHRATE / 1000.0);
        }

        public void DetermineCollission(ref double offsetX, ref double offsetY, ref GeometryModel3D ball, ref TranslateTransform3D movementBall, List<Wall> wallList)
        {
            double radius = ball.Bounds.SizeX / 2;
            double futureOffsetX = movementBall.OffsetX + offsetX;
            double futureOffsetY = movementBall.OffsetY + offsetY;

            foreach (Wall wall in wallList)
            {
                double distanceToWallY = Math.Abs(wall.wallY - futureOffsetY);
                double distanceToWallX = Math.Abs(wall.wallX - futureOffsetX);
                if (distanceToWallX - radius <= (wall.xWidth / 2) && distanceToWallY - radius <= (wall.yWidth / 2))
                {
                    if (distanceToWallX <= wall.xWidth / 2)
                    {
                        speedY = -1 * (speedY / BOUNCE);
                        offsetY = 0;
                    }
                    else if (distanceToWallY <= wall.yWidth / 2)
                    {
                        speedX = -1 * (speedX / BOUNCE);
                        offsetX = 0;
                    }
                    else
                    {
                        //Corner detection
                    }
                }
            }
        }


    }
}
