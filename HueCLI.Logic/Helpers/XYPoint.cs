namespace HueCLI.Logic
{
    public struct XYPoint
    {
        public readonly double x;
        public readonly double y;
        public readonly double z;

        public XYPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
            this.z = 1.0 - x - y;
        }
    }
}