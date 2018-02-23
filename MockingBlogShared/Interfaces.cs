using System;
using System.Collections.Generic;

namespace MockingBlog
{
  public interface IOrder
  {
    IReadOnlyCollection<IOrderLine> OrderLines { get; }
    IEnumerable<ICoupon> Coupons { get; }
    IShippingAddress ShippingAddress { get; }
  }
  public interface IValidationResults
  {
    bool IsValid { get; }
    IReadOnlyCollection<IValidationError> ValidationErrors { get; }
    void AddError(ValidationErrorTypes type, string message);
  }
  public enum ValidationErrorTypes
  {
    NotSet = 0,
    OrderlinesRequired = 1,
    ShippingAddressRequired = 2,
  }
  public interface IValidationError
  {
    string Message { get; }
    ValidationErrorTypes Type { get; }
  }

  public interface IOrderValidator
  {
    IValidationResults Validate(IOrder order);
  }
  public interface IOrderLine
  {
    IProduct Product { get; }
    IDiscount Discount { get; }
  }

  public interface IShippingAddress
  {
      string City { get; }
  }

  public interface IProduct
  {
    string Name { get; set; }
    IValue Price { get; set; }
  }

  public interface IDiscount
  {
    IPercentage Percentage { get; }
  }

  public interface IPercentage
  {
    int Percentage { get; set; }
  }

  public interface ICoupon
  {
    IValue Value { get; }
  }
  public enum CurrencyType
  {
    EUR,
    USD,
    YEN
  }
  public interface IValue
  {
    decimal Value { get; }
    CurrencyType Currency { get; }
  }
}
