namespace Checkout.Service;

public interface ICheckoutService
{
    IList<Product> ScanProducts(char SKU);
    decimal CaculateTotal();
}
