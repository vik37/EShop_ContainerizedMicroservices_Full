namespace Orders.UnitTest.Application;

public class Test_OrdersPaggination
{
    private readonly List<AdminOrderSummaryViewModel> _adminOrderSummaryViewModel;

    public Test_OrdersPaggination()
    {
        _adminOrderSummaryViewModel = new List<AdminOrderSummaryViewModel>
        {
            new AdminOrderSummaryViewModel
            {
                Status = "Test Status",
                OrderNumber = 1,
                OrderDate = DateTime.Now,
                Description = "Test Description",
                BuyerName = "Test Name",
                Total = 12
            },
            new AdminOrderSummaryViewModel
            {
                Status = "Test Status",
                OrderNumber = 2,
                OrderDate = DateTime.Now,
                Description = "Another Description",
                BuyerName = "Another Name",
                Total = 5
            },
             new AdminOrderSummaryViewModel
            {
                Status = "Test Status",
                OrderNumber = 1,
                OrderDate = DateTime.Now,
                Description = "Test Description",
                BuyerName = "Test Name",
                Total = 12
            },
            new AdminOrderSummaryViewModel
            {
                Status = "Test Status",
                OrderNumber = 2,
                OrderDate = DateTime.Now,
                Description = "Another Description",
                BuyerName = "Another Name",
                Total = 5
            },
             new AdminOrderSummaryViewModel
            {
                Status = "Test Status",
                OrderNumber = 1,
                OrderDate = DateTime.Now,
                Description = "Test Description",
                BuyerName = "Test Name",
                Total = 12
            },
            new AdminOrderSummaryViewModel
            {
                Status = "Test Status",
                OrderNumber = 2,
                OrderDate = DateTime.Now,
                Description = "Another Description",
                BuyerName = "Another Name",
                Total = 5
            }
        };
    }

    [Fact]
    public void Test_PagginationObject_PageSizeShouldBeEqualWithDataCount()
    {
        // Assert
        int pageSize = 3;
        int currentPage = 2;

        // Action
        var paggination = new Paggination<AdminOrderSummaryViewModel>(currentPage, _adminOrderSummaryViewModel, pageSize);

        // Arrange
        paggination.Data.Should().NotBeNull();
        paggination.Data.Should().HaveCount(pageSize);
    }

    [Fact]
    public void Test_PagginationObject_TotalItemsShouldBeEqualWithFullListOfDataCount()
    {
        // Assert
        int pageSize = 3;
        int currentPage = 2;

        // Action
        var paggination = new Paggination<AdminOrderSummaryViewModel>(currentPage, _adminOrderSummaryViewModel, pageSize);

        // Arrange
        paggination.TotalItems.Should().Be(_adminOrderSummaryViewModel.Count);
    }

    [Theory]
    [InlineData(2,1,3)]
    [InlineData(1, 1, 6)]
    [InlineData(3, 1, 2)]
    [InlineData(4, 1, 2)]
    public void Test_PagginationObject_TotalPagesSumShouldBeCorrect(int pageSize, int currentPage, int totalPagesExpectedSum)
    {
        var paggination = new Paggination<AdminOrderSummaryViewModel>(currentPage, _adminOrderSummaryViewModel, pageSize);

        paggination.TotalPages.Should().Be(totalPagesExpectedSum);
    }

    [Theory]
    [InlineData(1, 3, 2)]
    [InlineData(2, 6, 10)]
    public void Test_PagginationObject_StartItemSumShouldBeCorrect(int pageSize, int currentPage, int startItemExpectedSum)
    {
        var paggination = new Paggination<AdminOrderSummaryViewModel>(currentPage, _adminOrderSummaryViewModel, pageSize);

        paggination.StartItem.Should().Be(startItemExpectedSum);
    }

    [Theory]
    [InlineData(1,2,2)]
    [InlineData(2, 2, 4)]
    [InlineData(2, 7, 6)]
    [InlineData(9, 10, 6)]
    public void Test_PagginationObject_EndItemSumShouldBeCorrect(int pageSize, int currentPage, int endItemExpectedSum)
    {

        var paggination = new Paggination<AdminOrderSummaryViewModel>(currentPage, _adminOrderSummaryViewModel, pageSize);

        paggination.EndItem.Should().Be(endItemExpectedSum);
    }

    [Fact]
    public void Test_PagginationObject_HasPrevoiusShouldBeTrue()
    {
        // Assert

        int pageSize = 2;
        int currentPage = 2;

        // Action

        var paggination = new Paggination<AdminOrderSummaryViewModel>(currentPage, _adminOrderSummaryViewModel, pageSize);

        // Arrange
        Assert.True(paggination.HasPrevious);
    }

    [Fact]
    public void Test_PagginationObject_HasPrevoiusShouldBeFalse()
    {
        // Assert
        int pageSize = 2;
        int currentPage = 1;

        // Action
        var paggination = new Paggination<AdminOrderSummaryViewModel>(currentPage, _adminOrderSummaryViewModel, pageSize);

        // Arrange
        Assert.False(paggination.HasPrevious);
    }

    [Fact]
    public void Test_PagginationObject_HasNextShouldBeTrue()
    {
        // Assert
        int pageSize = 2;
        int currentPage = 2;

        // Action
        var paggination = new Paggination<AdminOrderSummaryViewModel>(currentPage, _adminOrderSummaryViewModel, pageSize);

        // Arrange
        Assert.True(paggination.HasNext);
    }

    [Fact]
    public void Test_PagginationObject_HasNextShouldBeFalse()
    {
        // Assert
        int pageSize = 3;
        int currentPage = 6;

        // Action
        var paggination = new Paggination<AdminOrderSummaryViewModel>(currentPage, _adminOrderSummaryViewModel, pageSize);

        // Arrange
        Assert.False(paggination.HasNext);
    }
}