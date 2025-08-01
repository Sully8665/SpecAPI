using SpecAPI.Models;

namespace SpecAPI.Interfaces;

public interface ITestLoader
{
    List<TestCase> Load(string filePath);
} 