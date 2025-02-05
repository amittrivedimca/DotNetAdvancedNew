using Application;
using Application.ProductAL;
using CatalogRestAPI.ViewModels;
using HelperUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogRestAPI.Controllers
{
    /// <summary>
    /// Product api
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IApplicationManager _appManager;
        public ProductController(IApplicationManager applicationManager)
        {
            _appManager = applicationManager;
        }


        /// <summary>
        /// Get all 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("{categoryID}/{pageNumber?}/{pageSize?}")]
        //[Authorize(Roles = "Manager,StoreCustomer", AuthenticationSchemes = "MyAuth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<PagedList<ProductShortInfoDTO>>>> GetAll([FromRoute]int categoryID, 
            [FromRoute] int pageNumber=0, [FromRoute] int pageSize=0)
        {
            ProductsFilter filter = new ProductsFilter()
            {
                CategoryID = categoryID,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            ApiResponse<PagedList<ProductShortInfoDTO>> apiResponse = new ApiResponse<PagedList<ProductShortInfoDTO>>();
            apiResponse.Data = await _appManager.ProductProvider.GetAllAsync(filter);

            apiResponse.Links.Add(new Link() { 
                 Href = "/GetAll/" + filter.CategoryID + "/" + (filter.PageNumber + 1) + "/" + filter.PageSize,
                 Type="GET",
                 Rel = "get_next_page"
            });

            apiResponse.Links.Add(new Link()
            {
                Href = this.Url.ActionLink("Add") ?? "",
                Type = "POST",
                Rel = "add_product"
            });

            return Ok(apiResponse);
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet()]
        //[Authorize(Roles = "Manager,StoreCustomer", AuthenticationSchemes = "MyAuth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<ProductDTO>>> GetById(int id)
        {
            var result = await _appManager.ProductProvider.GetById(id);
            if (result.Item1 == DBOperationStatus.NotFound)
            {
                return NotFound(result.Item1.ToString());
            }

            ApiResponse<ProductDTO> apiResponse = new ApiResponse<ProductDTO>();
            apiResponse.Data = result.Item2;
            apiResponse.Links.Add(new Link()
            {
                Href = this.Url.ActionLink("GetById") ?? "",
                Type = "GET",
                Rel = "self"
            });

            apiResponse.Links.Add(new Link()
            {
                Href = this.Url.ActionLink("Add") ?? "",
                Type = "POST",
                Rel = "add_product"
            });

            apiResponse.Links.Add(new Link()
            {
                Href = this.Url.ActionLink("Update") ?? "",
                Type = "PUT",
                Rel = "update_product"
            });

            apiResponse.Links.Add(new Link()
            {
                Href = this.Url.ActionLink("Delete") ?? "",
                Type = "Delete",
                Rel = "delete_product"
            });

            return Ok(apiResponse);
        }

        /// <summary>
        /// Add product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost()]
        [Authorize(Roles = "Manager", AuthenticationSchemes = "MyAuth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<string>> Add(ProductDTO product)
        {
            try
            {
                DBOperationStatus status = await _appManager.ProductProvider.AddAsync(product);
                return Ok(new
                {
                    Id = product.ID,
                    Status = status.ToString()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut()]
        [Authorize(Roles = "Manager", AuthenticationSchemes = "MyAuth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<string>> Update(ProductDTO product)
        {
            try
            {
                var status = await _appManager.ProductProvider.UpdateAsync(product);
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

        /// <summary>
        /// Delete product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete()]
        [Authorize(Roles = "Manager", AuthenticationSchemes = "MyAuth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<string>> Delete(int id)
        {
            try
            {
                var status = await _appManager.ProductProvider.DeleteAsync(id);
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
    }
}
