namespace AngularClientGenerator.ExampleWebAPI.Models
{
    public enum Color
    {
        Red,
        Green,
        Blue
    }

    public class ExampleModel
    {
        public string Message { get; set; }
        public int Id { get; set; }
        public Color Color { get; set; }
    }
}