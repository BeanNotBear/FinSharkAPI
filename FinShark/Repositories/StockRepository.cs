using FinShark.Data;
using FinShark.Dtos.Stock;
using FinShark.Interfaces;
using FinShark.mappers;
using FinShark.Models;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Repositories
{
    // This class implements the IStockRepository interface to provide CRUD operations for Stock entities.
    public class StockRepository : IStockRepository
    {
        // Dependency injected via constructor injection.
        private readonly ApplicationDBContext _dbContext;

        // Constructor to initialize the repository with the necessary database context.
        public StockRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Method to create a new stock entity asynchronously.
        public async Task<Stock> CreateAsync(Stock stock)
        {
            // Adds the stock entity to the DbContext and saves changes.
            await _dbContext.Stocks.AddAsync(stock);
            await _dbContext.SaveChangesAsync();
            return stock; // Returns the created stock entity.
        }

        // Method to delete a stock entity by its ID asynchronously.
        public async Task<Stock?> DeleteAsync(int id)
        {
            // Finds the stock entity by ID.
            var stock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stock == null)
            {
                return null; // Returns null if the stock entity does not exist.
            }
            // Removes the stock entity from the DbContext and saves changes.
            _dbContext.Stocks.Remove(stock);
            await _dbContext.SaveChangesAsync();
            return stock; // Returns the deleted stock entity.
        }

        // Method to update an existing stock entity asynchronously.
        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            // Finds the stock entity by ID.
            var stock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stock == null)
            {
                return null; // Returns null if the stock entity does not exist.
            }

            // Updates the stock entity's properties with values from the DTO.
            stock.Symbol = stockDto.Symbol;
            stock.CompanyName = stockDto.CompanyName;
            stock.Purchase = stockDto.Purchase;
            stock.MarketCap = stockDto.MarketCap;
            stock.Industry = stockDto.Industry;
            stock.LastDiv = stockDto.LastDiv;

            // Saves changes to the DbContext.
            await _dbContext.SaveChangesAsync();
            return stock; // Returns the updated stock entity.
        }

        // Method to get a stock entity by its ID asynchronously.
        public async Task<Stock?> GetByIdAsync(int id)
        {
            // Finds and returns the stock entity by ID.
            var stock = await _dbContext.Stocks.FindAsync(id);
            return stock;
        }

        // Method to get all stock entities asynchronously.
        public async Task<List<Stock>> GettAllAsync()
        {
            // Returns all stock entities as a list.
            return await _dbContext.Stocks.ToListAsync();
        }
    }
}
