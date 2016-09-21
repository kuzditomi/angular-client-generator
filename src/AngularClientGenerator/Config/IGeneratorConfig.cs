using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularClientGenerator.Visitor;

namespace AngularClientGenerator.Config
{
    public interface IGeneratorConfig
    {
        string ExportPath { get; set; }
        Language Language { get; set; }
    }
}
