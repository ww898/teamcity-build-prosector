using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TeamCityBuildProsector
{
  internal static class WriteTsvUtil
  {
    private static readonly HashSet<char> ourEscapingChars = ['\t', '\r', '\n'];

    public static void Full(List<Tuple<string, string, Dictionary<string, List<Item>>>> rows, List<string> columns, TextWriter writer)
    {
      var builder = new StringBuilder(1024);

      builder.Append("File");
      foreach (var c in columns)
        builder.Append('\t').AppendEscaping(c);
      builder.AppendLine();
      writer.Write(builder.ToString());

      foreach (var row in rows)
      {
        builder.Length = 0;
        builder.AppendEscaping($"{row.Item1} - {row.Item2}");
        foreach (var column in columns)
        {
          builder.Append('\t');
          if (row.Item3.TryGetValue(column, out var items))
            AppendItems(builder, items);
        }

        builder.AppendLine();
        writer.Write(builder.ToString());
      }
    }

    public static void Short(List<Tuple<int, Dictionary<string, List<Item>>>> table, List<string> columns, TextWriter writer)
    {
      var builder = new StringBuilder(1024);

      builder.Append("Count");
      foreach (var c in columns)
        builder.Append('\t').AppendEscaping(c);
      builder.AppendLine();
      writer.Write(builder.ToString());

      foreach (var row in table)
      {
        builder.Length = 0;
        builder.Append(row.Item1);
        foreach (var column in columns)
        {
          builder.Append('\t');
          if (row.Item2.TryGetValue(column, out var items))
            AppendItems(builder, items);
        }

        builder.AppendLine();
        writer.Write(builder.ToString());
      }
    }

    private static void AppendItems(StringBuilder builder, List<Item> items)
    {
      var n = 0;
      foreach (var item in items)
      {
        if (n++ > 0)
          builder.Append("; ");
        builder.AppendEscaping(item.Condition.ToPresentationString());
        if (item.Value.Length > 0)
          builder.Append(' ').AppendEscaping(item.Value);
      }
    }

    extension(StringBuilder builder)
    {
      private StringBuilder AppendEscaping(string str)
      {
        foreach (var c in str)
          builder.Append(ourEscapingChars.Contains(c) ? ' ' : c);
        return builder;
      }
    }
  }
}