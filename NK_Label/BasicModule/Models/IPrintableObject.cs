namespace BasicModule.Models
{
    public interface IPrintableObject
    {
        string Text { get; set; }
        string OriginText { get; set; }
        IPrintableObject Clone { get; }
    }
}
