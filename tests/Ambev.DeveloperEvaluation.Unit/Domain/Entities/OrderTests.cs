using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class OrderTests
    {
        [Fact(DisplayName = "Must apply 10% discount when quantity is between 4 and 9")]
        public void AddItem_ShouldApply10PercentDiscount_WhenQuantityIsBetween4And9()
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), false, 0, 0);
            var orderItem = new OrderItem(Guid.NewGuid(), "Product A", 5, 100m);

            // Act
            order.AddItem(orderItem);

            // Assert
            orderItem.Discount.Should().Be(0.10m);
            orderItem.CalculateValue().Should().Be(450m); // 5 * 100 * (1 - 0.10)
            order.Discount.Should().Be(50m); // 5 * 100 * 0.10
        }

        [Fact(DisplayName = "Must apply 20% discount when quantity is between 10 and 20")]
        public void AddItem_ShouldApply20PercentDiscount_WhenQuantityIsBetween10And20()
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), false, 0, 0);
            var orderItem = new OrderItem(Guid.NewGuid(), "Product B", 10, 100m);

            // Act
            order.AddItem(orderItem);

            // Assert
            orderItem.Discount.Should().Be(0.20m);
            orderItem.CalculateValue().Should().Be(800m); // 10 * 100 * (1 - 0.20)
            order.Discount.Should().Be(200m); // 10 * 100 * 0.20
        }

        [Fact(DisplayName = "Should not apply discount when quantity is less than 4")]
        public void AddItem_ShouldNotApplyDiscount_WhenQuantityIsLessThan4()
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), false, 0, 0);
            var orderItem = new OrderItem(Guid.NewGuid(), "Product C", 3, 100m);

            // Act
            order.AddItem(orderItem);

            // Assert
            orderItem.Discount.Should().Be(0m);
            orderItem.CalculateValue().Should().Be(300m); // 3 * 100 * (1 - 0)
            order.Discount.Should().Be(0m);
        }

        [Fact(DisplayName = "Should throw exception when quantity exceeds 20")]
        public void AddItem_ShouldThrowException_WhenQuantityExceeds20()
        {
            // Arrange
            var order = new Order(Guid.NewGuid(), false, 0, 0);
            var orderItem = new OrderItem(Guid.NewGuid(), "Product D", 21, 100m);

            // Act
            Action act = () => order.AddItem(orderItem);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("The maximum quantity per product is 20.");
        }
    }
}
