using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TeamCityBuildProsector
{
  internal static class Program
  {
    internal static async Task<int> Main(string[] args)
    {
      try
      {
        var dirArgument = new Argument<DirectoryInfo>("dir") { Description = "Path to the directory with TeamCity generated configs" };
        var modeOption = new Option<Mode>("--mode", "-m")
          {
            Description = "Output mode: full or short",
            DefaultValueFactory = _ => Mode.Short,
            Arity = ArgumentArity.ZeroOrOne,
            CustomParser = result => result.Tokens.SingleOrDefault()?.Value switch
              {
                "full" => Mode.Full,
                "short" => Mode.Short,
                var v => throw new ArgumentException($"Invalid mode '{v}'. Expected 'full' or 'short'.")
              }
          };
        var filterOption = new Option<string?>("--filter", "-f")
          {
            Description = "Regex to filter first-level directories by name"
          };
        var outputOption = new Option<OutputFormat>("--output", "-o")
          {
            Description = "Output format: md or tsv",
            DefaultValueFactory = _ => OutputFormat.Md,
            Arity = ArgumentArity.ZeroOrOne,
            CustomParser = result => result.Tokens.SingleOrDefault()?.Value switch
              {
                "md" => OutputFormat.Md,
                "tsv" => OutputFormat.Tsv,
                var v => throw new ArgumentException($"Invalid output format '{v}'. Expected 'md' or 'tsv'.")
              }
          };
        var rootCommand = new RootCommand("TeamCity Build Prosector") { Options = { modeOption, filterOption, outputOption }, Arguments = { dirArgument } };
        rootCommand.SetAction(parseResult =>
          {
            var rootDirectory = parseResult.GetRequiredValue(dirArgument);
            var mode = parseResult.GetRequiredValue(modeOption);
            var output = parseResult.GetRequiredValue(outputOption);
            var filterPattern = parseResult.GetValue(filterOption);
            var filter = filterPattern != null ? new Regex(filterPattern, RegexOptions.Compiled) : null;

            var (rows, columns) = ReadUtil.Load(rootDirectory, filter);
            switch (mode)
            {
            case Mode.Full:
              switch (output)
              {
              case OutputFormat.Tsv:
                WriteTsvUtil.Full(rows, columns, Console.Out);
                break;
              case OutputFormat.Md:
                WriteMdUtil.Full(rows, columns, Console.Out);
                break;
              default:
                throw new ArgumentOutOfRangeException(nameof(output), output, null);
              }

              break;
            case Mode.Short:
              var aggregated = ReadUtil.Aggregate(rows);
              switch (output)
              {
              case OutputFormat.Tsv:
                WriteTsvUtil.Short(aggregated, columns, Console.Out);
                break;
              case OutputFormat.Md:
                WriteMdUtil.Short(aggregated, columns, Console.Out);
                break;
              default:
                throw new ArgumentOutOfRangeException(nameof(output), output, null);
              }

              break;
            default:
              throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
          });
        return await rootCommand.Parse(args).InvokeAsync();
      }
      catch (Exception ex)
      {
        await Console.Error.WriteLineAsync($"ERROR: {ex.Message}");
        return 1;
      }
    }
  }
}