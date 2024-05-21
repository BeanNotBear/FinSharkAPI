using FinShark.Dtos.Stock;
using FinShark.Models;
using System.Runtime.InteropServices;

namespace FinShark.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GettAllAsync();
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task<Stock?> DeleteAsync(int id); 
    }
}
