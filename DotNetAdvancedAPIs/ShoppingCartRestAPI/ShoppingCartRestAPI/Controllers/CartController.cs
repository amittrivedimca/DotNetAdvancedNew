using Application;
using Application.CartAL;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCartRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class CartController : ControllerBase
    {
        private readonly IApplicationManager _applicationManager;
        public CartController(IApplicationManager applicationManager)
        {
            _applicationManager = applicationManager;
        }

        /// <summary>
        /// Get Cart by id
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [HttpGet("GetCartInfo/{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiVersion("1.0")]
        [Authorize(Roles = "Manager,StoreCustomer", AuthenticationSchemes = "MyAuth")]
        public async Task<ActionResult<CartDTO>> GetCartInfo([FromRoute]string cartId)
        {
            var cart = await _applicationManager.CartProvider.GetCart(cartId);
            if (cart != null)
            {
                return Ok(cart);
            }
            return NotFound();
        }

        /// <summary>
        /// v2 - Get Cart by id
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [HttpGet("GetCartItems/{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiVersion("2.0")]
        [Authorize(Roles = "Manager,StoreCustomer", AuthenticationSchemes = "MyAuth")]
        public async Task<ActionResult<CartDTO>> GetCartItemsInfo([FromRoute] string cartId)
        {
            var cart = await _applicationManager.CartProvider.GetCart(cartId);
            if (cart != null)
            {
                return Ok(cart.CartItems);
            }
            return NotFound();
        }

        /// <summary>
        /// Get Cart Items by CartId
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [HttpGet("GetCartItems/{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Manager,StoreCustomer", AuthenticationSchemes = "MyAuth")]
        public async Task<ActionResult<List<CartItemDTO>>> GetCartItems([FromRoute]string cartId)
        {
            var items = await _applicationManager.CartProvider.GetCartItems(cartId);
            if (items != null)
            {
                return Ok(items);
            }
            return NotFound();
        }

        /// <summary>
        /// Add Cart Item. Create new cart if not exists.
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("AddCartItem/{cartId}")]
        [Authorize(Roles = "Manager,StoreCustomer", AuthenticationSchemes = "MyAuth")]
        public ActionResult AddCartItem([FromRoute]string cartId,CartItemDTO cartItem)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Invalid input data!");
            }

            try
            {
                var dto = _applicationManager.CartProvider.AddOrUpdateItem(cartId, cartItem);
                return Ok(new
                {
                    cartId = dto.CartId
                });
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete cart item
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("RemoveCartItem")]
        [Authorize(Roles = "Manager,StoreCustomer", AuthenticationSchemes = "MyAuth")]
        public ActionResult RemoveCartItem([FromQuery] string cartId, [FromQuery] int itemId)
        {
            try
            {
                _applicationManager.CartProvider.RemoveItem(cartId, itemId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Receive And Process ProductChange Messages to update cart
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReceiveAndProcessProductChangeMessages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Manager,StoreCustomer", AuthenticationSchemes = "MyAuth")]
        public async Task<ActionResult> ReceiveAndProcessProductChangeMessages()
        {
            try
            {
                IEnumerable<string> cartIds = await _applicationManager.CartProvider.ReceiveAndProcessProductChangeMessages();
                return Ok(new { updatedCartIds = cartIds });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex);
            }
        }

        /// <summary>
        /// Get Item Info
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetItemInfo/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ItemDTO GetItemInfo(int id)
        {
            return new ItemDTO()
            {
                ItemId = id,
                ItemImage = null,
                Name = "Item " + id,
                Price = 10,
                Quantity = 100
            };
        }
    }
}
