using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TeamCityBuildProsector
{
  internal static class WriteMdUtil
  {
    private static readonly HashSet<char> ourEscapingChars = ['\\', '`', '*', '_', '{', '}', '[', ']', '<', '>', '(', ')', '#', '+', '-', '.', '!', '|'];
    private static readonly HashSet<char> ourInlineEscapingChars = ['`', '|'];

    public static void Full(List<Tuple<string, string, Dictionary<string, List<Item>>>> rows, List<string> columns, TextWriter writer)
    {
      var builder = new StringBuilder(1024);

      builder.Append("|File|");
      foreach (var c in columns)
        builder.AppendEscaping(c).Append('|');
      builder.AppendLine();
      builder.Append("|---|");
      foreach (var _ in columns)
        builder.Append("---|");
      builder.AppendLine();
      writer.Write(builder.ToString());

      foreach (var row in rows)
      {
        builder.Length = 0;
        builder.Append('|');
        AppendEscaping(builder, $"{row.Item1} - {row.Item2}").Append('|');
        foreach (var column in columns)
        {
          if (row.Item3.TryGetValue(column, out var items))
          {
            var n = 0;
            foreach (var item in items)
            {
              if (n++ > 0)
                builder.Append("; ");
              AppendEscaping(builder, item.Condition.ToPresentationString());
              if (item.Value.Length > 0)
                AppendInlineEscaping(builder.Append(" `"), item.Value).Append('`');
            }
          }

          builder.Append('|');
        }

        builder.AppendLine();
        writer.Write(builder.ToString());
      }
    }

    public static void Short(List<Tuple<int, Dictionary<string, List<Item>>>> table, List<string> columns, TextWriter writer)
    {
      var builder = new StringBuilder(1024);

      builder.Append("|Count|");
      foreach (var c in columns)
        AppendEscaping(builder, c).Append('|');
      builder.AppendLine();
      builder.Append("|---:|");
      foreach (var _ in columns)
        builder.Append("---|");
      builder.AppendLine();
      writer.Write(builder.ToString());

      foreach (var row in table)
      {
        builder.Length = 0;
        builder.Append('|').Append(row.Item1).Append('|');
        foreach (var column in columns)
        {
          if (row.Item2.TryGetValue(column, out var items))
          {
            var n = 0;
            foreach (var item in items)
            {
              if (n++ > 0)
                builder.Append("; ");
              AppendEscaping(builder, item.Condition.ToPresentationString());
              if (item.Value.Length > 0)
                AppendInlineEscaping(builder.Append(" `"), item.Value).Append('`');
            }
          }

          builder.Append('|');
        }

        builder.AppendLine();
        writer.Write(builder.ToString());
      }
    }

    extension(StringBuilder builder)
    {
      private StringBuilder AppendEscaping(string str) => builder.Escaping(str, ourEscapingChars);
      private StringBuilder AppendInlineEscaping(string str) => builder.Escaping(str, ourInlineEscapingChars);

      private StringBuilder Escaping(string str, HashSet<char> escapingChars)
      {
        foreach (var c in str)
        {
          if (escapingChars.Contains(c))
            builder.Append('\\');
          builder.Append(c);
        }

        return builder;
      }
    }
  }
}