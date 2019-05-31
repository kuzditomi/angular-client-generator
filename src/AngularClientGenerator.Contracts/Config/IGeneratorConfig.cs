namespace AngularClientGenerator.Contracts.Config
{
    public interface IGeneratorConfig
    {
        string ExportPath { get; set; }
        ClientType ClientType { get; set; }
    }
}
