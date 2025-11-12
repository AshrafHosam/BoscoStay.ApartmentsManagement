using Application.Contracts.Services;

namespace Persistence.Implementation.Services
{
    internal class FileService : IFileService
    {
        //public byte[] CreateStatementSpreadSheet(FinancialStatementQueryResponse data)
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet1 = package.Workbook.Worksheets.Add("Summary");
        //        AddSummaryWorksheet(data, worksheet1);

        //        var worksheet2 = package.Workbook.Worksheets.Add("Details");
        //        AddDetailsWorksheet(data, worksheet2);

        //        return package.GetAsByteArray();
        //    }
        //}

        //private static void AddDetailsWorksheet(FinancialStatementQueryResponse data, ExcelWorksheet worksheet2)
        //{
        //    worksheet2.Cells[1, 1].Value = "Branch Name";
        //    worksheet2.Cells[1, 2].Value = "Area Name";
        //    worksheet2.Cells[1, 3].Value = "Transation Source";
        //    worksheet2.Cells[1, 4].Value = "Transaction Type";
        //    worksheet2.Cells[1, 5].Value = "Name";
        //    worksheet2.Cells[1, 6].Value = "Amount";
        //    worksheet2.Cells[1, 7].Value = "Date";

        //    worksheet2.Rows[1].Style.Font.Bold = true;

        //    for (int i = 2; i <= data.StatementDetails.Count + 1; i++)
        //    {
        //        worksheet2.Cells[i, 1].Value = data.StatementDetails[i - 2].BranchName;
        //        worksheet2.Cells[i, 2].Value = data.StatementDetails[i - 2].AreaName;
        //        worksheet2.Cells[i, 3].Value = data.StatementDetails[i - 2].TransactionSource;
        //        worksheet2.Cells[i, 4].Value = data.StatementDetails[i - 2].TransactionType;
        //        worksheet2.Cells[i, 5].Value = data.StatementDetails[i - 2].Name;
        //        worksheet2.Cells[i, 6].Value = data.StatementDetails[i - 2].Amount;
        //        worksheet2.Cells[i, 7].Value = data.StatementDetails[i - 2].Date;
        //    }

        //    worksheet2.Columns.AutoFit();
        //}

        //private static void AddSummaryWorksheet(FinancialStatementQueryResponse data, ExcelWorksheet worksheet1)
        //{
        //    worksheet1.Cells[1, 1].Value = "Transaction Source";
        //    worksheet1.Cells[1, 2].Value = "Transaction Type";
        //    worksheet1.Cells[1, 3].Value = "Number of Transactions";
        //    worksheet1.Cells[1, 4].Value = "Total Amount";

        //    worksheet1.Rows[1].Style.Font.Bold = true;

        //    for (int i = 2; i <= data.StatementSummary.Count + 1; i++)
        //    {
        //        worksheet1.Cells[i, 1].Value = data.StatementSummary[i - 2].TransactionSource;
        //        worksheet1.Cells[i, 2].Value = data.StatementSummary[i - 2].TransactionType;
        //        worksheet1.Cells[i, 3].Value = data.StatementSummary[i - 2].NumberOfTransactions;
        //        worksheet1.Cells[i, 4].Value = data.StatementSummary[i - 2].TotalAmount;
        //    }

        //    worksheet1.Columns.AutoFit();
        //}

        //public List<Client> ExportClientDataFromExcel(Guid brandId, IFormFile file)
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    var clients = new List<Client>();
        //    var excelPackage = new ExcelPackage(file.OpenReadStream());

        //    var worksheet = excelPackage.Workbook.Worksheets.First();

        //    var rowCount = worksheet.Dimension.Rows;

        //    for (int row = 2; row <= rowCount; row++)
        //    {
        //        clients.Add(new Client
        //        {
        //            BrandId = brandId,
        //            Name = worksheet.Cells[row, ((int)ExcelColumnsEnum.Name)].Value?.ToString(),
        //            Email = worksheet.Cells[row, ((int)ExcelColumnsEnum.Email)].Value?.ToString().ToLower(),
        //            MobileNumber = worksheet.Cells[row, ((int)ExcelColumnsEnum.MobileNumber)].Value?.ToString(),
        //            ProfessionalTitle = worksheet.Cells[row, ((int)ExcelColumnsEnum.Title)].Value?.ToString(),
        //            Interests = worksheet.Cells[row, ((int)ExcelColumnsEnum.Interests)].Value?.ToString()
        //        });
        //    }

        //    return clients
        //       .Where(a => !string.IsNullOrEmpty(a.Name)
        //       && (!string.IsNullOrEmpty(a.MobileNumber) || !string.IsNullOrEmpty(a.Email)))
        //       .GroupBy(c => new { c.MobileNumber, c.Email })
        //       .Select(group => group.First())
        //       .ToList();
        //}
    }
}
