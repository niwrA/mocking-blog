using MockingBlog;
using Xunit;

namespace MockingBlog.Tests
{
  [Trait("Category", "OrderValidator")]
  public class OrderValidatorTests
  {
    // This is a weak test setup. The test will fail as soon as a new validation requirement
    // is added, and the builder setup will have to be amended to get all the valid components
    // included for the order validation to succeed.

    [Fact(Skip = "This test setup is not solid")]
    public void OrderMustHaveOneOrderLine()
    {
      var emptyOrder = new Tests.OrderBuilder().Build();
      var sut = new OrderValidator();
      var sutInvalidResult = sut.Validate(emptyOrder);

      Assert.False(sutInvalidResult.IsValid);

      var validOrder = new Tests.OrderBuilder()
        .WithOrderLine()
        .Build();

      var sutValidResult = sut.Validate(validOrder);

      Assert.True(sutValidResult.IsValid);
    }
  }
}
