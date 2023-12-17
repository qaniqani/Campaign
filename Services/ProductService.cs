using Campaign.Models;

namespace Campaign.Services;

public interface IProductService
{
    List<Product> List();
    void GetProductInfo(string code);
    Product? GetProduct(string productCode);
    void CreateProduct(string code, decimal price, int stock);
}

public class ProductService : IProductService
{
    private static readonly List<Product> Products = new();
    
    public Product? GetProduct(string productCode)
    {
        return Products.FirstOrDefault(a => a.Code == productCode);
    }
    
    public void CreateProduct(string code, decimal price, int stock)
    {
        Products.Add(new Product { Code = code, Price = price, MainPrice = price, Stock = stock });
        Console.WriteLine($"Product created; code {code}, price {price}, stock {stock}");
    }

    public List<Product> List()
    {
        return Products;
    }

    public void GetProductInfo(string code)
    {
        var product = Products.FirstOrDefault(p => p.Code == code);
        Console.WriteLine(product != null
            ? $"Product {code} info; price {product.Price}, stock {product.Stock}"
            : $"Product {code} not found");
    }
}