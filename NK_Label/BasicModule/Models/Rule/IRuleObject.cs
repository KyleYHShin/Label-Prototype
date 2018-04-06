namespace BasicModule.Models.Rule
{
    public interface IRuleObject
    {
        IRuleObject Clone { get; }
        string PrintValue { get; }
    }
}
