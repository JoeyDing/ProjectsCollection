using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sfb.UI.Wpf
{
    public static class Utils
    {
        public static void SaveToExcel(string buildVersion, DateTime startDate, DateTime endDate, List<TestResult> data, string filePath)
        {
            //gather list of errors
            var sb = new StringBuilder();

            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);

            //add workbook part
            var workBookPart = spreadsheetDocument.AddWorkbookPart();
            workBookPart.Workbook = new Workbook();

            //add style to workbook
            var workBookStylesPart = workBookPart.AddNewPart<WorkbookStylesPart>();
            GeneratePartContent(workBookStylesPart);

            //add worksheet part
            WorksheetPart workSheetPart = workBookPart.AddNewPart<WorksheetPart>();
            workSheetPart.Worksheet = new Worksheet(new SheetData());

            //add sheets to the workbook
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            //append a new worksheet and associate it with the workbook
            var sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(workSheetPart),
                SheetId = 1,
                Name = "Automation Result"
            };
            sheets.Append(sheet);

            //get the sheetData cell table
            SheetData sheetData = workSheetPart.Worksheet.GetFirstChild<SheetData>();

            //add build and time information
            Row buildRow = new Row() { RowIndex = 1 };
            sheetData.Append(buildRow);
            buildRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Build Version") });
            buildRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(buildVersion) });

            Row processDateRow = new Row() { RowIndex = 2 };
            sheetData.Append(processDateRow);
            processDateRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Run Date:") });
            processDateRow.Append(new Cell()
            {
                DataType = CellValues.String,
                CellValue = new CellValue(string.Format("From {0} to {1}",
                startDate.ToString(), endDate.ToString()))
            });

            //add columns size
            Columns tableColumns1 = new Columns() { };
            Column tableColumn1 = new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 15D, CustomWidth = true };
            tableColumns1.Append(tableColumn1);
            foreach (var languages in data.GroupBy(c => c.Language))
            {
                Column tableColumn = new Column() { Min = (UInt32Value)1U, Max = (UInt32Value)1U, Width = 15D, CustomWidth = true };
                tableColumns1.Append(tableColumn);
            }

            //sheetData.Append(tableColumns1);
            workSheetPart.Worksheet.InsertBefore<Columns>(tableColumns1, sheetData);

            var emptyRow = new Row() { RowIndex = 4 };

            //create table headers row
            Row tableHeaderRow = new Row() { RowIndex = emptyRow.RowIndex };
            sheetData.Append(tableHeaderRow);
            var headers = new List<string> { "Test Cases" };
            headers.AddRange(data.GroupBy(c => c.Language).Select(c => c.Key).OrderBy(c => c));
            headers.ForEach(
                name =>
                {
                    var cell = new Cell() { DataType = CellValues.String };
                    cell.CellValue = new CellValue(name);
                    tableHeaderRow.Append(cell);
                });

            uint i = tableHeaderRow.RowIndex;

            //dump out interesting values

            foreach (var SR in data.GroupBy(c => c.TestCaseName).OrderBy(c => c.Key).ToList())
            {
                i++;
                Row row = new Row() { RowIndex = i };
                sheetData.Append(row);

                var lineData = new Dictionary<string, string>();
                lineData.Add("Test Cases", SR.Key);
                foreach (var item in SR)
                {
                    lineData[item.Language] = item.Result;
                }

                foreach (var header in headers)
                {
                    var cell = new Cell() { DataType = CellValues.String };
                    if (lineData.ContainsKey(header))
                    {
                        cell.CellValue = new CellValue(lineData[header]);
                    }
                    else
                    {
                        cell.CellValue = new CellValue("");
                    }

                    row.Append(cell);
                }
            }

            //add summary information
            Row summaryRow = new Row() { RowIndex = ++i };
            Row percentageRow = new Row() { RowIndex = ++i };
            sheetData.Append(summaryRow);
            sheetData.Append(percentageRow);
            summaryRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Summary") });
            percentageRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Success rate") });

            var totalTestCases = data.GroupBy(c => c.TestCaseName).Count();
            var groupedData = data.GroupBy(c => c.Language).ToList();
            foreach (var header in headers.Skip(1).ToList())
            {
                if (groupedData.Any(c => c.Key == header))
                {
                    var group = groupedData.First(c => c.Key == header);
                    var totalSuccess = group.Count(c => c.Result == "Passed");
                    summaryRow.Append(new Cell()
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(string.Format("{0}/{1}", totalSuccess, totalTestCases))
                    });

                    percentageRow.Append(new Cell()
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(string.Format("{0}%", Math.Round(Convert.ToDouble(totalSuccess) / Convert.ToDouble(totalTestCases) * 100, 2)))
                    });
                }
            }

            //add percentage row

            //add autofilter, conditional formating and margin (required)
            //add auto filter

            //var autoFilter = new AutoFilter() { Reference = "A1:H" + i.ToString() };
            var autoFilter = new AutoFilter() { Reference = string.Format("A{0}:{1}{2}", tableHeaderRow.RowIndex, GetExcelColumnName(headers.Count), i.ToString()) };
            workSheetPart.Worksheet.AppendChild<AutoFilter>(autoFilter);

            // add conditional formating
            ConditionalFormatting conditionalFormatting1 = new ConditionalFormatting() { SequenceOfReferences = new ListValue<StringValue>() { InnerText = "$A:$ZZ" } };
            ConditionalFormattingRule conditionalFormattingRule1 = new ConditionalFormattingRule() { Type = ConditionalFormatValues.ContainsText, FormatId = (UInt32Value)0U, Priority = 1, Operator = ConditionalFormattingOperatorValues.Equal/*, Text = "TRUE" */};
            Formula formula1 = new Formula();
            formula1.Text = "NOT(ISERROR(SEARCH(\"Failed\", A1)))";
            conditionalFormattingRule1.Append(formula1);
            conditionalFormatting1.Append(conditionalFormattingRule1);

            ConditionalFormatting conditionalFormatting2 = new ConditionalFormatting() { SequenceOfReferences = new ListValue<StringValue>() { InnerText = "$A:$ZZ" } };
            ConditionalFormattingRule conditionalFormattingRule2 = new ConditionalFormattingRule() { Type = ConditionalFormatValues.ContainsText, FormatId = (UInt32Value)1U, Priority = 1, Operator = ConditionalFormattingOperatorValues.Equal/*, Text = "TRUE" */};
            Formula formula2 = new Formula();
            formula2.Text = "NOT(ISERROR(SEARCH(\"Passed\", A1)))";
            conditionalFormattingRule2.Append(formula2);
            conditionalFormatting2.Append(conditionalFormattingRule2);

            workSheetPart.Worksheet.AppendChild<ConditionalFormatting>(conditionalFormatting1);
            workSheetPart.Worksheet.AppendChild<ConditionalFormatting>(conditionalFormatting2);

            //add page margin /!\ must be added at the end if using autofilter/conditionalFormating
            PageMargins pageMargins1 = new PageMargins() { Left = 0.7D, Right = 0.7D, Top = 0.75D, Bottom = 0.75D, Header = 0.3D, Footer = 0.3D };
            workSheetPart.Worksheet.AppendChild<PageMargins>(pageMargins1);

            // save the workbook
            workBookPart.Workbook.Save();

            // Close the document.
            spreadsheetDocument.Close();

            if (!string.IsNullOrWhiteSpace(sb.ToString()))
                throw new ArgumentException(sb.ToString());
        }

        //generate style for excel file
        private static void GeneratePartContent(WorkbookStylesPart part)
        {
            //Global
            Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            Fonts fonts1 = new Fonts() { Count = (UInt32Value)1U, KnownFonts = true };

            Font font1 = new Font();
            FontSize fontSize1 = new FontSize() { Val = 11D };
            Color color1 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName1 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };

            font1.Append(fontSize1);
            font1.Append(color1);
            font1.Append(fontName1);
            font1.Append(fontFamilyNumbering1);
            font1.Append(fontScheme1);
            fonts1.Append(font1);

            Fills fills1 = new Fills() { Count = (UInt32Value)2U };
            Fill fill1 = new Fill();
            PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };
            fill1.Append(patternFill1);
            Fill fill2 = new Fill();
            PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };
            fill2.Append(patternFill2);
            fills1.Append(fill1);
            fills1.Append(fill2);

            Borders borders1 = new Borders() { Count = (UInt32Value)1U };
            Border border1 = new Border();
            LeftBorder leftBorder1 = new LeftBorder();
            RightBorder rightBorder1 = new RightBorder();
            TopBorder topBorder1 = new TopBorder();
            BottomBorder bottomBorder1 = new BottomBorder();
            DiagonalBorder diagonalBorder1 = new DiagonalBorder();
            border1.Append(leftBorder1);
            border1.Append(rightBorder1);
            border1.Append(topBorder1);
            border1.Append(bottomBorder1);
            border1.Append(diagonalBorder1);
            borders1.Append(border1);

            CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
            CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };
            cellStyleFormats1.Append(cellFormat1);
            CellFormats cellFormats1 = new CellFormats() { Count = (UInt32Value)1U };
            CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
            cellFormats1.Append(cellFormat2);
            CellStyles cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
            CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };
            cellStyles1.Append(cellStyle1);

            DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)2U };
            DifferentialFormat differentialFormat1 = new DifferentialFormat();
            Font font2 = new Font();
            Color color2 = new Color() { Rgb = "FF9C0006" };
            font2.Append(color2);
            Fill fill3 = new Fill();
            PatternFill patternFill3 = new PatternFill();
            BackgroundColor backgroundColor1 = new BackgroundColor() { Rgb = "FFFFC7CE" };
            patternFill3.Append(backgroundColor1);
            fill3.Append(patternFill3);
            differentialFormat1.Append(font2);
            differentialFormat1.Append(fill3);
            differentialFormats1.Append(differentialFormat1);

            //DifferentialFormats differentialFormats2 = new DifferentialFormats() { Count = (UInt32Value)1U };
            DifferentialFormat differentialFormat2 = new DifferentialFormat();
            Font font3 = new Font();
            Color color3 = new Color() { Rgb = "FFFFFF" };
            font3.Append(color3);
            Fill fill4 = new Fill();
            PatternFill patternFill4 = new PatternFill();
            BackgroundColor backgroundColor2 = new BackgroundColor() { Rgb = "00CC00" };
            patternFill4.Append(backgroundColor2);
            fill4.Append(patternFill4);
            differentialFormat2.Append(font3);
            differentialFormat2.Append(fill4);
            differentialFormats1.Append(differentialFormat2);

            TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };
            stylesheet1.Append(fonts1);
            stylesheet1.Append(fills1);
            stylesheet1.Append(borders1);
            stylesheet1.Append(cellStyleFormats1);
            stylesheet1.Append(cellFormats1);
            stylesheet1.Append(cellStyles1);
            stylesheet1.Append(differentialFormats1);
            stylesheet1.Append(tableStyles1);
            part.Stylesheet = stylesheet1;
        }

        private static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}