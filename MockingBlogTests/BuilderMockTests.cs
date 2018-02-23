using MockingBlog;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MockingBlog.Tests
{
    // mocking everything can be a nice way to design by contract only, without
    // any actual implementations but still being able to test the interface validity
    [Trait("Category", "CanMockEverything")]
    public class BuilderMockTests
    {
        [Fact]
        public void TestInterfacesOnlyWithoutBuilders()
        {
            var validatorMock = new Mock<IOrderValidator>();
            var validationResultsMock = new Mock<IValidationResults>();

            var orderMock = new Mock<IOrder>();

            validatorMock.Setup(s => s.Validate(orderMock.Object)).Returns(validationResultsMock.Object);

            IOrderValidator sut = validatorMock.Object;

            var sutResult = sut.Validate(orderMock.Object);

            Assert.False(sutResult.IsValid);
        }

        [Fact]
        public void TestInterfacesOnlyWithoutBuilders_ReturnsIsValid_True()
        {
            var validatorMock = new Mock<IOrderValidator>();
            var validationResultsMock = new Mock<IValidationResults>();

            var orderMock = new Mock<IOrder>();

            validatorMock.Setup(s => s.Validate(orderMock.Object)).Returns(validationResultsMock.Object);
            validationResultsMock.Setup(s => s.IsValid).Returns(true);

            IOrderValidator sut = validatorMock.Object;

            var sutResult = sut.Validate(orderMock.Object);

            Assert.True(sutResult.IsValid);
        }


        [Fact]
        public void CanValidateOrder()
        {
            var order = new OrderBuilder()
              .Build();

            var validator = new ValidatorBuilder()
              .Build();

            var result = validator.Validate(order);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void CanValidateOrder_WithOrderLine_AndShippingAddress()
        {
            var order = new OrderBuilder()
              .WithOrderLine()
              .WithShippingAddress()
              .Build();

            var validator = new ValidatorBuilder()
              .Build();

            var result = validator.Validate(order);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void CanValidateInvalidOrder()
        {
            var order = new OrderBuilder()
              .Build();

            var validator = new ValidatorBuilder()
              .WithInvalidResults()
              .Build();

            var result = validator.Validate(order);

            Assert.False(result.IsValid);
        }


    }
}