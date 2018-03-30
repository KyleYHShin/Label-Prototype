namespace BasicModule.Models.Rule
{
    public interface IRuleObject
    {
        IRuleObject Clone();
        string PrintValue();
    }
}
