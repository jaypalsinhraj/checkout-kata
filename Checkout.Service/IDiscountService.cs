namespace Checkout.Service;

public interface IDiscountService
{
    decimal GetDiscountedPrice(IList<Product> scannedProducts);
}
