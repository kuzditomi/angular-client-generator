namespace AngularClientGenerator.Contracts.Config
{
    public interface IGeneratorConfig
    {
        string ExportPath { get; set; }
        ClientType Language { get; set; }
    }
}
