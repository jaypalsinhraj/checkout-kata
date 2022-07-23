namespace Checkout.Service;

public interface ICheckoutService
{
    void ScanProducts(char SKU);
    IList<Product> GetScannedProducts();
    decimal GetTotalPrice();
}
