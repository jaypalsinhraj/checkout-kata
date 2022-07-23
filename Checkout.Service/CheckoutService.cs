namespace Checkout.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IList<Product> _products;
        private readonly IList<DiscountOnQty> _discountPrices;
        private IList<Product> _scannedProducts;

        public CheckoutService(IList<Product> products, IList<DiscountOnQty> discountPrices)
        {
            _products = products;
            _discountPrices = discountPrices;
            _scannedProducts = new List<Product>();
        }

        public void ScanProducts(char SKU)
        {
            var product = _products.SingleOrDefault(p => p.SKU == SKU);

            if(product != null)
            {
                _scannedProducts.Add(product);
            }
        }

        public IList<Product> GetScannedProducts()
        {
            return _scannedProducts;
        }

        private decimal calculateDiscount()
        {
            var scannedProductsGroup = _scannedProducts.GroupBy(p => p.SKU);
            decimal discount = 0;

            foreach(var scannedProduct in scannedProductsGroup)
            {
                var productQty = scannedProduct.Count();

                var discountItem = _discountPrices.SingleOrDefault(d => d.SKU == scannedProduct.Key);

                if (discountItem == null)
                    continue;

                var discountQty = discountItem.Quantity;
                var discountPrice = discountItem.Price;

                if (productQty < discountQty)
                    continue;

                discount += (productQty / discountQty) * discountPrice;
                discount += (productQty % discountQty) * (_products.Single(p => p.SKU == scannedProduct.Key)?.Price ?? 0);
            }

            return discount;
        }

        public decimal CaculateTotal()
        {
            var total = _scannedProducts.Sum(p => p.Price);
            var discountedPrice = calculateDiscount();

            return discountedPrice <= 0 ? total : discountedPrice;
        }

    }
}