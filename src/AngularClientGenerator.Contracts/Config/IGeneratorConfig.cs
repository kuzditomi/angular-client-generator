namespace AngularClientGenerator.Contracts.Config
{
    public interface IGeneratorConfig
    {
        string ExportPath { get; set; }
        Language Language { get; set; }
    }
}
