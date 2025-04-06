namespace MammothWPF.Models
{
    /// <summary>
    /// Точка с координатами X и Y
    /// </summary>
    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}