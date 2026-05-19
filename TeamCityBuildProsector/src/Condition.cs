namespace TeamCityBuildProsector
{
  internal enum Condition
  {
    Contains,
    DoesNotContain,

    Equals,
    DoesNotEqual,

    Matches,
    DoesNotMatch,

    Exists,
    NotExists,

    LessThan,
    MoreThan,
    NoLessThan,
    NoMoreThan,

    StartsWith,
    EndsWith,

    VerMoreThan,
    VerLessThan,
    VerNoMoreThan,
    VerNoLessThan,
  }
}