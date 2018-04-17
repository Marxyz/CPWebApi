using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Migrations;

namespace CreativePowerAPI.Models
{
    public class Report
    {

        public string CompanyName { get; set; }
        public string ProjectTaskName { get; set; }
        public string InvestorName { get; set; }
        public string ProjectName { get; set; }        
        public int Id { get; set; }
        public string Content { get; set; }
        public int ProjectTaskId { get; set; }
        public DateTime CreateDate { get; set; }
        public List<File> Files { get; set; }
        public PriceList PriceList { get; set; }
        public string SwitcherNisNumber { get; set; }
        public string BoxNisNumber { get; set; }
        public ReportState State { get; set; } = ReportState.Sent;
        public string ReportCsvUrl { get; set; }
    }
    public enum ReportState
    {
        Started,
        Sent,
        Confirmed = 9
    }
}