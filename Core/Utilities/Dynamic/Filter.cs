namespace Core.Utilities.Dynamic;

public class Filter
{
    public string Field { get; set; } // Nesnesin Prop Adı
    public string Operator { get; set; } // = > < == != <= >= 
    public string? Value { get; set; }// Nesnesin Prop Değeri
    public string? Logic { get; set; }// And / Or
    public IEnumerable<Filter>? Filters { get; set; }

    public Filter()
    {
    }

    public Filter(string field, string @operator, string? value, string? logic, IEnumerable<Filter>? filters) : this()
    {
        Field = field;
        Operator = @operator;
        Value = value;
        Logic = logic;
        Filters = filters;
    }
}