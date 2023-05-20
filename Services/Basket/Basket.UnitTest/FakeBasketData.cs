namespace Basket.UnitTest;

public static class FakeBasketData
{
    public static List<CustomerBasket?> GetFakeBasketItems()
    {
        var cb = new List<CustomerBasket?>();
        cb.Add(new CustomerBasket()
        {
            BuyerId = "1",
            Items = new List<BasketItem>
            {
                new BasketItem()
                {
                    Id = 1,
                    ProductName = "test-1",
                    UnitPrice = 10
                },
                new BasketItem()
                {
                    Id = 3,
                    ProductName = "test-3",
                    UnitPrice = 20
                },
                new BasketItem()
                {
                    Id = 4,
                    ProductName = "test-3",
                    UnitPrice = 20
                }
            } 
        });
        cb.Add(new CustomerBasket
        {
            BuyerId = "2",
            Items = new List<BasketItem>
            {
                new BasketItem
                {
                    Id = 2,
                    ProductName = "test-2",
                    UnitPrice = 20
                }
            }
        });

        return cb;
    }
}



