using DataEntities;

namespace Store.Services;

internal sealed class ProductService(HttpClient httpClient, ILogger<ProductService> logger)
{
    public async Task<List<Product>> GetProductsAsync()
    {
        List<Product>? products = null;

        try
        {
            var response = await httpClient.GetAsync("/api/Product");
            var responseText = await response.Content.ReadAsStringAsync();

            logger.LogInformation("Http status code: {StatusCode}", response.StatusCode);
            logger.LogInformation("Http response content: {ResponseText}", responseText);

            if (response.IsSuccessStatusCode)
            {
                products = await response.Content.ReadFromJsonAsync(ProductSerializerContext.Default.ListProduct);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during GetProducts.");
        }

        return products ?? [];
    }
}
