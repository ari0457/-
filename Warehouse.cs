class Warehouse
{
    public Product[] Products { get; set; }

    public double GetTotalStockValue()
    {
        double total = 0;
        for (int i = 0; i < Products.Length; i++)
        {
            total += Products[i].Price * Products[i].Quantity;
        }
        return total;
    }

    public Product FindMostExpensiveProduct()
    {
        Product mostExpensive = Products[0];
        for (int i = 1; i < Products.Length; i++)
        {
            if (Products[i].Price > mostExpensive.Price)
            {
                mostExpensive = Products[i];
            }
        }
        return mostExpensive;
    }
}
