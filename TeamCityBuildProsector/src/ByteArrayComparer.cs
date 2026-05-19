using System;
using System.Collections.Generic;

namespace TeamCityBuildProsector
{
  internal sealed class ByteArrayComparer : IEqualityComparer<byte[]>
  {
    public static readonly ByteArrayComparer Instance = new();

    public bool Equals(byte[]? x, byte[]? y) => ReferenceEquals(x, y) || (x != null && y != null && x.SequenceEqual(y));

    public int GetHashCode(byte[] obj)
    {
      var hash = new HashCode();
      hash.AddBytes(obj);
      return hash.ToHashCode();
    }
  }
}