# Submission Notes

## 1. Evidence for web API with ASP.NET

Project path: `w01_assignments/ContosoPizza`

The Web API controller module is implemented in these files:

- [w01_assignments/ContosoPizza/Program.cs](./w01_assignments/ContosoPizza/Program.cs): registers controllers and maps them with `app.MapControllers()`.
- [w01_assignments/ContosoPizza/Controllers/PizzaController.cs](./w01_assignments/ContosoPizza/Controllers/PizzaController.cs): includes `GET`, `GET {id}`, `POST`, `PUT {id}`, and `DELETE {id}` endpoints as mentioned in the course.
- [w01_assignments/ContosoPizza/Services/PizzaService.cs](./w01_assignments/ContosoPizza/Services/PizzaService.cs): contains the in-memory data used by the controller.


## 2. Working Sales Summary Function for Part 2

Project path: `w01_assignments/mslearn-dotnet-files`

The assignment code follows the Microsoft Learn module flow and adds an extra function that generates `salesTotalDir/sales-summary.txt`.

```csharp
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
```

Generated output file:

[w01_assignments/mslearn-dotnet-files/salesTotalDir/sales-summary.txt](./w01_assignments/mslearn-dotnet-files/salesTotalDir/sales-summary.txt) 
