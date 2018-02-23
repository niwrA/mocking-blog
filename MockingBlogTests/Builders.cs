using MockingBlog;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockingBlog.Tests
{

    public class ValidatorBuilder
    {
        Mock<IOrderValidator> Mock = new Mock<IOrderValidator>();
        IValidationResults _validationResults = new ValidationResultsBuilder().Build();
        public IOrderValidator Build()
        {
            Mock.Setup(s => s.Validate(It.IsAny<IOrder>())).Returns(_validationResults);
            return Mock.Object;
        }
        public ValidatorBuilder WithValidationResults(IValidationResults results)
        {
            _validationResults = results;
            return this;
        }

        public ValidatorBuilder WithInvalidResults()
        {
            _validationResults = new ValidationResultsBuilder()
              .WithValidationError("Mock validation error")
              .Build();

            return this;
        }
    }
    public class ValidationResultsBuilder
    {
        Mock<IValidationResults> ValidationResultsMock = new Mock<IValidationResults>();
        ICollection<IValidationError> _validationErrors = new List<IValidationError>();
        public IValidationResults Build()
        {
            if (_validationErrors.Any())
            {
                ValidationResultsMock.Setup(s => s.IsValid).Returns(false);
                ValidationResultsMock.Setup(s => s.ValidationErrors).Returns(_validationErrors);
            }
            else
            {
                ValidationResultsMock.Setup(s => s.IsValid).Returns(true);
            }
            return ValidationResultsMock.Object;
        }
        public ValidationResultsBuilder WithValidationError(IValidationError error)
        {
            ((List<IValidationError>)_validationErrors).Add(error);
            return this;
        }
        public ValidationResultsBuilder WithValidationError(string message)
        {
            var error = new ValidationErrorBuilder()
              .WithMessage(message)
              .Build();

            ((List<IValidationError>)_validationErrors).Add(error);

            return this;
        }
    }
    public class ValidationErrorBuilder
    {
        Mock<IValidationError> mock = new Mock<IValidationError>();
        public IValidationError Build()
        {
            return mock.Object;
        }
        public ValidationErrorBuilder WithMessage(string message)
        {
            mock.Setup(s => s.Message).Returns(message);
            return this;
        }
    }
    public class OrderBuilder
    {
        Mock<IOrder> _mock = new Mock<IOrder>();
        IList<IOrderLine> _orderlines = new List<IOrderLine>();
        Mock<IShippingAddress> _shippingAddressMock;

        public IOrder Build()
        {
            _mock.Setup(s => s.OrderLines).Returns(_orderlines);
            return _mock.Object;
        }
        public OrderBuilder WithOrderLine()
        {
            _orderlines.Add(new OrderLineBuilder().Build());
            return this;
        }

        public OrderBuilder WithShippingAddress()
        {
            _shippingAddressMock = new Mock<IShippingAddress>();
            _mock.Setup(s => s.ShippingAddress).Returns(_shippingAddressMock.Object);
            return this;
        }
    }
    public class ValidOrderBuilder
    {
        Mock<IOrder> _mock = new Mock<IOrder>();
        IList<IOrderLine> _orderlines = new List<IOrderLine>();
        Mock<IShippingAddress> _shippingAddressMock = null;
        public ValidOrderBuilder()
        {
            _shippingAddressMock = new Mock<IShippingAddress>();
            _orderlines.Add(new OrderLineBuilder().Build());
        }
        public IOrder Build()
        {
            _mock.Setup(s => s.OrderLines).Returns(_orderlines);
            if (_shippingAddressMock != null)
            {
                _mock.Setup(s => s.ShippingAddress).Returns(_shippingAddressMock.Object);
            }
            return _mock.Object;
        }
        public ValidOrderBuilder WithoutOrderLines()
        {
            _orderlines.Clear();
            return this;
        }

        public ValidOrderBuilder WithoutShippingAddress()
        {
            _shippingAddressMock = null;
            return this;
        }
    }

    public class OrderLineBuilder
    {
        Mock<IOrderLine> OrderlineMock { get; set; } = new Mock<IOrderLine>();
        public IOrderLine Build()
        {
            return OrderlineMock.Object;
        }
    }
}
