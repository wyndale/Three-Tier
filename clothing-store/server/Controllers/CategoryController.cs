using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] CategoryDTO categoryDto)
    {
        await _categoryService.AddCategoryAsync(categoryDto);

        return CreatedAtAction(nameof(GetAllCategories), new { Name = categoryDto.Name });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryDTO categoryDto)
    {
        var existingCategory = await _categoryService.GetCategoryByIdAsync(id);

        await _categoryService.UpdateCategoryAsync(categoryDto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var existingCategory = await _categoryService.GetCategoryByIdAsync(id);

        await _categoryService.DeleteCategoryAsync(id);

        return NoContent();
    }
}

