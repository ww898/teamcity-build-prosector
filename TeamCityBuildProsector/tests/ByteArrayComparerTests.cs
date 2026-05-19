using Xunit;

namespace TeamCityBuildProsector.Tests;

public class ByteArrayComparerTests
{
  [Fact]
  public void Equals_SameReference_ReturnsTrue()
  {
    var array = new byte[] { 1, 2, 3 };
    Assert.True(ByteArrayComparer.Instance.Equals(array, array));
  }

  [Fact]
  public void Equals_SameContent_ReturnsTrue()
  {
    Assert.True(ByteArrayComparer.Instance.Equals([1, 2, 3], [1, 2, 3]));
  }

  [Fact]
  public void Equals_DifferentContent_ReturnsFalse()
  {
    Assert.False(ByteArrayComparer.Instance.Equals([1, 2, 3], [1, 2, 4]));
  }

  [Fact]
  public void Equals_DifferentLengths_ReturnsFalse()
  {
    Assert.False(ByteArrayComparer.Instance.Equals([1, 2], [1, 2, 3]));
  }

  [Fact]
  public void Equals_BothNull_ReturnsTrue()
  {
    Assert.True(ByteArrayComparer.Instance.Equals(null, null));
  }

  [Fact]
  public void Equals_OneNull_ReturnsFalse()
  {
    Assert.False(ByteArrayComparer.Instance.Equals(null, [1, 2, 3]));
    Assert.False(ByteArrayComparer.Instance.Equals([1, 2, 3], null));
  }

  [Fact]
  public void Equals_BothEmpty_ReturnsTrue()
  {
    Assert.True(ByteArrayComparer.Instance.Equals([], []));
  }

  [Fact]
  public void GetHashCode_EqualArrays_ReturnsSameHash()
  {
    Assert.Equal(ByteArrayComparer.Instance.GetHashCode([1, 2, 3]), ByteArrayComparer.Instance.GetHashCode([1, 2, 3]));
  }

  [Fact]
  public void GetHashCode_DifferentArrays_ReturnsDifferentHash()
  {
    Assert.NotEqual(ByteArrayComparer.Instance.GetHashCode([1, 2, 3]), ByteArrayComparer.Instance.GetHashCode([1, 2, 4]));
  }

  [Fact]
  public void GetHashCode_EmptyArray_IsConsistent()
  {
    Assert.Equal(ByteArrayComparer.Instance.GetHashCode([]), ByteArrayComparer.Instance.GetHashCode([]));
  }
}