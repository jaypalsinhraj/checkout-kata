

namespace Checkout.Service.Tests;

public class CheckoutTests
{
    ICheckoutService _checkoutService;
    IList<Product> _fakeProducts;
    IList<DiscountOnQty> _fakeDiscountPrices;

    public CheckoutTests()
    {
        _fakeProducts = new List<Product>
        {
            new Product { SKU = 'A', Price = 50 },
            new Product { SKU = 'B', Price = 30 },
            new Product { SKU = 'C', Price = 20 },
            new Product { SKU = 'D', Price = 15 }
        };

        _fakeDiscountPrices = new List<DiscountOnQty>
        {
            new DiscountOnQty { SKU = 'A', Quantity = 3, Price = 130 },
            new DiscountOnQty { SKU = 'B', Quantity = 2, Price = 45 }
        };

        _checkoutService = new CheckoutService(_fakeProducts, _fakeDiscountPrices);
    }

    private void fakeScanProduct(string products)
    {
        foreach (var product in products.ToCharArray())
        {
            _checkoutService.ScanProducts(product);
        }
    }

    [Fact]
    public void ReturnScannedProductWhenSKUScanned()
    {
        _checkoutService.ScanProducts('A');

        var scannedProducts = _checkoutService.GetScannedProducts();

        Assert.Equal('A', scannedProducts[0].SKU);
    }

    [Theory]
    [InlineData("A",50)]
    [InlineData("B", 30)]
    [InlineData("C", 20)]
    [InlineData("D", 15)]
    public void ReturnTotalWhenSingleProductIsScanned(string products, decimal expectedTotal)
    {
        fakeScanProduct(products);

        var total = _checkoutService.CaculateTotal();

        Assert.Equal(expectedTotal, total);
    }

    [Theory]
    [InlineData("AB", 80)]
    [InlineData("BC", 50)]
    [InlineData("CD", 35)]
    [InlineData("DA", 65)]
    public void ReturnTotalWhenTwoDifferentProductsAreScanned(string products, decimal expectedTotal)
    {
        fakeScanProduct(products);

        var total = _checkoutService.CaculateTotal();

        Assert.Equal(expectedTotal, total);
    }

    [Theory]
    [InlineData("AAA", 130)]
    [InlineData("BB", 45)]
    public void ReturnDiscountedTotalWhenProductsAligibleForDiscountAreScanned(string products, decimal expectedTotal)
    {
        fakeScanProduct(products);

        var total = _checkoutService.CaculateTotal();

        Assert.Equal(expectedTotal, total);
    }

    [Theory]
    [InlineData("",0)]
    [InlineData("A",50)]
    [InlineData("AA",100)]
    [InlineData("AAAA",180)]
    public void ReturnDiscountedTotalWhenProductsAreScannedInVariousOrder(string products, decimal expectedTotal)
    {
        fakeScanProduct(products);

        var total = _checkoutService.CaculateTotal();

        Assert.Equal(expectedTotal, total);
    }

}
