using System;
using FluentAssertions;
using Sprint1CSharp.Services.Prediction;
using Xunit;

namespace Trackyard.Tests;

public class PredictionServiceTests
{
    [Fact]
    public void Predict_RecentLowMileage_ShouldBeLowRisk()
    {
        var svc = new PredictionService();
        var result = svc.Predict(DateTime.UtcNow.Year, 10_000);
        result.Predicted.Should().BeFalse();
        result.Probability.Should().BeLessThan(0.6f);
    }

    [Fact]
    public void Predict_OldHighMileage_ShouldBeHighRisk()
    {
        var svc = new PredictionService();
        var result = svc.Predict(2010, 150_000);
        result.Predicted.Should().BeTrue();
        result.Probability.Should().BeGreaterThan(0.5f);
    }
}
