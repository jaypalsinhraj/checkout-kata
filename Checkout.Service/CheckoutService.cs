namespace Checkout.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IList<Product> _products;
        private readonly IList<DiscountOnQty> _discountPrices;
        private readonly IDiscountService _discountService;
        private readonly IPricingService _pricingService;
        private IList<Product> _scannedProducts;

        public CheckoutService(IList<Product> products, IList<DiscountOnQty> discountPrices, IDiscountService discountService, IPricingService pricingService)
        {
            _products = products;
            _discountPrices = discountPrices;
            _discountService = discountService;
            _pricingService = pricingService;
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

        public decimal GetTotalPrice()
        {
            var scannedProductsWhereDiscountNotApplicable = calculateTotalWhereDiscountNotApplicable();
            var scannedProductsTotalWithoutDiscount = calculateTotalForProductsWithNoDiscount();
            var discountedPrice = _discountService.GetDiscountedPrice(_scannedProducts);

            var totalDiscount = _pricingService.GetDiscountedPrice(_scannedProducts);
            var totalPrice = _pricingService.GetTotalPrice(_scannedProducts);

            return totalDiscount + totalPrice;
        }

        private decimal calculateTotalForProductsWithNoDiscount()
        {
            var scannedProductsWithoutDiscount = _scannedProducts.Where(p => _discountPrices.All(d => d.SKU != p.SKU));
            return scannedProductsWithoutDiscount?.Sum(x => x.Price) ?? 0;
        }

        private decimal calculateTotalWhereDiscountNotApplicable()
        {
            decimal total = 0;

            foreach (var discountItem in _discountPrices.Where(d => _scannedProducts.Any(p => p.SKU == d.SKU)))
            {
                var discountQty = discountItem?.Quantity ?? 0;
                var products = _scannedProducts.Where(p => p.SKU == discountItem?.SKU);
                var productQty = products?.Count() ?? 0;

                if (productQty < discountQty)
                    total += products?.Sum(p => p.Price) ?? 0;
            }

            return total;
        }
    }
}