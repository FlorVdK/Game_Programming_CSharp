using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Topic01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static double cubeBaseHeight = 2;
        public static double cubeBaseWidth = 4;
        public static double cubeBaseTop = cubeBaseWidth + cubeBaseHeight;
        public static double baseCubeHeight = 2;
        public static double baseCubeBase = cubeBaseHeight - baseCubeHeight;
        public static double stairHeigth = 0.2;
        public static double stairwidth = 0.5;
        public static double headStoneHeigth = 1;
        public static double headStoneWidth = cubeBaseWidth + 1;
        public static double headStoneTop = cubeBaseTop + headStoneHeigth;
        public static double pillarBottomWidth = 4 * cubeBaseWidth / 6;
        public static double pillarTopWidth = headStoneWidth/4;
        public static double pillarHeigth = cubeBaseWidth * 8;
        public static double pillarTop = headStoneTop + pillarHeigth;
        public static double piramideBottomWidth = pillarTopWidth;
        public static double piramideHeight = piramideBottomWidth;
        public static double piramideTop = pillarTop + piramideHeight;
        Storyboard RotCube;
        PerspectiveCamera Camera1;
        double CameraR, CameraPhi, CameraTheta; 


        MeshGeometry3D MCube()
        {
            MeshGeometry3D cube = new MeshGeometry3D();
            Point3DCollection corners = new
                                   Point3DCollection();
            corners.Add(new Point3D(cubeBaseWidth , cubeBaseTop, cubeBaseWidth));
            corners.Add(new Point3D(cubeBaseWidth * 0.75, cubeBaseTop, cubeBaseWidth));
            corners.Add(new Point3D(cubeBaseWidth * 0.75, cubeBaseHeight, cubeBaseWidth));
            corners.Add(new Point3D(cubeBaseWidth, cubeBaseHeight, cubeBaseWidth));
            corners.Add(new Point3D(-cubeBaseWidth * 0.75, cubeBaseTop, cubeBaseWidth));
            corners.Add(new Point3D(-cubeBaseWidth, cubeBaseHeight + cubeBaseWidth, cubeBaseWidth));
            corners.Add(new Point3D(-cubeBaseWidth, cubeBaseHeight, cubeBaseWidth));
            corners.Add(new Point3D(-cubeBaseWidth * 0.75, cubeBaseHeight, cubeBaseWidth));
            corners.Add(new Point3D(cubeBaseWidth * 0.75, cubeBaseTop, cubeBaseWidth * 0.75));
            corners.Add(new Point3D(-cubeBaseWidth * 0.75, cubeBaseTop, cubeBaseWidth * 0.75));
            corners.Add(new Point3D(-cubeBaseWidth * 0.75, cubeBaseHeight, cubeBaseWidth * 0.75));
            corners.Add(new Point3D(cubeBaseWidth * 0.75, cubeBaseHeight, cubeBaseWidth * 0.75));
            corners.Add(new Point3D(cubeBaseWidth, cubeBaseTop, -cubeBaseWidth));
            corners.Add(new Point3D(-cubeBaseWidth, cubeBaseTop, -cubeBaseWidth));
            corners.Add(new Point3D(-cubeBaseWidth, cubeBaseHeight, -cubeBaseWidth));
            corners.Add(new Point3D(cubeBaseWidth , cubeBaseHeight, -cubeBaseWidth ));
            cube.Positions = corners;

            Int32[] indices ={
            //front right
                0,1,2,
                0,2,3,
            //front left
                4,5,6,
                4,6,7,
            //back
                12,13,14,
                12,14,15,
              //Right
                0,12,15,
                0,15,3,
              //Left
                 5,6,14,
                 5,14,13,
              //cut deep 
                 8,9,10,
                 8,10,11,
              //cut right
                1,2,11,
                1,8,11,
              //cut left
                 4,7,10,
                 4,9,10

              };

            Int32Collection Triangles = new Int32Collection();
            foreach (Int32 index in indices)
            {
                Triangles.Add(index);
            }
            cube.TriangleIndices = Triangles;

            //adding TextureCoordinates

            PointCollection woodpoints = new PointCollection();
            woodpoints = getwoodpoints();
            cube.TextureCoordinates = woodpoints;

            return cube;
        }

        MeshGeometry3D MStairs(int number)
        {
            MeshGeometry3D stairs = new MeshGeometry3D();
            Point3DCollection corners = new Point3DCollection();
            for (int i = 0; i<number; i++)
            {
                corners.Add(new Point3D(headStoneWidth + stairwidth * (i + 1), baseCubeBase - stairHeigth * i, headStoneWidth + stairwidth * (i + 1)));
                corners.Add(new Point3D(-headStoneWidth - stairwidth * (i + 1), baseCubeBase - stairHeigth * i, headStoneWidth + stairwidth * (i + 1)));
                corners.Add(new Point3D(-headStoneWidth - stairwidth * (i + 1), baseCubeBase - stairHeigth * (i + 1), headStoneWidth + stairwidth * (i + 1)));
                corners.Add(new Point3D(headStoneWidth + stairwidth * (i + 1), baseCubeBase - stairHeigth * (i + 1), headStoneWidth + stairwidth * (i + 1)));
                corners.Add(new Point3D(headStoneWidth + stairwidth * (i + 1), baseCubeBase - stairHeigth * i, -headStoneWidth - stairwidth * (i + 1)));
                corners.Add(new Point3D(-headStoneWidth - stairwidth * (i + 1), baseCubeBase - stairHeigth * i, -headStoneWidth - stairwidth * (i + 1)));
                corners.Add(new Point3D(-headStoneWidth - stairwidth * (i + 1), baseCubeBase - stairHeigth * (i + 1), -headStoneWidth - stairwidth * (i + 1)));
                corners.Add(new Point3D(headStoneWidth + stairwidth * (i + 1), baseCubeBase - stairHeigth * (i + 1), -headStoneWidth - stairwidth * (i + 1)));
            }
            stairs.Positions = corners;

            Int32[] indices ={
            //front
                0,1,2,
                0,2,3,
            //back
               4,7,6,
                4,6,5,
              //Right
                4,0,3,
                4,3,7,
              //Left
                1,5,6,
                 1,6,2,
              //Top
                 1,0,4,
                 1,4,5,
              //Bottom
                 2,6,7,
                 2,7,3
              };

            Int32Collection Triangles = new Int32Collection();
            for (int i = 0; i < number; i++)
            {
                foreach (Int32 index in indices)
                {
                    Triangles.Add(index + i * 8);
                }
            }
            stairs.TriangleIndices = Triangles;

            //adding TextureCoordinates

            PointCollection woodpoints = new PointCollection();
            woodpoints = getwoodpoints();
            stairs.TextureCoordinates = woodpoints;

            return stairs;
        }

        MeshGeometry3D MHeadstone()
        {
            MeshGeometry3D headStone = new MeshGeometry3D();
            Point3DCollection corners = new Point3DCollection();

            corners.Add(new Point3D(headStoneWidth, headStoneTop, headStoneWidth));
            corners.Add(new Point3D(-headStoneWidth, headStoneTop, headStoneWidth));
            corners.Add(new Point3D(-cubeBaseWidth , cubeBaseTop, cubeBaseWidth));
            corners.Add(new Point3D(cubeBaseWidth , cubeBaseTop, cubeBaseWidth ));
            corners.Add(new Point3D(headStoneWidth, headStoneTop, -headStoneWidth));
            corners.Add(new Point3D(-headStoneWidth, headStoneTop, -headStoneWidth));
            corners.Add(new Point3D(-cubeBaseWidth , cubeBaseTop, -cubeBaseWidth ));
            corners.Add(new Point3D(cubeBaseWidth , cubeBaseTop, -cubeBaseWidth ));
            Console.WriteLine(cubeBaseWidth + headStoneHeigth);

            headStone.Positions = corners;

            Int32[] indices ={
            //front
                0,1,2,
                0,2,3,
            //back
               4,7,6,
                4,6,5,
              //Right
                4,0,3,
                4,3,7,
              //Left
                1,5,6,
                 1,6,2,
              //Top
                 1,0,4,
                 1,4,5,
              //Bottom
                 2,6,7,
                 2,7,3
              };

            Int32Collection Triangles = new Int32Collection();

            foreach (Int32 index in indices)
            {
                Triangles.Add(index);
            }

            headStone.TriangleIndices = Triangles;

            //adding TextureCoordinates

            PointCollection woodpoints = new PointCollection();
            woodpoints = getwoodpoints();
            headStone.TextureCoordinates = woodpoints;

            return headStone;
        }

        MeshGeometry3D MBaseCube()
        {
            MeshGeometry3D baseCube = new MeshGeometry3D();
            Point3DCollection corners = new Point3DCollection();

            corners.Add(new Point3D(headStoneWidth - 0.5, baseCubeHeight, headStoneWidth - 0.5));
            corners.Add(new Point3D(-headStoneWidth + 0.5, baseCubeHeight, headStoneWidth - 0.5));
            corners.Add(new Point3D(-headStoneWidth, baseCubeBase, headStoneWidth));
            corners.Add(new Point3D(headStoneWidth, baseCubeBase, headStoneWidth));
            corners.Add(new Point3D(headStoneWidth - 0.5, baseCubeHeight, -headStoneWidth + 0.5));
            corners.Add(new Point3D(-headStoneWidth + 0.5, baseCubeHeight, -headStoneWidth + 0.5));
            corners.Add(new Point3D(-headStoneWidth, baseCubeBase, -headStoneWidth));
            corners.Add(new Point3D(headStoneWidth, baseCubeBase, -headStoneWidth));
            Console.WriteLine(cubeBaseWidth + headStoneHeigth);

            baseCube.Positions = corners;

            Int32[] indices ={
            //front
                0,1,2,
                0,2,3,
            //back
               4,7,6,
                4,6,5,
              //Right
                4,0,3,
                4,3,7,
              //Left
                1,5,6,
                 1,6,2,
              //Top
                 1,0,4,
                 1,4,5,
              //Bottom
                 2,6,7,
                 2,7,3
              };

            Int32Collection Triangles = new Int32Collection();

            foreach (Int32 index in indices)
            {
                Triangles.Add(index);
            }

            baseCube.TriangleIndices = Triangles;

            //adding TextureCoordinates

            PointCollection woodpoints = new PointCollection();
            woodpoints = getwoodpoints();
            baseCube.TextureCoordinates = woodpoints;

            return baseCube;
        }

        MeshGeometry3D MPillar()
        {
            MeshGeometry3D pillar = new MeshGeometry3D();
            Point3DCollection corners = new Point3DCollection();

            corners.Add(new Point3D(pillarTopWidth, pillarTop, pillarTopWidth));
            corners.Add(new Point3D(-pillarTopWidth, pillarTop, pillarTopWidth));
            corners.Add(new Point3D(-pillarBottomWidth, headStoneTop, pillarBottomWidth));
            corners.Add(new Point3D(pillarBottomWidth, headStoneTop, pillarBottomWidth));
            corners.Add(new Point3D(pillarTopWidth, pillarTop, -pillarTopWidth));
            corners.Add(new Point3D(-pillarTopWidth, pillarTop, -pillarTopWidth));
            corners.Add(new Point3D(-pillarBottomWidth, headStoneTop, -pillarBottomWidth));
            corners.Add(new Point3D(pillarBottomWidth, headStoneTop, -pillarBottomWidth));
            Console.WriteLine(cubeBaseWidth + headStoneHeigth);

            pillar.Positions = corners;

            Int32[] indices ={
            //front
                0,1,2,
                0,2,3,
            //back
               4,7,6,
                4,6,5,
              //Right
                4,0,3,
                4,3,7,
              //Left
                1,5,6,
                 1,6,2,
              //Top
                 1,0,4,
                 1,4,5,
              //Bottom
                 2,6,7,
                 2,7,3
              };

            Int32Collection Triangles = new Int32Collection();

            foreach (Int32 index in indices)
            {
                Triangles.Add(index);
            }

            pillar.TriangleIndices = Triangles;

            //adding TextureCoordinates

            PointCollection woodpoints = new PointCollection();
            woodpoints = getwoodpoints();
            pillar.TextureCoordinates = woodpoints;

            return pillar;
        }

        MeshGeometry3D MPiramide()
        {
            MeshGeometry3D pillar = new MeshGeometry3D();
            Point3DCollection corners = new Point3DCollection();

            corners.Add(new Point3D(0, piramideTop, 0));
            corners.Add(new Point3D(-piramideBottomWidth, pillarTop, piramideBottomWidth));
            corners.Add(new Point3D(piramideBottomWidth, pillarTop, piramideBottomWidth));
            corners.Add(new Point3D(-piramideBottomWidth, pillarTop, -piramideBottomWidth));
            corners.Add(new Point3D(piramideBottomWidth, pillarTop, -piramideBottomWidth));
            Console.WriteLine(piramideTop);

            pillar.Positions = corners;

            Int32[] indices ={
            //front
                0,1,2,
                0,2,3,
                0,3,4,
                0,4,5
              };

            Int32Collection Triangles = new Int32Collection();

            foreach (Int32 index in indices)
            {
                Triangles.Add(index);
            }

            pillar.TriangleIndices = Triangles;

            //adding TextureCoordinates

            PointCollection woodpoints = new PointCollection();
            woodpoints = getwoodpoints();
            pillar.TextureCoordinates = woodpoints;

            return pillar;
        }

        MeshGeometry3D MPlacard()
        {
            MeshGeometry3D placard = new MeshGeometry3D();
            Point3DCollection corners = new Point3DCollection();

            corners.Add(new Point3D(cubeBaseWidth * 0.65, cubeBaseHeight + cubeBaseWidth *3/4, cubeBaseWidth * 0.85));
            corners.Add(new Point3D(-cubeBaseWidth * 0.65, cubeBaseHeight + cubeBaseWidth * 3 / 4, cubeBaseWidth * 0.85));
            corners.Add(new Point3D(-cubeBaseWidth * 0.65, cubeBaseHeight + cubeBaseWidth  / 4, cubeBaseWidth * 0.85));
            corners.Add(new Point3D(cubeBaseWidth * 0.65, cubeBaseHeight + cubeBaseWidth  / 4, cubeBaseWidth * 0.85));
            corners.Add(new Point3D(cubeBaseWidth * 0.65, cubeBaseHeight + cubeBaseWidth * 3 / 4, cubeBaseWidth * 0.75));
            corners.Add(new Point3D(-cubeBaseWidth * 0.65, cubeBaseHeight + cubeBaseWidth * 3 / 4, cubeBaseWidth * 0.75));
            corners.Add(new Point3D(-cubeBaseWidth * 0.65, cubeBaseHeight + cubeBaseWidth  / 4, cubeBaseWidth * 0.75));
            corners.Add(new Point3D(cubeBaseWidth * 0.65, cubeBaseHeight + cubeBaseWidth  / 4, cubeBaseWidth * 0.75));
            Console.WriteLine(cubeBaseWidth + headStoneHeigth);

            placard.Positions = corners;

            Int32[] indices ={
            //front
                0,1,2,
                0,2,3,
              //Right
                4,0,3,
                4,3,7,
              //Left
                1,5,6,
                 1,6,2,
              //Top
                 1,0,4,
                 1,4,5,
              //Bottom
                 2,6,7,
                 2,7,3
              };

            Int32Collection Triangles = new Int32Collection();

            foreach (Int32 index in indices)
            {
                Triangles.Add(index);
            }

            placard.TriangleIndices = Triangles;

            //adding TextureCoordinates

            PointCollection woodpoints = new PointCollection();
            woodpoints = getwoodpoints();
            placard.TextureCoordinates = woodpoints;

            return placard;
        }

        private PointCollection getwoodpoints()
        {
            PointCollection woodpoints = new PointCollection();
            woodpoints.Add(new Point(1, 0));
            woodpoints.Add(new Point(0, 0));
            woodpoints.Add(new Point(0, 1));
            woodpoints.Add(new Point(1, 1));

            woodpoints.Add(new Point(1, 0));
            woodpoints.Add(new Point(0, 0));
            woodpoints.Add(new Point(0, 1));
            woodpoints.Add(new Point(1, 1));


            woodpoints.Add(new Point(0, 1));
            woodpoints.Add(new Point(0, 0));
            woodpoints.Add(new Point(1, 0));
            woodpoints.Add(new Point(1, 1));


            woodpoints.Add(new Point(0, 1));
            woodpoints.Add(new Point(0, 0));
            woodpoints.Add(new Point(1, 0));
            woodpoints.Add(new Point(1, 1));


            woodpoints.Add(new Point(1, 0));
            woodpoints.Add(new Point(0, 0));
            woodpoints.Add(new Point(0, 1));
            woodpoints.Add(new Point(1, 1));

            woodpoints.Add(new Point(1, 0));
            woodpoints.Add(new Point(0, 0));
            woodpoints.Add(new Point(0, 1));
            woodpoints.Add(new Point(1, 1));
            return woodpoints;

        }

        private void Window_Loaded(object sender,
                                  RoutedEventArgs e)
        {
            GeometryModel3D Cube1 =new GeometryModel3D();
            MeshGeometry3D cubeMesh = MCube();
            Cube1.Geometry = cubeMesh;

            //Cube1.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Azure));
            //Cube1.BackMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Azure));

            GeometryModel3D stairs1 =new GeometryModel3D();
            MeshGeometry3D stairMesh = MStairs(10);
            stairs1.Geometry = stairMesh;

            //stairs1.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Azure));

            GeometryModel3D headStone =new GeometryModel3D();
            MeshGeometry3D headStoneMesh = MHeadstone();
            headStone.Geometry = headStoneMesh;

            // headStone.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Azure));

            GeometryModel3D pillar = new GeometryModel3D();
            MeshGeometry3D pillarMesh = MPillar();
            pillar.Geometry = pillarMesh;

            //pillar.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Azure));

            GeometryModel3D piramide = new GeometryModel3D();
            MeshGeometry3D piramideMesh = MPiramide();
            piramide.Geometry = piramideMesh;

            //piramide.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Azure));            

            GeometryModel3D baseCube = new GeometryModel3D();
            MeshGeometry3D baseCubeMesh = MBaseCube();
            baseCube.Geometry = baseCubeMesh;

            //baseCube.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Azure));

            GeometryModel3D placard = new GeometryModel3D();
            MeshGeometry3D placardMesh = MPlacard();
            placard.Geometry = placardMesh;

            //placard.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Brown));

            // Make the surface's material using an image brush.
            ImageBrush colors_brush = new ImageBrush();
            colors_brush.ImageSource = new BitmapImage(new Uri("E:\\School\\C#tech\\1617GPVandeKerckhoveFlor\\Topic01\\Topic01\\wood.jpg", UriKind.Relative));
            DiffuseMaterial colors_material = new DiffuseMaterial(colors_brush);

            ImageBrush colors_brush_bronze = new ImageBrush();
            colors_brush_bronze.ImageSource = new BitmapImage(new Uri("E:\\School\\C#tech\\1617GPVandeKerckhoveFlor\\Topic01\\Topic01\\bronze.jpg", UriKind.Relative));
            DiffuseMaterial colors_material_bronze = new DiffuseMaterial(colors_brush_bronze);

            Cube1.Material = colors_material;
            stairs1.Material = colors_material;
            headStone.Material = colors_material;
            pillar.Material = colors_material;
            piramide.Material = colors_material;
            baseCube.Material = colors_material;
            placard.Material = colors_material_bronze;
            Cube1.BackMaterial = colors_material;
            stairs1.BackMaterial = colors_material;
            headStone.BackMaterial = colors_material;
            pillar.BackMaterial = colors_material;
            piramide.BackMaterial = colors_material;

            DirectionalLight DirLight1 = new DirectionalLight();
            DirLight1.Color = Colors.White;
            DirLight1.Direction = new Vector3D(-1, -1, -1);

            Camera1 =new PerspectiveCamera();
            Camera1.FarPlaneDistance = 60;
            Camera1.NearPlaneDistance = 1;
            Camera1.FieldOfView = 60;
            Camera1.Position = new Point3D(0, piramideTop / 2, 40);
            Camera1.LookDirection =
                              new Vector3D(0, 0, -1);
            Camera1.UpDirection =
                                 new Vector3D(0, 1, 0);
            Console.WriteLine(Camera1.Position + "test");

            Model3DGroup modelGroup =
                                    new Model3DGroup();
            modelGroup.Children.Add(Cube1);
            modelGroup.Children.Add(stairs1);
            modelGroup.Children.Add(headStone);
            modelGroup.Children.Add(pillar);
            modelGroup.Children.Add(piramide);
            modelGroup.Children.Add(baseCube);
            modelGroup.Children.Add(placard);
            modelGroup.Children.Add(DirLight1);
            ModelVisual3D modelsVisual = new ModelVisual3D();
            modelsVisual.Content = modelGroup;

            Viewport3D myViewport = new Viewport3D();
            myViewport.Camera = Camera1;
            myViewport.Children.Add(modelsVisual);
            Canvas1.Children.Add(myViewport);

            // this.Content = myViewport;

            myViewport.Height = 1000;
            myViewport.Width = 800;
            Canvas.SetTop(myViewport, 0);
            Canvas.SetLeft(myViewport, 0);
            this.Width = myViewport.Width;
            this.Height = myViewport.Height;

            AxisAngleRotation3D axis = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            RotateTransform3D Rotate = new RotateTransform3D(axis);
            /*Cube1.Transform = Rotate;
            stairs1.Transform = Rotate;
            headStone.Transform = Rotate;
            pillar.Transform = Rotate;
            piramide.Transform = Rotate;*/
            Camera1.Transform = Rotate;
            DoubleAnimation RotAngle = new DoubleAnimation();
            RotAngle.From = 0;
            RotAngle.To = 360;
            RotAngle.Duration = new Duration(TimeSpan.FromSeconds(20.0));
            RotAngle.RepeatBehavior =RepeatBehavior.Forever;
            NameScope.SetNameScope(Canvas1,new NameScope());
            Canvas1.RegisterName("cubeaxis", axis);
            Storyboard.SetTargetName(RotAngle,"cubeaxis");
            Storyboard.SetTargetProperty(RotAngle,new PropertyPath(AxisAngleRotation3D.AngleProperty));
            RotCube = new Storyboard();
            RotCube.Children.Add(RotAngle);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(button.Content.Equals("start"))
            {
                button.Content = "stop";
                RotCube.Begin(Canvas1);
            }
            else if(button.Content.Equals("stop"))
            {
                button.Content = "start";
                RotCube.Pause(Canvas1);
                Console.WriteLine("pause");
            }
        }

        private void buttonCamera_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                Point3DAnimation cameraAnim = new Point3DAnimation(Camera1.Position, PositionCamera(0,i), new Duration(TimeSpan.FromSeconds(5)));
                Camera1.BeginAnimation(PerspectiveCamera.PositionProperty, cameraAnim);
                /*Vector3DAnimation cameraLookAnim = new Vector3DAnimation(new Vector3D(Camera1.LookDirection.X+i, Camera1.LookDirection.Y + i, Camera1.LookDirection.Z + i), new Duration(TimeSpan.FromSeconds(5)));
                Camera1.BeginAnimation(PerspectiveCamera.LookDirectionProperty, cameraLookAnim);*/
                Thread.Sleep(1000);
                Console.WriteLine("position " + Camera1.Position);
            }
            Console.WriteLine("look " + Camera1.LookDirection);
            Console.WriteLine("position " + Camera1.Position);
        }

        private Point3D PositionCamera(double CameraP, double CameraT)
        {
            CameraR = 40;
            CameraPhi = CameraP;
            CameraTheta = CameraT;
            // Calculate the camera's position in Cartesian coordinates.
            double y = CameraR * Math.Sin(CameraPhi);
            double hyp = CameraR * Math.Cos(CameraPhi);
            double x = hyp * Math.Cos(CameraTheta);
            double z = hyp * Math.Sin(CameraTheta);
            return new Point3D(x, piramideTop/2, z);
        }
    }
}
