namespace Checkout.Service;

public class DiscountService : IDiscountService
{
    private readonly IList<DiscountOnQty> _discountPrices;

    public DiscountService(IList<DiscountOnQty> discountPrices)
    {
        _discountPrices = discountPrices;
    }

    public decimal GetDiscountedPrice(IList<Product> scannedProducts)
    {
        var scannedProductsGroup = scannedProducts.GroupBy(p => p.SKU);
        decimal discount = 0;

        foreach (var scannedProduct in scannedProductsGroup)
        {
            var SKU = scannedProduct.Key;
            var productQty = scannedProduct.Count();

            var discountItem = _discountPrices.SingleOrDefault(d => d.SKU == SKU);
            if (discountItem == null)
                continue;

            var discountQty = discountItem.Quantity;
            var discountPrice = discountItem.Price;

            if (productQty < discountQty)
                continue;

            discount += (productQty / discountQty) * discountPrice;
            discount += (productQty % discountQty) * (scannedProducts.Distinct()?.SingleOrDefault(p => p.SKU == SKU)?.Price ?? 0);
        }

        return discount;
    }

    public IList<DiscountOnQty> GetDiscountPrices()
    {
        return _discountPrices;
    }
}
