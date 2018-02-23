using MockingBlog;
using Moq;
using System.Linq;
using Xunit;

namespace MockingBlog.Tests
{
    [Trait("Category", "OrderValidator")]
    public class OrderValidatorTests
    {
        // This is a weak test setup. The test will fail as soon as a new validation requirement
        // is added, and the builder setup will have to be amended to get all the valid components
        // included for the order validation to succeed.

        [Fact]
        public void ValidOrder_IsValid()
        {
            var emptyOrder = new OrderBuilder().Build();
            var sut = new OrderValidator();
            var sutInvalidResult = sut.Validate(emptyOrder);

            Assert.False(sutInvalidResult.IsValid);

            var validOrder = new OrderBuilder()
              .WithOrderLine()
              .WithShippingAddress()
              .Build();

            var sutValidResult = sut.Validate(validOrder);

            Assert.True(sutValidResult.IsValid);
        }

        [Fact]
        public void OrderWithOutOrderLine_IsNotValid()
        {
            var emptyOrder = new OrderBuilder()
                .Build();
            var sut = new OrderValidator();

            var sutInvalidResult = sut.Validate(emptyOrder);

            Assert.False(sutInvalidResult.IsValid);
            Assert.Single(sutInvalidResult.ValidationErrors);
            Assert.Equal(ValidationErrorTypes.OrderlinesRequired, sutInvalidResult.ValidationErrors.First().Type);
        }

        [Fact]
        public void OrderWithOutShippingAddress_IsNotValid()
        {
            var emptyOrder = new OrderBuilder()
                .WithOrderLine()
                .Build();
            var sut = new OrderValidator();

            var sutInvalidResult = sut.Validate(emptyOrder);

            Assert.False(sutInvalidResult.IsValid);
            Assert.Single(sutInvalidResult.ValidationErrors);
            Assert.Equal(ValidationErrorTypes.ShippingAddressRequired, sutInvalidResult.ValidationErrors.First().Type);
        }

        [Fact]
        public void OrderMustHaveOneOrderLine()
        {
            var validOrder = new OrderBuilder()
                .WithOrderLine()
                .Build();
            var sut = new OrderValidator();

            var sutValidResult = sut.Validate(validOrder);

            Assert.True(sutValidResult.IsValid);
        }

    }
}
