using System;
using VPets.Domain.Models;
using VPets.Domain.Models.Metrics;
using Xunit;

namespace VPetsUnitTests.Models
{
    /// <summary>
    /// Test base metrics class by proxy - could be split as metric
    /// functionality expands.
    /// </summary>
    public class MetricTest
    {
        /// <summary>
        /// Make the metric deltas instant given time taken to process test.
        /// </summary>
        private const int INSTANT_DELTA = int.MaxValue;

        [Fact]
        public void TestMetricsDegradeCorrectly()
        {
            // Arrange
            Metric descendingMetric = new HappinessMetric(INSTANT_DELTA);
            Metric ascendingMetric = new HungerMetric(INSTANT_DELTA);

            // Act
            descendingMetric.Degrade();
            ascendingMetric.Degrade();

            // Assert
            Assert.Equal(descendingMetric.Min, descendingMetric.Value);
            Assert.Equal(ascendingMetric.Max, ascendingMetric.Value);
        }

        [Fact]
        public void TestMetricsImproveCorrectly()
        {
            // Arrange
            Metric descendingMetric = new HappinessMetric(INSTANT_DELTA);
            Metric ascendingMetric = new HungerMetric(INSTANT_DELTA);

            // Act
            descendingMetric.Improve();
            ascendingMetric.Improve();

            // Assert
            Assert.Equal(descendingMetric.Max, descendingMetric.Value);
            Assert.Equal(ascendingMetric.Min, ascendingMetric.Value);
        }
    }
}
