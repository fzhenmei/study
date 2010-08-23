using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZedGraph;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Westwind.WebToolkit
{
    /// <summary>
    /// Wrapper class around ZedGraph to provide some simple graph renderings
    /// </summary>
    public class Graph
    {
        Color[] DefaultLineColors = new Color[10] { Color.Red, Color.DarkGreen, Color.Goldenrod, Color.Maroon, Color.DarkOrange, Color.DarkCyan, Color.DarkMagenta, Color.DarkSalmon, Color.DarkOrchid, Color.Ivory };
        
        public List<DataPointSet> DataPointSets
        {
            get { return _DataPointSets; }
            set { _DataPointSets = value; }
        }
        private List<DataPointSet> _DataPointSets = new List<DataPointSet>();


        /// <summary>
        /// Title of the graphTitle
        /// </summary>
        public string GraphTitle
        {
            get { return _GraphTitle; }
            set { _GraphTitle = value; }
        }
        private string _GraphTitle = "";

        /// <summary>
        /// Title for X Axis
        /// </summary>
        public string XAxisTitle
        {
            get { return _XAxisTitle; }
            set { _XAxisTitle = value; }
        }
        private string _XAxisTitle = "";

        /// <summary>
        /// Title for the Y Axis
        /// </summary>
        public string YAxisTitle
        {
            get { return _YAxisTitle; }
            set { _YAxisTitle = value; }
        }
        private string _YAxisTitle = "";

        /// <summary>
        /// 
        /// </summary>
        public double ChartHeight
        {
            get { return _ChartHeight; }
            set { _ChartHeight = value; }
        }
        private double _ChartHeight = 400F;

        /// <summary>
        /// The width of the full chart 
        /// </summary>        
        public double ChartWidth
        {
            get { return _ChartWidth; }
            set { _ChartWidth = value; }
        }
        private double _ChartWidth = 500F;


        /// <summary>
        /// Returns an image as a PNG image and saves it to the specified file name. 
        /// 
        /// Note this method always returns NULL.
        /// </summary>
        /// <param name="fileName">Name of the file to output the image to.</param>
        /// <returns>This version of the method always returns NULL</returns>
        public byte[] LineChart(string fileName)
        {
            GraphPane pane = new GraphPane(
                                new RectangleF { Height=(float) this.ChartHeight, 
                                                 Width=(float) this.ChartWidth },
                                this.GraphTitle,this.XAxisTitle,this.YAxisTitle);


            int LineColorIndex = 0;
       
            foreach (DataPointSet data in this.DataPointSets)
            {
                // Build a PointPairList with points based on Sine wave
                PointPairList list = new PointPairList();                

                Type xType = null;
                foreach (KeyValuePair<object, Double> key in data.Data)
                {
                    if (xType == null)
                    {
                        xType = key.Key.GetType();
                        if (xType == typeof(DateTime))
                            pane.XAxis.Type = AxisType.Date;
                    }

                    // Display dates special
                    if (xType == typeof(DateTime))
                    {
                        list.Add(new XDate((DateTime)key.Key), key.Value);
                    }
                    else
                        list.Add(Convert.ToDouble(key.Key), key.Value);

                }

                if (data.LineColor == Color.Empty)
                {
                    // Use fixed colors for first 10
                    if (LineColorIndex < 10)
                    {
                        data.LineColor =  this.DefaultLineColors[LineColorIndex];
                        LineColorIndex++;
                    }
                    else 
                        // Then use random colors
                        data.LineColor = Graph.GetRandomColor(); 
                        
                }

                // Add a curve
                LineItem curve = pane.AddCurve(data.Legend, list, data.LineColor, SymbolType.Circle);
                curve.Line.Width = 2.0F;
                curve.Line.IsAntiAlias = true;
                curve.Symbol.Fill = new Fill(Color.White);
                curve.Symbol.Size = 5;
            }
  
            // Hide the legend
            pane.Legend.IsVisible = true;

            // Fill the axis background with a gradient
            pane.Chart.Fill = new Fill(Color.White, Color.SteelBlue, 45.0F);
            
            pane.YAxis.Scale.MaxGrace = 0.2;

            // Force refresh of chart
            pane.AxisChange();            

            // Save the image out
            Bitmap bmp = pane.GetImage();

            if (fileName == null)
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Png);
                bmp.Dispose();

                return ms.ToArray();
            }
            else
            {
                bmp.Save(fileName, ImageFormat.Png);
                bmp.Dispose();
                return null;
            }
        }

        /// <summary>
        /// Returns a line chart as a byte buffer PNG image.
        /// </summary>
        /// <returns></returns>
        public byte[] LineChart()
        {
            return this.LineChart(null);
        }

        private static Array knownColors = null;
        private static Random rnd = null;

        /// <summary>
        /// Retrieves a random color from known color values.
        /// Used for large numbers of graph data lines
        /// </summary>
        /// <returns></returns>
        public static Color GetRandomColor()
        {
            KnownColor col = KnownColor.Transparent;

            if (knownColors == null)
            {
                knownColors = Enum.GetValues(typeof(KnownColor));
                rnd = new Random();
            }
            col = (KnownColor) knownColors.GetValue(rnd.Next(knownColors.Length));
            return Color.FromKnownColor(col);
        }

    }

    public class DataPointSet
    {
        public Dictionary<object, Double> Data = new Dictionary<object, double>();
        public string Legend = "";
        public Color LineColor = Color.Empty;
    }


}
