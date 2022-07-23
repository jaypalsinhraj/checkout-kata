

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
}
