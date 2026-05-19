using System;
using System.Linq;

namespace TeamCityBuildProsector
{
  internal static class ConditionUtil
  {
    public static Condition Parse(string value) => Enum.Parse<Condition>(string.Concat(value.Split('-').Select(w => char.ToUpperInvariant(w[0]) + w[1..])));

    public static string ToPresentationString(this Condition condition) => condition switch
      {
        Condition.Contains => "contains",
        Condition.DoesNotContain => "!contains",
        Condition.Equals => "=",
        Condition.DoesNotEqual => "!=",
        Condition.Matches => "matches",
        Condition.DoesNotMatch => "!matches",
        Condition.Exists => "exist",
        Condition.NotExists => "!exist",
        Condition.LessThan => "<",
        Condition.MoreThan => ">",
        Condition.NoLessThan => ">=",
        Condition.NoMoreThan => "<=",
        Condition.StartsWith => "startsWith",
        Condition.EndsWith => "endsWith",
        Condition.VerLessThan => "<ver",
        Condition.VerMoreThan => ">ver",
        Condition.VerNoLessThan => ">=ver",
        Condition.VerNoMoreThan => "<=ver",
        _ => throw new ArgumentOutOfRangeException(nameof(condition), condition, null)
      };
  }
}