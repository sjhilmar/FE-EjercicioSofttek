using FE_EjercicioSofttek.Models;
using FE_EjercicioSofttek.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FE_EjercicioSofttek.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVentasService service;

        public HomeController(IVentasService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Ventas> lista = await service.Listar();
            return View(lista);
        }

        public async Task<IActionResult> Venta(int id)
        {
            Ventas ventas = new Ventas();
            ViewBag.Accion = "Nueva Venta";

            if (id != 0)
            {
                ventas = await service.Obtener(id);
                ViewBag.Accion = "Editar Venta";
            }

            return View(ventas);
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(Ventas ventas)
        {
            bool respuesta;
            if (ventas.Id == 0) respuesta = await service.Guardar(ventas);
            else respuesta = await service.Editar(ventas);
            if (respuesta) return RedirectToAction("Index");
            else return NoContent();

        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var respuesta = await service.Eliminar(id);
            if (respuesta) return RedirectToAction("Index");
            else return NoContent();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}