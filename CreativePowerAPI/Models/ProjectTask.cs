using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace CreativePowerAPI.Models
{
    public
        class ProjectTask : ATask
    {
        
        public List<OnLineObject> OnLineObjects { get; set; }
        public List<Models.File> Files { get; set; }
        public MarginalLinePointTask StartingLineTask { get; set; }
        public MarginalLinePointTask EndingLineTask { get; set; }

        
    }
}

public enum TaskType
{
    Point = 1,
    Line = 2
}
