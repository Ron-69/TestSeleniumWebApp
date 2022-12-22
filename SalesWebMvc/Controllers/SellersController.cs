using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService; // Objeto para receber dependencia
        private readonly DepartmentService _departmentService;
        public SellersController(SellerService sellerService, DepartmentService departmentService) //construtor para injetar dependencia
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index() // Chama o controlador que acessa o model, pegando o dado na lista list e vai encaminhar os dados para a View Index
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll(); //carrega os departamentos do banco de dados
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);//Vai retornar o objeto viewmodel com populado com os departamentos do banco de dados
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Evita que sofra ataques de CSRF
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index)); //redireciona o retorno para o Index
        }
    }
}
