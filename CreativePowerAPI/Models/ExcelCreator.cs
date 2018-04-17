using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Repositories.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ServiceStack;

namespace CreativePowerAPI.Models
{
    public class ExcelCreator
    {
        public readonly ExcelWorkbook Workbook;
        public readonly ExcelPackage Pck;
        public readonly ExcelWorksheets Worksheets;
        public ExcelWorksheet CurrentWorksheet;
        public readonly Dictionary<Tuple<int, int>, object> ExcelData;

        public ExcelCreator(ExcelPackage package)
        {
            Pck = package;
            Workbook = Pck.Workbook;
            Worksheets = Pck.Workbook.Worksheets;
            ExcelData = new Dictionary<Tuple<int, int>, object>();

        }

        public PriceList ExcelDataToPricelist()
        {
            var priceList = new PriceList();
            priceList.Categories = new List<Category>();
            int maxRow = CurrentWorksheet.Dimension.End.Row;
            int counter = 1;
            //int maxColumn = 7;
            bool descChanged = false;
            string firstDesc = (string)ExcelData[new Tuple<int, int>(1, 7)];
            // CO Z TYM CPOWER
            for (int r = 1; r <= maxRow - 3; r++)
            {
                if (ReferenceEquals((ExcelData[new Tuple<int, int>(r, 1)]), string.Empty))
                {
                    continue;
                }
                if (((string) ExcelData[new Tuple<int, int>(r, 1)]).StartsWith($"G{counter}"))
                {
                    if ((string) ExcelData[new Tuple<int, int>(r, 1)] == $"G{counter}")
                    {
                        var category = new Category()
                        {
                            ListofPriceListElements = new List<PriceListElement>(),
                            Name = (string) ExcelData[new Tuple<int, int>(r, 2)]
                        };
                        priceList.Categories.Add(category);
                    }
                    else
                    {
                        string name = (string) ExcelData[new Tuple<int, int>(r, 2)];
                        string unit = (string) ExcelData[new Tuple<int, int>(r, 3)];
                        double price = (double) ExcelData[new Tuple<int, int>(r, 4)];
                        double quant;
                        if (!ReferenceEquals(ExcelData[new Tuple<int, int>(r, 5)], string.Empty))
                        {

                            quant = (double) ExcelData[new Tuple<int, int>(r, 5)];
                        }
                        else
                        {
                            quant = 0;
                        }
                        string desc = (string) ExcelData[new Tuple<int, int>(r, 7)];
                        //Todo Sczytywanie
                        if (desc != firstDesc)
                        {
                            descChanged = true;
                            firstDesc = desc;
                        }
                        priceList.Categories[counter - 1].ListofPriceListElements.Add(new PriceListElement()
                        {
                            Description = desc,
                            Name = name,
                            Price = (float) price,
                            // Na sztywno na 0, jak konrad kazał
                            Quantity = 0,
                            Unit = unit
                        });
                    }

                }
                else
                {
                    
                    counter++;
                    r--;
                }


            }
            foreach (var cat in priceList.Categories)
            {
                cat.Sum = cat.ListofPriceListElements.Sum(el => el.Price);
            }
            return priceList;

        }

        public void ToDictionaryData()
        {
            CurrentWorksheet.Calculate();
            for (int i = CurrentWorksheet.Dimension.Start.Row; i <= CurrentWorksheet.Dimension.End.Row; i++)
            {
                for (int j = CurrentWorksheet.Dimension.Start.Column; j <= CurrentWorksheet.Dimension.End.Column; j++)
                {
                    try
                    {
                        object cellValue = CurrentWorksheet.Cells[i, j].Value == null
                            ? string.Empty
                            : CurrentWorksheet.Cells[i, j].Value;

                        ExcelData.Add(new Tuple<int, int>(i, j), cellValue);

                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }
            }
        }




        public void SaveWork()
        {
            Pck.Save();
        }

        public void SetCurrentWorksheet(string name)
        {
            CurrentWorksheet = Workbook.Worksheets.FirstOrDefault(wk => wk.Name == name);
        }

        public void AddSheet(string name)
        {
            Worksheets.Add(name);
            CurrentWorksheet = Workbook.Worksheets.FirstOrDefault(wk =>  name.StartsWith(wk.Name));
        }
        

        public void AddReportData(int rowOffset, int columnOffset, PriceList pricelist)
        {

            var activeRow = rowOffset;
            var activeColumn = columnOffset;

            int categoryCounter = 1;
            foreach (var category in pricelist.Categories)
            {
                var categoryStartCell = CurrentWorksheet.Cells[activeRow, activeColumn];
                //Category formatting
                using (ExcelRange range = CurrentWorksheet.Cells[activeRow, activeColumn, activeRow, activeColumn + 1])
                {
                    range.Style.Font.Bold = true;
                }

                CurrentWorksheet.Cells[activeRow, activeColumn].Value = $"G{categoryCounter}";
                activeColumn++;
                CurrentWorksheet.Cells[activeRow, activeColumn].Value = category.Name;
                activeRow++;
                activeColumn = columnOffset;
                int elementCounter = 1;

                

                //ToDo :: Scal komórki z tym samym opisem
                string firstDescription = category.ListofPriceListElements[0].Description;
                foreach (var element in category.ListofPriceListElements)
                {

                    if (element.Description == firstDescription && firstDescription != string.Empty )
                    {
                        
                    }
                    CurrentWorksheet.Cells[activeRow, activeColumn].Value = $"G{categoryCounter}_{elementCounter}";
                    activeColumn++;
                    CurrentWorksheet.Cells[activeRow, activeColumn].Value = element.Name;
                    activeColumn++;
                    CurrentWorksheet.Cells[activeRow, activeColumn].Value = element.Unit;
                    activeColumn++;
                    CurrentWorksheet.Cells[activeRow, activeColumn].Value = element.Price;
                    string priceAddres = CurrentWorksheet.Cells[activeRow, activeColumn].Address;
                    activeColumn++;
                    CurrentWorksheet.Cells[activeRow, activeColumn].Value = element.Quantity;
                    string quantAddres = CurrentWorksheet.Cells[activeRow, activeColumn].Address;
                    activeColumn++;
                    CurrentWorksheet.Cells[activeRow, activeColumn].Formula = $"{priceAddres}*{quantAddres}";
                    activeColumn++;
                    //Todo zmien formatowanie description
                    /*using (ExcelRange range = CurrentWorksheet.Cells[activeRow, activeColumn])
                    {
                        range.Style.WrapText = true;
                    }
*/
                    CurrentWorksheet.Cells[activeRow, activeColumn].Value = element.Description;

                    activeColumn = columnOffset;
                    activeRow++;
                    elementCounter++;
                }


                activeColumn = columnOffset + 5;
                //Sum formatting
                using (ExcelRange range = CurrentWorksheet.Cells[activeRow, activeColumn-1, activeRow, activeColumn + 1])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
                CurrentWorksheet.Cells[activeRow, activeColumn].Value = category.ListofPriceListElements.Sum(el => el.Price * el.Quantity);
                activeColumn--;
                CurrentWorksheet.Cells[activeRow, activeColumn].Value = "Podsuma";
                activeColumn = columnOffset;
                var categoryEndCell = CurrentWorksheet.Cells[activeRow, columnOffset + 6];
                //Category Border formmatting


                using (ExcelRange range = CurrentWorksheet.Cells[categoryStartCell.End.Row  , categoryStartCell.End.Column,categoryEndCell.End.Row,categoryEndCell.End.Column] )
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                //Koniec kategorii
                activeRow++;
                categoryCounter++;

            }
            activeColumn = columnOffset + 4;
            //End sum formatting
            using (ExcelRange range = CurrentWorksheet.Cells[activeRow, activeColumn, activeRow, activeColumn + 1])
            {
                range.Style.Font.Bold = true;
                range.Style.Border.BorderAround(ExcelBorderStyle.None);
            }

            CurrentWorksheet.Cells[activeRow, activeColumn].Value = "Suma";
            activeColumn++;
            CurrentWorksheet.Cells[activeRow, activeColumn].Value = pricelist.Categories.Sum(cat => cat.ListofPriceListElements.Sum(el => el.Price * el.Quantity));

            //End formatting
            using (ExcelRange range = CurrentWorksheet.Cells[rowOffset, columnOffset, activeRow - 1, columnOffset + 6])
            {  
                range.AutoFitColumns();
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Justify;
                
            }
            
            using (ExcelRange range = CurrentWorksheet.Cells[rowOffset, columnOffset + 5, activeRow , columnOffset + 5])
            {
                range.Style.Numberformat.Format = "##0.00 zł#";
            }
            using (ExcelRange range = CurrentWorksheet.Cells[rowOffset, columnOffset + 3, activeRow, columnOffset + 3])
            {
                range.Style.Numberformat.Format = "##0.00 zł#";
            }
            CurrentWorksheet.Column(2).Width = 100;
            CurrentWorksheet.Column(2).Style.WrapText = true;
            CurrentWorksheet.Column(7).Width = 200;
            CurrentWorksheet.Column(7).Style.WrapText = true;
            
        }
    }
}
