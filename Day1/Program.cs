// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

var text = await File.ReadAllLinesAsync("aocinput.txt");

int total = 0;

var regex1 = new Regex(@"\D*(\d).*(\d)\D*", new RegexOptions { });
var regex2 = new Regex(@"\D*(\d)\D*", new RegexOptions { });

foreach (var line in text)
{
    var regexResult = regex1.Match(line);
    int value = 0;

    if (regexResult.Success)
    {
        // we have two groups, so we have two numbers
        value += int.Parse(regexResult.Groups[1].Value) * 10;
        value += int.Parse(regexResult.Groups[2].Value);
    }
    else
    {
        regexResult = regex2.Match(line);
        if (!regexResult.Success)
        {
            throw new Exception("Regex failed");
        }

        value += int.Parse(regexResult.Groups[1].Value);
        value += int.Parse(regexResult.Groups[1].Value) * 10;
    }

    Console.WriteLine($"Value: {value}");
    total += value;
}

Console.WriteLine($"Total: {total}");
