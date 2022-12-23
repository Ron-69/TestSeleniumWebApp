using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context; // O private readonly previne que esta dependência não possa ser alterada

        public SellerService(SalesWebMvcContext context)//Construtor para injetar a dependência do context
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync() //Opereção para acessar a fonte de dados relacionada a tabela de vendedores e converter para lista -Modificação
        {
            return await _context.Seller.ToListAsync();
        }
        public async Task Insert(Seller obj) // trocamos o void por Task
        {
            
            _context.Add(obj); //é feita apenas em memória
            await _context.SaveChangesAsync();// vai acessar o banco de dados e tem que ser assincrona
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return  await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);

        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Seller.FindAsync(id);
            _context.Seller.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)// testa se no banco de dados tem alguém com o mesmo id do objeto
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
           
        }
    }
}
