using WebApplication1.DTO;

namespace WebApplication1.Models.ViewModel
{
    public class CategoryProductVM
    {
        public List<CategoryDTO> CategoryVMs { get; set; } = new List<CategoryDTO>();

        public List<ProductDTO> ProductVMs { get; set; } = new List<ProductDTO>();
    }
}
