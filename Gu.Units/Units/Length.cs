namespace Gu.Units
{
    public partial struct Length
    {
        public static Area operator *(Length left, Length right)
        {
            return new Area(left.Metres * right.Metres);
        }

        public static Speed operator /(Length left, Time right)
        {
            return new Speed(left.Metres / right.Seconds);
        }
    }
}
