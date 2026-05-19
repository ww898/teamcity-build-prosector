using Xunit;

namespace TeamCityBuildProsector.Tests
{
  public class ConditionUtilTests
  {
    [Theory]
    [InlineData("contains", (int)Condition.Contains)]
    [InlineData("does-not-contain", (int)Condition.DoesNotContain)]
    [InlineData("equals", (int)Condition.Equals)]
    [InlineData("does-not-equal", (int)Condition.DoesNotEqual)]
    [InlineData("matches", (int)Condition.Matches)]
    [InlineData("does-not-match", (int)Condition.DoesNotMatch)]
    [InlineData("exists", (int)Condition.Exists)]
    [InlineData("not-exists", (int)Condition.NotExists)]
    [InlineData("less-than", (int)Condition.LessThan)]
    [InlineData("more-than", (int)Condition.MoreThan)]
    [InlineData("no-less-than", (int)Condition.NoLessThan)]
    [InlineData("no-more-than", (int)Condition.NoMoreThan)]
    [InlineData("starts-with", (int)Condition.StartsWith)]
    [InlineData("ends-with", (int)Condition.EndsWith)]
    [InlineData("ver-less-than", (int)Condition.VerLessThan)]
    [InlineData("ver-more-than", (int)Condition.VerMoreThan)]
    [InlineData("ver-no-less-than", (int)Condition.VerNoLessThan)]
    [InlineData("ver-no-more-than", (int)Condition.VerNoMoreThan)]
    public void Parse_ReturnsExpectedCondition(string teamcityTagName, int expectedCondition)
    {
      Assert.Equal((Condition)expectedCondition, ConditionUtil.Parse(teamcityTagName));
    }

    [Theory]
    [InlineData((int)Condition.Contains, "contains")]
    [InlineData((int)Condition.DoesNotContain, "!contains")]
    [InlineData((int)Condition.Equals, "=")]
    [InlineData((int)Condition.DoesNotEqual, "!=")]
    [InlineData((int)Condition.Matches, "matches")]
    [InlineData((int)Condition.DoesNotMatch, "!matches")]
    [InlineData((int)Condition.Exists, "exist")]
    [InlineData((int)Condition.NotExists, "!exist")]
    [InlineData((int)Condition.LessThan, "<")]
    [InlineData((int)Condition.MoreThan, ">")]
    [InlineData((int)Condition.NoLessThan, ">=")]
    [InlineData((int)Condition.NoMoreThan, "<=")]
    [InlineData((int)Condition.StartsWith, "startsWith")]
    [InlineData((int)Condition.EndsWith, "endsWith")]
    [InlineData((int)Condition.VerLessThan, "<ver")]
    [InlineData((int)Condition.VerMoreThan, ">ver")]
    [InlineData((int)Condition.VerNoLessThan, ">=ver")]
    [InlineData((int)Condition.VerNoMoreThan, "<=ver")]
    public void ToString_ReturnsExpectedString(int condition, string expectedPresentationStr)
    {
      Assert.Equal(expectedPresentationStr, ((Condition)condition).ToPresentationString());
    }
  }
}