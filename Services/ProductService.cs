using Campaign.Models;

namespace Campaign.Services;

public interface IProductService
{
    List<Product> List();
    string GetProductInfo(string code);
    Product? GetProduct(string productCode);
    string CreateProduct(string code, decimal price, int stock);
}

public class ProductService : IProductService
{
    private static readonly List<Product> Products = new();
    
    public Product? GetProduct(string productCode)
    {
        return Products.FirstOrDefault(a => a.Code == productCode);
    }
    
    public string CreateProduct(string code, decimal price, int stock)
    {
        Products.Add(new Product { Code = code, Price = price, MainPrice = price, Stock = stock });
        var msg = $"Product created; code {code}, price {price}, stock {stock}";
        Console.WriteLine(msg);
        return msg;
    }

    public List<Product> List()
    {
        return Products;
    }

    public string GetProductInfo(string code)
    {
        var product = Products.FirstOrDefault(p => p.Code == code);
        var msg = product != null
            ? $"Product {code} info; price {product.Price}, stock {product.Stock}"
            : $"Product {code} not found";
        Console.WriteLine(msg);
        return msg;
    }
}