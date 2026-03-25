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
       
        private static Cadeteria miCadeteria = InicializarCadeteria(); // aqui se ejecuta una vez debido a que es static, y se mantiene en memoria durante toda la vida de la aplicación.

        
        private static Cadeteria InicializarCadeteria()
        {
            Cadeteria nuevaCadeteria = AccesoADatos.CargarDatosCadeteria("src/cadeteria.csv");
            List<Cadete> cadetesCSV = AccesoADatos.CargarListadoCadetes("src/cadetes.csv");

        
            foreach (Cadete cadete in cadetesCSV)
            {
                nuevaCadeteria.AgregarCadete(cadete.Nombre, cadete.Direccion, cadete.Telefono);
            }
            
            List<Pedido> pedidosCSV = AccesoADatos.CargarPedidos("src/pedidos.csv", nuevaCadeteria.ObtenerListadoCadetes());

            foreach (Pedido pedido in pedidosCSV)
            {
                nuevaCadeteria.AltaPedido(pedido);
            }

            return nuevaCadeteria;
        }

        [HttpPost("AltaPedido")]
        public IActionResult AltaPedido([FromBody]Pedido pedido)
        {
            if(pedido == null) return BadRequest("El pedido no puede ser nulo.");
            if (miCadeteria.AltaPedido(pedido))
            {
                return Created("Pedido agregado exitosamente.", pedido);
            }
            else
            {
                return StatusCode(500, "No se pudo agregar el pedido.");
            }
        }

        [HttpGet("ObtenerListaPedidos")]
        public IActionResult GetPedidos()
        {
            List<Pedido> pedidos = miCadeteria.ObtenerListadoPedido();
            if(pedidos == null || pedidos.Count == 0) return NoContent();
            return Ok(pedidos);
        }

        [HttpGet("ObtenerListadoCadetes")]
        public IActionResult GetCadetes()
        {
            List<Cadete>Cadetes = miCadeteria.ObtenerListadoCadetes();
            if(Cadetes == null || Cadetes.Count == 0) return NoContent();
            return Ok(Cadetes);
        }

        [HttpGet("ObtenerInforme")]
        public IActionResult GetInforme()
        {
            Informe informe = miCadeteria.GenerarInforme();
            if(informe == null) return NoContent();
            return Ok(informe);
        }

    }
}