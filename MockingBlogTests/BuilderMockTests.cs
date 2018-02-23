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
    public void CanValidateOrder()
    {
      var order = new OrderBuilder()
        .WithOrderLine()
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