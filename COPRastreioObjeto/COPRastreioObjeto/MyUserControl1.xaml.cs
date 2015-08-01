using Bing.Maps;
using Business;
using Entities;
using Entities.Endereco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace COPRastreioObjeto
{
    public sealed partial class MyUserControl1 : UserControl
    {
        MapShapeLayer mShapeLayer;
        MapLayer mPinLayer;
        LocationCollection mPolyShapeLocations;
        LocationCollection mMarkerLocations;

        PointCollection myPointCollection;
        private static SROXml objSROXml;
        string nomeClasse = "ItemDetailPage.xaml.cs";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sroXml"></param>
        public static void objSROXmlSet(SROXml sroXml)
        {
            objSROXml = sroXml;
        }

        public MyUserControl1()
        {
            this.InitializeComponent();

            mShapeLayer = new MapShapeLayer();
            map.ShapeLayers.Add(mShapeLayer);

            mPolyShapeLocations = new LocationCollection();
            myPointCollection = new PointCollection();


            string strMarkers = "53.5609739,134.77280|-14.2350040, -51.925280|-25.59496540,-49.37794850";

            string[] Markers = strMarkers.Split('|');

            Location location;

            try
            {
                this.MarkerMap(Markers);

                if (objSROXml != null && objSROXml.Objetos != null && objSROXml.Objetos.Count > 0 && objSROXml.Objetos[0].Numero.Substring(11) == "BR")//Se for do Brasil deixa o zoom menor. verificar como centralizar no pais.
                {
                    location = new Location(objSROXml.Objetos[0].Eventos[0].GeoX, objSROXml.Objetos[0].Eventos[0].GeoY);
                    map.SetView(location, 11);

                }
                else
                {
                    if (objSROXml.Objetos[0].Eventos != null && objSROXml.Objetos[0].Eventos.Count > 0 && (objSROXml.Objetos[0].Eventos[0].GeoX != 0 && objSROXml.Objetos[0].Eventos[0].GeoY != 0))
                    {
                        location = new Location(objSROXml.Objetos[0].Eventos[0].GeoX, objSROXml.Objetos[0].Eventos[0].GeoY);
                        map.SetView(location, 11);
                    }
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog(nomeClasse, "MyUserControl1", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        public async void MarkerMap(string[] points)
        {
            try
            {
                if (objSROXml != null && objSROXml.Objetos != null && objSROXml.Objetos[0].IdEndereco >= 0)
                {
                    Endereco objEndFim = await Core.GetEnderecos(objSROXml.Objetos[0].IdEndereco);

                    if (objEndFim.GeoX == 0 || objEndFim.GeoY == 0)
                    {
                        objEndFim.BuscaGeoLocalizacaoEndereco();
                        var sucesso = await objEndFim.GravaEndereco();

                        if (sucesso)
                        {

                        }
                    }

                    Location locationFim = new Location(Convert.ToDouble(objEndFim.GeoX), Convert.ToDouble(objEndFim.GeoY));

                    SolidColorBrush s = new SolidColorBrush();
                    s.Color = Windows.UI.Colors.Black;

                    mPolyShapeLocations.Clear();

                    bool entregueDestinatario = Status.VerificarStatus(objSROXml.Objetos[0].Eventos[0].Tipo, Convert.ToInt16(objSROXml.Objetos[0].Eventos[0].Status));

                    if (objSROXml != null && objSROXml.Objetos != null && objSROXml.Objetos.Count > 0)
                    {
                        if (objSROXml.Objetos != null && objSROXml.Objetos.Count > 0 && (objSROXml.Objetos[0].Eventos.Count > 0 && entregueDestinatario))
                        {
                            mPolyShapeLocations.Add(locationFim);
                        }
                        foreach (var point in objSROXml.Objetos[0].Eventos)
                        {

                            Location location = new Location(point.GeoX, point.GeoY);

                            if (!mPolyShapeLocations.Contains(location))
                                mPolyShapeLocations.Add(location);

                            myPointCollection.Add(new Point(point.GeoX, point.GeoY));
                        }
                    }

                    var uri = new Uri("ms-appx:///Assets/cop_icon.png");
                    var image = new Image { Source = new BitmapImage(uri), Margin = new Thickness(-15, -35.0, 0, 0) };
                    image.Width = 34;
                    image.Height = 40;

                    map.Children.Add(image);
                    MapLayer.SetPosition(image, locationFim);

                    if (objSROXml.Objetos != null && objSROXml.Objetos.Count > 0 && objSROXml.Objetos[0].Eventos.Count > 0 && entregueDestinatario)
                    {
                        myPointCollection.Add(new Point(objEndFim.GeoX, objEndFim.GeoY));
                    }

                    AddPolyShape(CreatePolyline());
                }
                else
                {
                    SolidColorBrush s = new SolidColorBrush();
                    s.Color = Windows.UI.Colors.Black;

                    mPolyShapeLocations.Clear();

                    if (objSROXml != null && objSROXml.Objetos != null && objSROXml.Objetos.Count > 0)
                    {
                        foreach (var point in objSROXml.Objetos[0].Eventos)
                        {

                            Location location = new Location(point.GeoX, point.GeoY);

                            if (!mPolyShapeLocations.Contains(location))
                                mPolyShapeLocations.Add(location);

                            myPointCollection.Add(new Point(point.GeoX, point.GeoY));
                        }
                    }

                    AddPolyShape(CreatePolyline());
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog(nomeClasse, "MarkerMap", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapShape"></param>
        private void AddPolyShape(MapShape mapShape)
        {
            mShapeLayer.Shapes.Clear();

            mShapeLayer.Shapes.Add(mapShape);
            bool entregue = false;

            try
            {
                for (int i = 0; i < mPolyShapeLocations.Count; i++)
                {
                    bool entregueDestinatario = Status.VerificarStatus(objSROXml.Objetos[0].Eventos[0].Tipo, Convert.ToInt16(objSROXml.Objetos[0].Eventos[0].Status));

                    if (objSROXml.Objetos != null && objSROXml.Objetos.Count > 0 && objSROXml.Objetos[0].Eventos.Count > 0 && entregueDestinatario && !entregue)
                    {
                        entregue = true;
                        continue;
                    }

                    SiglasCorreios siglasCorreios = new SiglasCorreios();
                    siglasCorreios.VerificaFormaEnvio(objSROXml.Objetos[0].Numero);

                    if (siglasCorreios.CaminhoIcon != string.Empty)
                    {
                        var uris = new Uri(siglasCorreios.CaminhoIcon);
                        var images = new Image { Source = new BitmapImage(uris), Margin = new Thickness(-15, -35.0, 0, 0) };
                        images.Width = 34;
                        images.Height = 40;

                        map.Children.Add(images);
                        MapLayer.SetPosition(images, mPolyShapeLocations[i]);
                    }
                    else
                    {
                        icon icone = new icon();

                        map.Children.Add(icone);
                        MapLayer.SetPosition(icone, mPolyShapeLocations[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog(nomeClasse, "AddPolyShape", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private MapPolyline CreatePolyline()
        {
            MapPolyline polyline = new MapPolyline();
            polyline.Color = Windows.UI.Colors.Black;
            polyline.Width = 3;
            polyline.Locations = mPolyShapeLocations;

            return polyline;
        }

        /// <summary>
        /// 
        /// </summary>
        public void PolyQuadraticBezierSegmentExample()
        {
            SolidColorBrush s = new SolidColorBrush();
            s.Color = Windows.UI.Colors.Black;

            // Create a PathFigure to be used for the PathGeometry of myPath
            PathFigure myPathFigure = new PathFigure();

            // Set the starting point for the PathFigure specifying that the
            // geometry starts at point 10,100.
            myPathFigure.StartPoint = new Point(30, 400);

            // Create a PointCollection that holds the Points used to specify 
            // the points of the PolyQuadraticBezierSegment below.
            myPointCollection = new PointCollection();

            IReadOnlyList<Pointer> p = map.PointerCaptures;

            myPointCollection.Add(new Point(100, 200));
            myPointCollection.Add(new Point(230, 400));

            // The PolyQuadraticBezierSegment specifies two Bezier curves.
            // The first curve is from 10,100 (start point specified above)
            // to 300,100 with a control point of 200,200. The second curve
            // is from 200,200 (end of the last curve) to 30,400 with a 
            // control point of 0,200.
            PolyQuadraticBezierSegment myBezierSegment = new PolyQuadraticBezierSegment();
            myBezierSegment.Points = myPointCollection;

            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
            myPathSegmentCollection.Add(myBezierSegment);

            myPathFigure.Segments = myPathSegmentCollection;

            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            myPathFigureCollection.Add(myPathFigure);

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures = myPathFigureCollection;

            // Create a path to draw a geometry with.
            Windows.UI.Xaml.Shapes.Path myPath = new Windows.UI.Xaml.Shapes.Path();

            myPath.Stroke = s;//Brushes.Black;
            myPath.StrokeThickness = 1;

            //myPath.Width = 3;

            // specify the shape (quadratic Bezier curve) of the path using the StreamGeometry.
            myPath.Data = myPathGeometry;

            // Add path shape to the UI.
            //StackPanel mainPanel = new StackPanel();
            //mainPanel.Children.Add(myPath);
            map.Children.Add(myPath);
            //this.Content = mainPanel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="tension"></param>
        /// <returns></returns>
        private PointCollection GetBezierPoints(PointCollection pts, double tension)
        {
            PointCollection ret = new PointCollection();

            try
            {
                for (int i = 0; i < pts.Count; i++)
                {
                    // for first point append as is.
                    if (i == 0)
                    {
                        ret.Add(pts[0]);
                        continue;
                    }

                    // for each point except first and last get B1, B2. next point. 
                    // Last point do not have a next point.
                    ret.Add(GetB1(pts, i - 1, tension));
                    ret.Add(GetB2(pts, i - 1, tension));
                    ret.Add(pts[i]);
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog(nomeClasse, "GetBezierPoints", ex);
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="i"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private Point GetB1(PointCollection pts, int i, double a)
        {
            var drv = GetDerivative(pts, i, a);
            return new Point(pts[i].X + drv.X / 3, pts[i].Y + drv.Y / 3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="i"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private Point GetB2(PointCollection pts, int i, double a)
        {
            var drv = GetDerivative(pts, i + 1, a);
            return new Point(pts[i + 1].X - drv.X / 3, pts[i + 1].Y - drv.Y / 3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="i"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        private Point GetDerivative(PointCollection pts, int i, double a)
        {
            if (pts.Count < 2)
                throw new ArgumentOutOfRangeException("pts", "MapBezier must contain at least two points.");

            if (i == 0)
            {
                // First point.
                return new Point((pts[1].X - pts[0].X) / a, (pts[1].Y - pts[0].Y) / a);
            }
            if (i == pts.Count - 1)
            {
                // Last point.
                return new Point((pts[i].X - pts[i - 1].X) / a, (pts[i].Y - pts[i - 1].Y) / a);
            }

            return new Point((pts[i + 1].X - pts[i - 1].X) / a, (pts[i + 1].Y - pts[i - 1].Y) / a);
        }
    }
}
