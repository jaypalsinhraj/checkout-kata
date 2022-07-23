﻿namespace Checkout.Service
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

        public decimal CaculateTotal()
        {
            var total = _scannedProducts.Sum(p => p.Price);

            return total;
        }

    }
}