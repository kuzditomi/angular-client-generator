namespace AngularClientGeneratorTest.Util
{
    public static class StringExtensions
    {
        public static int Occures(this string text, string contain)
        {
            return (text.Length - text.Replace(contain, "").Length) / contain.Length;
        }
    }
}
