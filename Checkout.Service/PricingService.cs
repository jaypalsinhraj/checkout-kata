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

    public decimal GetNonDiscountedPrice(IList<Product> scannedProducts)
    {
        if (scannedProducts == null || !scannedProducts.Any())
            return 0;

        var total = getScannedProductsWithoutDiscount(scannedProducts)?.Sum(x => x.Price) ?? 0;

        foreach (var discountItem in getDiscountedItemsNotApplicabelToThisCart(scannedProducts))
        {
            var discountQty = discountItem?.Quantity ?? 0;
            var products = scannedProducts.Where(p => p.SKU == discountItem?.SKU);
            var productQty = products?.Count() ?? 0;

            if (productQty < discountQty)
                total += products?.Sum(p => p.Price) ?? 0;
        }

        return total;

    }


    private IEnumerable<Product> getScannedProductsWithoutDiscount(IList<Product> scannedProducts)
    {
        return scannedProducts.Where(p => _discountPrices.All(d => d.SKU != p.SKU));
    }

    private IEnumerable<DiscountOnQty> getDiscountedItemsNotApplicabelToThisCart(IList<Product> scannedProducts)
    {
        return _discountPrices.Where(d => scannedProducts.Any(p => p.SKU == d.SKU));
    }
}
