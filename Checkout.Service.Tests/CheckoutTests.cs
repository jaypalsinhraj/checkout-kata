

namespace Checkout.Service.Tests;

public class CheckoutTests
{
    ICheckoutService _checkoutService;
    IList<Product> _fakeProducts;

    public CheckoutTests()
    {
        _fakeProducts = new List<Product>
        {
            new Product { SKU = 'A', Price = 50 },
            new Product { SKU = 'B', Price = 30 },
            new Product { SKU = 'C', Price = 20 },
            new Product { SKU = 'D', Price = 15 }
        };

        _checkoutService = new CheckoutService(_fakeProducts);
    }

    [Fact]
    public void ReturnScannedProductWhenSKUScanned()
    {
        var scannedProducts = _checkoutService.ScanProducts('A');

        Assert.Equal('A', scannedProducts[0].SKU);
    }

    [Theory]
    [InlineData("A",50)]
    [InlineData("B", 30)]
    [InlineData("C", 20)]
    [InlineData("D", 15)]
    public void ReturnTotalWhenSingleProductIsScanned(string products, decimal expectedTotal)
    {
        IList<Product> fakeScannedProducts = new List<Product>();
        foreach(var product in products)
        {
            fakeScannedProducts = _checkoutService.ScanProducts(product);
        }

        var total = _checkoutService.CaculateTotal(fakeScannedProducts);

        Assert.Equal(expectedTotal, total);
    }
}
