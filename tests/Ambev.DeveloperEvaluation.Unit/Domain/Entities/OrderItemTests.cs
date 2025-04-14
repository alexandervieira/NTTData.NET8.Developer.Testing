using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class OrderItemTests
    {
        [Fact(DisplayName = "Item: Must apply 10% discount when quantity is between 4 and 9")]
        public void ApplyDiscount_ShouldApply10PercentDiscount_WhenQuantityIsBetween4And9()
        {
            // Arrange
            var orderItem = new OrderItem(Guid.NewGuid(), "Product A", 5, 100m);

            // Act
            orderItem.ApplyDiscount(0.10m);

            // Assert
            orderItem.Discount.Should().Be(0.10m);
            orderItem.CalculateValue().Should().Be(450m); // 5 * 100 * (1 - 0.10)
        }

        [Fact(DisplayName = "Item: Must apply 20% discount when quantity is between 10 and 20")]
        public void ApplyDiscount_ShouldApply20PercentDiscount_WhenQuantityIsBetween10And20()
        {
            // Arrange
            var orderItem = new OrderItem(Guid.NewGuid(), "Product B", 10, 100m);

            // Act
            orderItem.ApplyDiscount(0.20m);

            // Assert
            orderItem.Discount.Should().Be(0.20m);
            orderItem.CalculateValue().Should().Be(800m); // 10 * 100 * (1 - 0.20)
        }

        [Fact(DisplayName = "Item: Should not apply discount when quantity is less than 4")]
        public void ApplyDiscount_ShouldNotApplyDiscount_WhenQuantityIsLessThan4()
        {
            // Arrange
            var orderItem = new OrderItem(Guid.NewGuid(), "Product C", 3, 100m);

            // Act
            orderItem.ApplyDiscount(0m);

            // Assert
            orderItem.Discount.Should().Be(0m);
            orderItem.CalculateValue().Should().Be(300m); // 3 * 100 * (1 - 0)
        }
    }
}
