using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularClientGenerator
{
    public interface IGeneratorConfig
    {
        string ExportPath { get; set; }
        Language Language { get; set; }
    }
}
