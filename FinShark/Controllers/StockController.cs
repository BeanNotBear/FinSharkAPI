using FinShark.Data;
using FinShark.Dtos.Stock;
using FinShark.Interfaces;
using FinShark.mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Controllers
{
    // This attribute marks this class as an API controller, enabling automatic model validation and routing.
    [ApiController]
    // This attribute specifies the route template for the controller, setting the base route to "api/stock".
    [Route("api/stock")]
    public class StockController : ControllerBase
    {
        // Dependencies injected via constructor injection.
        private readonly ApplicationDBContext _dbContext;
        private readonly IStockRepository _stockRepository;

        // Constructor to initialize the controller with the necessary dependencies.
        public StockController(ApplicationDBContext context, IStockRepository stockRepository)
        {
            _dbContext = context;
            _stockRepository = stockRepository;
        }

        // HTTP GET method to retrieve all stock records.
        [HttpGet] // same with [HttpRead]
        public async Task<IActionResult> GetAll()
        {
            // Fetches all stock entities asynchronously from the repository.
            var stocks = await _stockRepository.GettAllAsync();

            // Maps the stock entities to their corresponding DTOs.
            var stockDtos = stocks.Select(s => s.ToStockDto());

            // Returns the stock DTOs with a 200 OK status.
            return Ok(stockDtos); // Corrected to return DTOs instead of entities directly.
        }

        // HTTP GET method to retrieve a stock record by its ID.
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            // Fetches the stock entity by ID asynchronously from the repository.
            var stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null)
            {
                // Returns a 404 Not Found status if the stock entity does not exist.
                return NotFound();
            }
            else
            {
                // Maps the stock entity to its corresponding DTO and returns it with a 200 OK status.
                return Ok(stock.ToStockDto());
            }
        }

        // HTTP POST method to create a new stock record.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            // Maps the CreateStockRequestDto to a stock entity.
            var stockModel = stockDto.ToStockFromCreateStockRequestDto();

            // Saves the new stock entity asynchronously to the repository.
            await _stockRepository.CreateAsync(stockModel);

            // Returns a 201 Created status with the URI of the newly created stock entity.
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel);
        }

        // HTTP PUT method to update an existing stock record.
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            // Updates the stock entity asynchronously in the repository.
            var stockModel = await _stockRepository.UpdateAsync(id, stockDto);

            if (stockModel == null)
            {
                // Returns a 404 Not Found status if the stock entity does not exist.
                return NotFound();
            }

            // Maps the updated stock entity to its corresponding DTO and returns it with a 200 OK status.
            return Ok(stockModel.ToStockDto());
        }

        // HTTP DELETE method to delete an existing stock record.
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // Deletes the stock entity asynchronously from the repository.
            var stockModel = await _stockRepository.DeleteAsync(id);

            if (stockModel == null)
            {
                // Returns a 404 Not Found status if the stock entity does not exist.
                return NotFound();
            }

            // Returns a 204 No Content status indicating successful deletion.
            return NoContent();
        }
    }
}
