using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace TeamCityBuildProsector
{
  internal static class ReadUtil
  {
    private static readonly Regex ourSubstitutionRegex = new("%([^%]+)%", RegexOptions.Compiled);

    public static (List<Tuple<string, string, Dictionary<string, List<Item>>>> Table, List<string> Columns) Load(DirectoryInfo dir, Regex? filter = null)
    {
      if (!dir.Exists)
        throw new DirectoryNotFoundException($"Directory not found: {dir.FullName}");

      var resolver = new ParameterResolver(dir);
      var table = new List<Tuple<string, string, Dictionary<string, List<Item>>>>();
      var columns = new HashSet<string>();
      foreach (var projectDir in dir.EnumerateDirectories().Where(d => filter == null || filter.IsMatch(d.Name)))
      {
        var buildTypesDir = new DirectoryInfo(Path.Combine(projectDir.FullName, "buildTypes"));
        if (buildTypesDir.Exists)
          foreach (var file in buildTypesDir.EnumerateFiles("*.xml"))
          {
            var requirements = ParseBuildTypeFile(file, name => resolver.Resolve(projectDir.Name, name));
            if (requirements != null)
            {
              foreach (var r in requirements.Keys)
                columns.Add(r);
              table.Add(Tuple.Create(projectDir.Name, Path.GetFileNameWithoutExtension(file.Name), requirements));
            }
          }
      }

      return (table, columns.Order().ToList());
    }

    public static List<Tuple<int, Dictionary<string, List<Item>>>> Aggregate(List<Tuple<string, string, Dictionary<string, List<Item>>>> rows)
    {
      var counters = new Dictionary<byte[], Tuple<Ref<int>, Dictionary<string, List<Item>>>>(ByteArrayComparer.Instance);
      foreach (var row in rows)
      {
        byte[] key;
        using (var mem = new MemoryStream())
        using (var wr = new StreamWriter(mem))
        {
          foreach (var item in row.Item3.OrderBy(x => x.Key))
          {
            wr.Write(item.Key);
            wr.Write(item.Value.Count);
            foreach (var x in item.Value.Order())
            {
              wr.Write(x.Condition);
              wr.Write(x.Value);
            }
          }

          wr.Flush();
          key = mem.ToArray();
        }

        if (!counters.TryGetValue(key, out var data))
          counters.Add(key, data = Tuple.Create(new Ref<int>(), row.Item3));
        ++data.Item1.Value;
      }

      return counters.OrderByDescending(x => x.Value.Item1.Value).Select(x => Tuple.Create(x.Value.Item1.Value, x.Value.Item2)).ToList();
    }

    private static Dictionary<string, List<Item>>? ParseBuildTypeFile(FileInfo file, Func<string, string?> resolver)
    {
      var rootSection = XDocument.Load(file.FullName).Root;
      var settingsSection = rootSection?.Element("settings");
      var options = XmlUtil.ReadOptions(settingsSection);

      if (options.TryGetValue("buildConfigurationType", out var buildConfigurationType) && buildConfigurationType == "COMPOSITE")
        return null;

      var parameters = XmlUtil.ReadParameters(settingsSection);

      string ExpandValue(string str) => ourSubstitutionRegex.Replace(str, m =>
        {
          var name = m.Groups[1].Value;
          return parameters.TryGetValue(name, out var value) ? ExpandValue(value) : resolver(name) ?? m.Value;
        });

      return XmlUtil.ReadRequirements(settingsSection).ToDictionary(
        x => ExpandValue(x.Key),
        v => v.Value.Select(x => x with { Value = ExpandValue(x.Value) }).Distinct().Order().ToList());
    }
  }
}