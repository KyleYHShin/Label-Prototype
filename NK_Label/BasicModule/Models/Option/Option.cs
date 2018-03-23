namespace BasicModule.Models.Option
{
    public class Option
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public Option(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
