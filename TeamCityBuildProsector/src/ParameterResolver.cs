using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace TeamCityBuildProsector
{
  internal class ParameterResolver(DirectoryInfo dir)
  {
    private record CacheItem(Dictionary<string, string> Params, string? ParentId);

    private readonly Dictionary<string, CacheItem> myCache = new();

    public string? Resolve(string? subDir, string name)
    {
      while (subDir != null)
      {
        if (!myCache.TryGetValue(subDir, out var cacheItem))
        {
          var projectConfig = new FileInfo(Path.Combine(Path.Combine(dir.FullName, subDir), "project-config.xml"));
          var rootSection = XDocument.Load(projectConfig.FullName).Root;
          myCache[subDir] = cacheItem = new CacheItem(XmlUtil.ReadParameters(rootSection), rootSection?.Attribute("parent-id")?.Value);
        }

        if (cacheItem.Params.TryGetValue(name, out var value))
          return value;

        subDir = cacheItem.ParentId;
      }

      return null;
    }
  }
}