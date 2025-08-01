using Xunit;
using FluentAssertions;
using SpecAPI.Services;
using System.IO;
using SpecAPI.Logic;

namespace SpecAPI.Tests
{
    public class TestLoaderTests
    {
        [Fact]
        public void Load_ShouldLoadTestsFromYamlFile()
        {
            // Arrange
            var filePath = Path.Combine("Examples", "sample-test.yaml");

            // Act
            var tests = TestLoader.Load(filePath);

            // Assert
            tests.Should().NotBeNull();
            tests.Should().HaveCountGreaterThan(0);
        }
    }
}
