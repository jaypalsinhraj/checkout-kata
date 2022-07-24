namespace Checkout.Service;

public class PricingService : IPricingService
{
    private readonly IDiscountService _discountService;
    private readonly IList<DiscountOnQty> _discountPrices;

    public PricingService(IDiscountService discountService)
    {
        _discountService = discountService;
        _discountPrices = discountService.GetDiscountPrices();
    }

    public decimal GetDiscountedPrice(IList<Product> scannedProducts)
    {
        return _discountService.GetDiscountedPrice(scannedProducts);
    }

    public decimal GetTotalPrice(IList<Product> scannedProducts)
    {
        return calculateTotalForProductsWithNoDiscount(scannedProducts) + calculateTotalWhereDiscountNotApplicable(scannedProducts);
    }


    private decimal calculateTotalForProductsWithNoDiscount(IList<Product> scannedProducts)
    {
        var scannedProductsWithoutDiscount = scannedProducts.Where(p => _discountPrices.All(d => d.SKU != p.SKU));
        return scannedProductsWithoutDiscount?.Sum(x => x.Price) ?? 0;
    }

    private decimal calculateTotalWhereDiscountNotApplicable(IList<Product> scannedProducts)
    {
        decimal total = 0;

        foreach (var discountItem in _discountPrices.Where(d => scannedProducts.Any(p => p.SKU == d.SKU)))
        {
            var discountQty = discountItem?.Quantity ?? 0;
            var products = scannedProducts.Where(p => p.SKU == discountItem?.SKU);
            var productQty = products?.Count() ?? 0;

            if (productQty < discountQty)
                total += products?.Sum(p => p.Price) ?? 0;
        }

        return total;
    }

}
