using SpecAPI.Models;
public class TestCase
{
    public string Name { get; set; } = string.Empty;
    public Request Request { get; set; } = new Request();
    public Expect Expect { get; set; } = new Expect();
}