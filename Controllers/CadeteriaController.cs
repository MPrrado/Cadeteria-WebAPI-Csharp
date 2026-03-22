using EspacioAccesoADatos;
using EspacioCadete;
using EspacioCadeteria;
using EspacioCliente;
using EspacioPedido;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EspacioCadeteriaController
{
    [ApiController]
    [Route("[controller]")]
    public class CadeteriaController : ControllerBase
    {
        // [HttpGet]
        // public IActionResult GetPedidos()
        // {
            
        // }
        private readonly string rutaArchivoCadeteria = "src/cadeteria.csv";
        private readonly string rutaArchivoClientes = "src/clientes.csv";
        private Cadeteria miCadeteria;  
        public CadeteriaController()
        {
            miCadeteria = AccesoADatos.CargarDatosCadeteria(rutaArchivoCadeteria);
            List<Cadete>listadoCadetes =AccesoADatos.CargarListadoCadetes("src/cadetes.csv");
            foreach (Cadete cadete in listadoCadetes)
            {
                miCadeteria.AgregarCadete(cadete.Nombre, cadete.Direccion, cadete.Telefono);
            }
        }

        [HttpPost]
        public IActionResult AltaPedido([FromBody]Pedido pedido)
        {
            if(pedido == null) return BadRequest("El pedido no puede ser nulo.");
            if (miCadeteria.AltaPedido(pedido, rutaArchivoClientes))
            {
                return Created("Pedido agregado exitosamente.", pedido);
            }
            else
            {
                return StatusCode(500, "No se pudo agregar el pedido.");
            }
        }
    }
}