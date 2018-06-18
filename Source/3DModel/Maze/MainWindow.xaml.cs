using Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Logic;


namespace Maze
{
    public partial class MainWindow : Window
    {

        private Physics physics = new Physics();
        private MazeGenerator maze = new MazeGenerator();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            foreach (Node node in maze.nodeList)
            {
                mazeItems.Children.Add(node.node3D);
            }

            foreach (Wall wall in maze.wallList)
            {
                mazeItems.Children.Add(wall.wall3D);
            }

            SetTimers();
        }

        private void SetTimers()
        {
            DispatcherTimer logicRefreshTimer = new DispatcherTimer();
            logicRefreshTimer.Tick += new EventHandler(UpdateGameLogic);
            logicRefreshTimer.Interval = new TimeSpan(0, 0, 0, 0, Physics.REFRESHRATE);
            logicRefreshTimer.Start();

            DispatcherTimer graphicsRefreshTimer = new DispatcherTimer();
            graphicsRefreshTimer.Tick += new EventHandler(UpdateGraphics);
            graphicsRefreshTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / Physics.FPS);
            graphicsRefreshTimer.Start();
        }

        private void UpdateGameLogic(object sender, EventArgs e)
        {
            double offsetX = 0;
            double offsetY = 0;

            DetectUserInput();
            physics.CalculateBallMovement(ref offsetX, ref offsetY);
            physics.DetermineCollission(ref offsetX, ref offsetY, ref ball, ref movementBall, maze.wallList);
            movementBall.OffsetY += offsetY;
            movementBall.OffsetX += offsetX;
        }

        private void DetectUserInput()
        {
            if (Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W))
            {
                if (physics.xRotation > -10)
                {
                    physics.xRotation -= 0.5;
                }
            }
            if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.S))
            {
                if (physics.xRotation < 10)
                {
                    physics.xRotation += 0.5;
                }
            }
            if (Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D))
            {
                if (physics.yRotation < 10)
                {
                    physics.yRotation += 0.5;
                }
            }
            if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A))
            {
                if (physics.yRotation > -10)
                {
                    physics.yRotation -= 0.5;
                }
            }
        }

        private void UpdateGraphics(object sender, EventArgs e)
        {
            string arrow = "";
            if(physics.xRotation < 0)
            {
                arrow = char.ConvertFromUtf32(0x2191);
            }
            else if(physics.xRotation > 0)
            {
                arrow = char.ConvertFromUtf32(0x2193);
            }
            labelXangle.Content = "XRotation angle: " + physics.xRotation + "\t" + arrow;
            arrow = "";
            if (physics.yRotation < 0)
            {
                arrow = char.ConvertFromUtf32(0x2190);
            }
            else if (physics.yRotation > 0)
            {
                arrow = char.ConvertFromUtf32(0x2192);
            }
            labelYangle.Content = "YRotation angle: " + physics.yRotation + "\t" + arrow;
            UpdateAngle();
        }

        private void UpdateAngle()
        {
            horizontalRotation.Angle = physics.xRotation;
            verticalRotation.Angle = physics.yRotation;
        }
    }
}
