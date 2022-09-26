using FluentAssertions;
using NetArchTest.Rules;

namespace Architecture.Tests;

public class ArchitectureTests
{
    private const string _domainNamespace = "UnicornValley.Domain";
    private const string _applicationNamespace = "UnicornValley.Application";
    private const string _infrastructureNamespace = "UnicornValley.Infrastructure";
    private const string _webApiNamespace = "UnicornValley.WebAPI";

    [Fact]
    public void Domain_ShouldNotHaveDependencyOnOtherProjects()
    {
        var assembly = typeof(UnicornValley.Domain.AssemblyMarker).Assembly;

        var otherProjects = new[]
        {
            _applicationNamespace,
            _infrastructureNamespace,
            _webApiNamespace
        };

        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_ShouldNotHaveDependencyOnOtherProjects()
    {
        var assembly = typeof(UnicornValley.Application.AssemblyMarker).Assembly;

        var otherProjects = new[]
        {
            _infrastructureNamespace,
            _webApiNamespace
        };

        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_ShouldNotHaveDependencyOnOtherProjects()
    {
        var assembly = typeof(UnicornValley.Infrastructure.AssemblyMarker).Assembly;

        var otherProjects = new[]
        {
            _webApiNamespace
        };

        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        testResult.IsSuccessful.Should().BeTrue();
    }
}