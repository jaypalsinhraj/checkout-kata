namespace Checkout.Service;

public interface IPricingService
{
    decimal GetDiscountedPrice(IList<Product> scannedProducts);
    decimal GetTotalPrice(IList<Product> scannedProducts);
}
