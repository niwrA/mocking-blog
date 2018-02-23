using MockingBlog;
using System.Linq;
using Xunit;

namespace MockingBlog.Tests
{
  [Trait("Category", "OrderValidatorFailTests")]
  public class OrderValidatorFailTests
  {
    [Fact]
    public void ValidOrder_IsValid()
    {
      var order = new Tests.ValidOrderBuilder().Build();
      var sut = new OrderValidator();
      var sutResult = sut.Validate(order);

      Assert.True(sutResult.IsValid);
      Assert.Empty(sutResult.ValidationErrors);
    }
    [Fact]
    public void ValidOrder_WhenNoOrderLines_InvalidatesCorrectly()
    {
      var order = new Tests.ValidOrderBuilder()
        .WithoutOrderLines()
        .Build();

      var sut = new OrderValidator();

      var sutResult = sut.Validate(order);

      Assert.False(sutResult.IsValid);
      Assert.Single(sutResult.ValidationErrors);
      Assert.Equal(ValidationErrorTypes.OrderlinesRequired, sutResult.ValidationErrors.First().Type);
    }
    [Fact]
    public void ValidOrder_WhenNoShippingAddress_InvalidatesCorrectly()
    {
      var order = new Tests.ValidOrderBuilder()
        .WithoutShippingAddress()
        .Build();

      var sut = new OrderValidator();

      var sutResult = sut.Validate(order);

      Assert.False(sutResult.IsValid);
      Assert.Single(sutResult.ValidationErrors);
      Assert.Equal(ValidationErrorTypes.ShippingAddressRequired, sutResult.ValidationErrors.First().Type);
    }
    [Fact]
    public void ValidOrder_WhenMultipleErrors_ReportsMultipleErrors()
    {
      var order = new Tests.ValidOrderBuilder()
        .WithoutShippingAddress()
        .WithoutOrderLines()
        .Build();

      var sut = new OrderValidator();

      var sutResult = sut.Validate(order);

      Assert.False(sutResult.IsValid);
      Assert.Equal(2, sutResult.ValidationErrors.Count);

      Assert.NotNull(sutResult.ValidationErrors.First(f => f.Type == ValidationErrorTypes.ShippingAddressRequired));
      Assert.NotNull(sutResult.ValidationErrors.First(f => f.Type == ValidationErrorTypes.OrderlinesRequired));
    }
  }
}
