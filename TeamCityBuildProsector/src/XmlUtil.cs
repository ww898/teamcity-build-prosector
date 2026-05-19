using System.Collections.Generic;
using System.Xml.Linq;

namespace TeamCityBuildProsector
{
  internal static class XmlUtil
  {
    public static Dictionary<string, string> ReadParameters(XElement? parent)
    {
      var parameters = new Dictionary<string, string>();
      var parametersSection = parent?.Element("parameters");
      if (parametersSection != null)
        foreach (var param in parametersSection.Elements("param"))
        {
          var name = param.Attribute("name")?.Value;
          if (name != null)
            parameters[name] = param.Attribute("value")?.Value ?? "";
        }

      return parameters;
    }

    public static Dictionary<string, string> ReadOptions(XElement? parent)
    {
      var options = new Dictionary<string, string>();
      var optionsSection = parent?.Element("options");
      if (optionsSection != null)
        foreach (var option in optionsSection.Elements("option"))
        {
          var name = option.Attribute("name")?.Value;
          if (name != null)
            options[name] = option.Attribute("value")?.Value ?? "";
        }

      return options;
    }

    public static Dictionary<string, List<Item>> ReadRequirements(XElement? parent)
    {
      var requirements = new Dictionary<string, List<Item>>();
      var requirementsSection = parent?.Element("requirements");
      if (requirementsSection != null)
        foreach (var requirement in requirementsSection.Elements())
        {
          var name = requirement.Attribute("name")?.Value;
          if (name != null)
          {
            if (!requirements.TryGetValue(name, out var items))
              requirements.Add(name, items = []);
            items.Add(new Item(ConditionUtil.Parse(requirement.Name.LocalName), requirement.Attribute("value")?.Value ?? ""));
          }
        }

      return requirements;
    }
  }
}