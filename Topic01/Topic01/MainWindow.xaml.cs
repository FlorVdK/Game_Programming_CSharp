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

        private double cubeBaseHeight = 0;
        private double cubeBaseWidth = 4;
        double space;
        double headStoneHeigth = 1;
        double headStoneWidth = 5;

        MeshGeometry3D MCube()
        {
            MeshGeometry3D cube = new MeshGeometry3D();
            Point3DCollection corners = new
                                   Point3DCollection();
            corners.Add(new Point3D(cubeBaseWidth , cubeBaseHeight + cubeBaseWidth, cubeBaseWidth));
            corners.Add(new Point3D(cubeBaseWidth * 0.75, cubeBaseHeight + cubeBaseWidth, cubeBaseWidth));
            corners.Add(new Point3D(cubeBaseWidth * 0.75, cubeBaseHeight, cubeBaseWidth));
            corners.Add(new Point3D(cubeBaseWidth, cubeBaseHeight, cubeBaseWidth));
            corners.Add(new Point3D(-cubeBaseWidth * 0.75, cubeBaseHeight + cubeBaseWidth, cubeBaseWidth));
            corners.Add(new Point3D(-cubeBaseWidth, cubeBaseHeight + cubeBaseWidth, cubeBaseWidth));
            corners.Add(new Point3D(-cubeBaseWidth, cubeBaseHeight, cubeBaseWidth));
            corners.Add(new Point3D(-cubeBaseWidth * 0.75, cubeBaseHeight, cubeBaseWidth));
            corners.Add(new Point3D(cubeBaseWidth * 0.75, cubeBaseHeight + cubeBaseWidth, cubeBaseWidth * 0.75));
            corners.Add(new Point3D(-cubeBaseWidth * 0.75, cubeBaseHeight + cubeBaseWidth, cubeBaseWidth * 0.75));
            corners.Add(new Point3D(-cubeBaseWidth * 0.75, cubeBaseHeight, cubeBaseWidth * 0.75));
            corners.Add(new Point3D(cubeBaseWidth * 0.75, cubeBaseHeight, cubeBaseWidth * 0.75));
            corners.Add(new Point3D(cubeBaseWidth, cubeBaseHeight + cubeBaseWidth , -cubeBaseWidth));
            corners.Add(new Point3D(-cubeBaseWidth, cubeBaseHeight + cubeBaseWidth , -cubeBaseWidth));
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
              //Top
                 0,5,12,
                 0,12,13,
              //Bottom
                 3,6,14,
                 3,14,15,
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


            cube.TextureCoordinates = woodpoints;

            return cube;
        }

        MeshGeometry3D MStairs(int number)
        {
            MeshGeometry3D stairs = new MeshGeometry3D();
            Point3DCollection corners = new Point3DCollection();
            space = 0.2;
            for (int i = 0; i<number; i++)
            {
                corners.Add(new Point3D(cubeBaseWidth + space * (i + 1), cubeBaseHeight - space * i, cubeBaseWidth + space * (i + 1)));
                corners.Add(new Point3D(-cubeBaseWidth - space * (i + 1), cubeBaseHeight - space * i, cubeBaseWidth + space * (i + 1)));
                corners.Add(new Point3D(-cubeBaseWidth - space * (i + 1), cubeBaseHeight - space * (i + 1), cubeBaseWidth + space * (i + 1)));
                corners.Add(new Point3D(cubeBaseWidth + space * (i + 1), cubeBaseHeight - space * (i + 1), cubeBaseWidth + space * (i + 1)));
                corners.Add(new Point3D(cubeBaseWidth + space * (i + 1), cubeBaseHeight - space * i, -cubeBaseWidth - space * (i + 1)));
                corners.Add(new Point3D(-cubeBaseWidth - space * (i + 1), cubeBaseHeight - space * i, -cubeBaseWidth - space * (i + 1)));
                corners.Add(new Point3D(-cubeBaseWidth - space * (i + 1), cubeBaseHeight - space * (i + 1), -cubeBaseWidth - space * (i + 1)));
                corners.Add(new Point3D(cubeBaseWidth + space * (i + 1), cubeBaseHeight - space * (i + 1), -cubeBaseWidth - space * (i + 1)));
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


            stairs.TextureCoordinates = woodpoints;

            return stairs;
        }

        MeshGeometry3D MHeadstone()
        {
            MeshGeometry3D headStone = new MeshGeometry3D();
            Point3DCollection corners = new Point3DCollection();

            corners.Add(new Point3D(headStoneWidth, cubeBaseWidth + headStoneHeigth, headStoneWidth));
            corners.Add(new Point3D(-headStoneWidth, cubeBaseWidth + headStoneHeigth, headStoneWidth));
            corners.Add(new Point3D(-cubeBaseWidth , cubeBaseWidth, cubeBaseWidth));
            corners.Add(new Point3D(cubeBaseWidth , cubeBaseWidth, cubeBaseWidth ));
            corners.Add(new Point3D(headStoneWidth, cubeBaseWidth + headStoneHeigth, -headStoneWidth));
            corners.Add(new Point3D(-headStoneWidth, cubeBaseWidth + headStoneHeigth, -headStoneWidth));
            corners.Add(new Point3D(-cubeBaseWidth , cubeBaseWidth, -cubeBaseWidth ));
            corners.Add(new Point3D(cubeBaseWidth , cubeBaseWidth, -cubeBaseWidth ));

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


            headStone.TextureCoordinates = woodpoints;

            return headStone;
        }

        private void Window_Loaded(object sender,
                                  RoutedEventArgs e)
        {
            GeometryModel3D Cube1 =
                                 new GeometryModel3D();
            MeshGeometry3D cubeMesh = MCube();
            Cube1.Geometry = cubeMesh;

            Cube1.Material = new DiffuseMaterial(
                     new SolidColorBrush(Colors.Azure));

            GeometryModel3D stairs1 =
                                 new GeometryModel3D();
            MeshGeometry3D stairMesh = MStairs(10);
            stairs1.Geometry = stairMesh;

            stairs1.Material = new DiffuseMaterial(
                     new SolidColorBrush(Colors.Azure));

            GeometryModel3D headStone =
                                 new GeometryModel3D();
            MeshGeometry3D headStoneMesh = MHeadstone();
            headStone.Geometry = headStoneMesh;

            headStone.Material = new DiffuseMaterial(
                     new SolidColorBrush(Colors.Azure));

            // Make the surface's material using an image brush.
            //ImageBrush colors_brush = new ImageBrush();
            //colors_brush.ImageSource =
            //    new BitmapImage(new Uri("wood.jpg", UriKind.Relative));
            //DiffuseMaterial colors_material =
            //    new DiffuseMaterial(colors_brush);

            //Cube1.Material = colors_material;
            //stairs1.Material = colors_material;




            DirectionalLight DirLight1 =
                                new DirectionalLight();
            DirLight1.Color = Colors.White;
            DirLight1.Direction =
                              new Vector3D(-1, -1, -1);

            PerspectiveCamera Camera1 =
                               new PerspectiveCamera();
            Camera1.FarPlaneDistance = 45;
            Camera1.NearPlaneDistance = 1;
            Camera1.FieldOfView = 45;
            Camera1.Position = new Point3D(8, 8, 12);
            Camera1.LookDirection =
                              new Vector3D(-8, -8, -12);
            Camera1.UpDirection =
                                 new Vector3D(0, 1, 0);

            Model3DGroup modelGroup =
                                    new Model3DGroup();
            modelGroup.Children.Add(Cube1);
            modelGroup.Children.Add(stairs1);
            modelGroup.Children.Add(headStone);
            modelGroup.Children.Add(DirLight1);
            ModelVisual3D modelsVisual =
                                   new ModelVisual3D();
            modelsVisual.Content = modelGroup;

            Viewport3D myViewport = new Viewport3D();
            myViewport.Camera = Camera1;
            myViewport.Children.Add(modelsVisual);
            Canvas1.Children.Add(myViewport);

            // this.Content = myViewport;

            myViewport.Height = 800;
            myViewport.Width = 800;
            Canvas.SetTop(myViewport, 0);
            Canvas.SetLeft(myViewport, 0);
            this.Width = myViewport.Width;
            this.Height = myViewport.Height;

            /*AxisAngleRotation3D axis =
                         new AxisAngleRotation3D(
                           new Vector3D(0, 1, 0), 0);
            RotateTransform3D Rotate =
                         new RotateTransform3D(axis);
            Cube1.Transform = Rotate;
            DoubleAnimation RotAngle =
                               new DoubleAnimation();
            RotAngle.From = 0;
            RotAngle.To = 360;
            RotAngle.Duration = new Duration(
                         TimeSpan.FromSeconds(20.0));
            RotAngle.RepeatBehavior =
                             RepeatBehavior.Forever;
            NameScope.SetNameScope(Canvas1,
                                   new NameScope());
            Canvas1.RegisterName("cubeaxis", axis);
            Storyboard.SetTargetName(RotAngle,
                                       "cubeaxis");
            Storyboard.SetTargetProperty(RotAngle,
             new PropertyPath(
                AxisAngleRotation3D.AngleProperty));
            Storyboard RotCube = new Storyboard();
            RotCube.Children.Add(RotAngle);
            RotCube.Begin(Canvas1);*/
        }
    }
}
