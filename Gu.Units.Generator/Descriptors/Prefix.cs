namespace Gu.Units.Generator
{
    public class Prefix
    {
        public Prefix(string name, string shortName, int power)
        {
            Name = name;
            ShortName = shortName;
            Power = power;
        }
        
        public string Name { get; set; }
        
        public string ShortName { get; set; }
        
        public int Power { get; set; }
    }
}