namespace TeamCityBuildProsector
{
  internal sealed class Ref<TValue>
    where TValue : struct
  {
    public TValue Value;

    public override string ToString() => Value.ToString()!;
  }
}