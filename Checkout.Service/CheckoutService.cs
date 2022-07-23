namespace Checkout.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IList<Product> _products;

        public CheckoutService(IList<Product> products)
        {
            _products = products;
        }

        public IList<Product> ScanProducts(char SKU)
        {
            var _scannedProducts = new List<Product>();
            var product = _products.SingleOrDefault(p => p.SKU == SKU);

            if(product != null)
            {
                _scannedProducts.Add(product);
            }

            return _scannedProducts;
        }

        public decimal CaculateTotal(IList<Product> scannedProducts)
        {
            var total = scannedProducts.Sum(p => p.Price);

            return total;
        }

    }
}