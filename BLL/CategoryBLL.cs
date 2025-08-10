using DTOs;

namespace BusinessLogicLayer;

public class CategoryBLL
{
    DAL.CategoryDAL dll = new DAL.CategoryDAL();

    public int AddCategory(string categoryName)
    {
        return dll.AddCategory(categoryName);
    }

    public int DeleteCategory(int id)
    {
        return dll.DeleteCategory(id);
    }

    public List<DTO.CategoryDTO> GetAllCategories()
    {
        return dll.GetAllCategories();
    }
}