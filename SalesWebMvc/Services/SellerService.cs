using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System.Collections.Generic;
using System.Linq;


namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context; // O private readonly previne que esta dependência não possa ser alterada

        public SellerService(SalesWebMvcContext context)//Construtor para injetar a dependência do context
        {
            _context = context;
        }

        public List<Seller> FindAll() //Opereção para acessar a fonte de dados relacionada a tabela de vendedores e converter para lista
        {
            return _context.Seller.ToList();
        }
        public void Insert(Seller obj)
        {
            
            _context.Add(obj);
            _context.SaveChanges();
        }
    }
}
