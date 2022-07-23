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

        private decimal calculateTotalForProductsWithNoDiscount()
        {
            var scannedProductsWithoutDiscount = _scannedProducts.Where(p => !_discountPrices.Any(d => d.SKU == p.SKU));
            return scannedProductsWithoutDiscount.Sum(x => x.Price);
        }

        private decimal calculateTotalWhereDiscountNotApplicable()
        {
            var discountItems = _discountPrices.Where(d => _scannedProducts.Any(p => p.SKU == d.SKU));
            
            decimal total = 0;
            
            foreach(var discountItem in discountItems)
            {
                var discountQty = discountItem?.Quantity ?? 0;
                var products = _scannedProducts.Where(p => p.SKU == discountItem?.SKU);
                var productQty = products.Count();

                if (productQty < discountQty)
                    total += products.Sum(p => p.Price);
            }

            return total;
        }


        public decimal CaculateTotal()
        {
            var scannedProductsWhereDiscountNotApplicable = calculateTotalWhereDiscountNotApplicable();
            var scannedProductsTotalWithoutDiscount = calculateTotalForProductsWithNoDiscount();
            var discountedPrice = calculateDiscount();

            return scannedProductsWhereDiscountNotApplicable + scannedProductsTotalWithoutDiscount + discountedPrice;
        }

    }
}