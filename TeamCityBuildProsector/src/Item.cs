using System;

namespace TeamCityBuildProsector
{
  internal record Item(Condition Condition, string Value) : IComparable<Item>
  {
    public int CompareTo(Item? other)
    {
      if (ReferenceEquals(this, other))
        return 0;
      if (other is null)
        return 1;
      var res = Condition.CompareTo(other.Condition);
      if (res != 0)
        return res;
      return Value.CompareTo(other.Value, StringComparison.Ordinal);
    }
  }
}