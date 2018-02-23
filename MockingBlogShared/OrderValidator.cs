using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MockingBlog
{
    public class ValidationResults : IValidationResults
    {
        public bool IsValid => _validationErrors.Count() == 0;
        private ICollection<IValidationError> _validationErrors { get; } = new List<IValidationError>();

        IReadOnlyCollection<IValidationError> IValidationResults.ValidationErrors {
            get {
                return (IReadOnlyCollection<IValidationError>)_validationErrors;
            }
        }
        public void AddError(ValidationErrorTypes type, string message)
        {
            var error = new ValidationError(type, message);
            _validationErrors.Add(error);
        }
    }

    internal class ValidationError : IValidationError
    {
        private ValidationErrorTypes type = ValidationErrorTypes.NotSet;
        private string message = "";

        public ValidationError(ValidationErrorTypes type, string message)
        {
            this.type = type;
            this.message = message;
        }

        public string Message { get { return message; } }

        public ValidationErrorTypes Type { get { return type; } }
    }

    public class OrderValidator : IOrderValidator
    {
        public IValidationResults Validate(IOrder order)
        {
            var validationResults = new ValidationResults();
            MustHaveOrderlines(order, validationResults);
            MustHaveShippingAddress(order, validationResults);
            return validationResults;
        }

        private void MustHaveOrderlines(IOrder order, IValidationResults results)
        {
            if (order.OrderLines.Count() < 1)
            {
                results.AddError(ValidationErrorTypes.OrderlinesRequired, "Must have at least one orderline.");
            }
        }
        private void MustHaveShippingAddress(IOrder order, IValidationResults results)
        {
            if (order.ShippingAddress == null)
            {
                results.AddError(ValidationErrorTypes.ShippingAddressRequired, "Must have a shipping address.");
            }
        }
    }
}
