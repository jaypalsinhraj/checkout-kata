namespace Checkout.Service;

public interface IPricingService
{
    decimal GetDiscountedPrice(IList<Product> scannedProducts);
    decimal GetNonDiscountedPrice(IList<Product> scannedProducts);
}
