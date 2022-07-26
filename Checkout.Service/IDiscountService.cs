﻿namespace Checkout.Service;

public interface IDiscountService
{
    IList<DiscountOnQty> GetDiscountPrices();
    decimal GetDiscountedPrice(IList<Product> scannedProducts);
}
