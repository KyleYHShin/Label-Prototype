namespace BasicModule.Models.Common
{
    public interface IPrintableObject
    {
        string Text { get; set; }
        int MaxLength { get; set; }
        string OriginText { get; set; }
        IPrintableObject Clone { get; }
    }
}
