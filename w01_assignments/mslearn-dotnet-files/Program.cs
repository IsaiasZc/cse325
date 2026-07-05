using Newtonsoft.Json;
using System.Text;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

GenerateSalesSummaryReport(salesFiles, salesTotal, Path.Combine(salesTotalDir, "sales-summary.txt"));

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);

        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

void GenerateSalesSummaryReport(IEnumerable<string> salesFiles, double salesTotal, string reportPath)
{
    var reportBuilder = new StringBuilder();

    reportBuilder.AppendLine("Sales Summary");
    reportBuilder.AppendLine("------------------------------");
    reportBuilder.AppendLine($"Total Sales: {salesTotal:C}");
    reportBuilder.AppendLine();
    reportBuilder.AppendLine("Details:");

    foreach (var file in salesFiles.OrderBy(file => file))
    {
        var fileTotal = GetDetailedFileTotal(file);
        var relativePath = Path.GetRelativePath(currentDirectory, file);

        reportBuilder.AppendLine($"{relativePath}: {fileTotal:C}");
    }

    File.WriteAllText(reportPath, reportBuilder.ToString());
}

double GetDetailedFileTotal(string filePath)
{
    var salesJson = File.ReadAllText(filePath);
    var json = JsonConvert.DeserializeObject<Dictionary<string, double>?>(salesJson);

    if (json is null)
    {
        return 0;
    }

    if (json.TryGetValue("Total", out var total))
    {
        return total;
    }

    if (json.TryGetValue("OverallTotal", out var overallTotal))
    {
        return overallTotal;
    }

    return 0;
}

record SalesData(double Total);
