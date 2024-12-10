using Application;
using Application.CategoryAL;
using HelperUtils;
using Microsoft.AspNetCore.Mvc;

namespace CatalogRestAPI.Controllers
{
    /// <summary>
    /// Category Api
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoryController : ControllerBase
    {
        private readonly IApplicationManager _appManager;
        public CategoryController(IApplicationManager applicationManager)
        {
            _appManager = applicationManager;
        }

        /// <summary>
        /// Get All Category
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAll")]
        public ActionResult<IEnumerable<CategoryDTO>> GetAll()
        {
            return Ok(_appManager.CategoryProvider.GetAll());
        }

        /// <summary>
        /// Get Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Category data</returns>
        /// <remarks>
        /// Sample request:
        ///     GET /GetById
        /// </remarks>
        /// <response code="200">Success</response>
        /// <response code="404">Category not found</response>
        [HttpGet(Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var result = await _appManager.CategoryProvider.GetById(id);
            if (result.Item1 == DBOperationStatus.NotFound)
            {
                return NotFound(result.Item1.ToString());
            }
            return Ok(result.Item2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Internal Error</response>
        [HttpPost(Name = "AddAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> AddAsync(CategoryDTO category)
        {
            try
            {
                return (await _appManager.CategoryProvider.AddAsync(category)).ToString();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Category not found</response>
        /// <response code="400">Internal Error</response>
        [HttpPut(Name = "UpdateAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> UpdateAsync(CategoryDTO category)
        {
            try
            {
                var status = await _appManager.CategoryProvider.UpdateAsync(category);
                if (status == DBOperationStatus.NotFound)
                {
                    return NotFound(status.ToString());
                }
                return status.ToString();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete(Name = "DeleteAsync")]
        public async Task<ActionResult<string>> DeleteAsync(int id)
        {
            try
            {
                var status = await _appManager.CategoryProvider.DeleteAsync(id);
                if(status == DBOperationStatus.NotFound)
                {
                    return NotFound(status.ToString());
                }
                return status.ToString();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }

}
