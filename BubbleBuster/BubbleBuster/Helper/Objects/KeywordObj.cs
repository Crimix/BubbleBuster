namespace BubbleBuster.Helper.Objects
{
    public class KeywordObj
    {
        public string Name { get; set; } = "";
        public int Neg { get; set; } = 0;
        public int Pos { get; set; } = 0;
        public int Bas { get; set; } = 0;

        public KeywordObj(string nameVal, int negVal, int baseVal, int posVal)
        {
            Name = nameVal;
            Neg = negVal;
            Bas = baseVal;
            Pos = posVal;
        }
    }
}
