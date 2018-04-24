namespace BasicModule.Models.Common
{
    public class KeyValuePair
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public KeyValuePair(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
