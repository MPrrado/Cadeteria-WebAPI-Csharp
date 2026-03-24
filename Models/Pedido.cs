using System.Diagnostics;
using EspacioCliente;

namespace EspacioPedido
{
    public enum Estados
    {
        SinAsignar = 0,
        Pendiente = 1,
        Preparacion = 2,
        EnCamino = 3,
        Entregado = 4
    }
    public class Pedido
    {

        private int numPedido;
        private string? observacion;
        private Cliente cliente;
        private Estados estado;
        private int idCadete;

        public string? Observacion {get; init;}
        public Cliente Cliente {get; init;}

        public Pedido()
        {
            
        }

        public Pedido(string nombreCliente, string direccionEntrega, long telefonoCliente, string referenciasEntrega, string observacion)
        {
            this.cliente = new Cliente(nombreCliente, direccionEntrega, telefonoCliente, referenciasEntrega);
            this.observacion = observacion;
        }
        public Pedido(Cliente cliente, string observacion)
        {
            this.cliente = new Cliente(cliente);
            this.observacion = observacion;
        }

        public void ObtenerDatosCliente()
        {
            Console.WriteLine("==========================================================================================================================");
            Console.WriteLine("Nombre: ", cliente.Nombre);
            Console.WriteLine("Direccion: ", cliente.Direccion);
            Console.WriteLine("Telefono: ", cliente.Telefono);
            Console.WriteLine("Referencias: ", cliente.Referencias);
            Console.WriteLine("==========================================================================================================================");
        }

        public string ObtenerDireccionCliente()
        {
            return cliente.Direccion;
        }

        public string ObtenerEstadoPedido()
        {
            return estado.ToString();
        }
        
        public void AsignarNumeroPedido(int numero)
        {
            numPedido = numero;
        }

        public void CambiarEstado(Estados estado)
        {
            this.estado = estado;
        }
        
        public void MostrarPedido()
        {
             Console.WriteLine($"~~~ Pedido Numero: {numPedido} | Observacion: {observacion} | Nombre Cliente: {cliente.Nombre} | Direccion: {cliente.Direccion} | Estado: {estado}");
        }

        public void AsignarIdCadete(int numeroCadete)
        {
            idCadete = numeroCadete;
        }
        public int ObtenerIdPedido()
        {
            return numPedido;
        }
        public int ObtenerIdCadete()
        {
            return idCadete;
        }
        public bool AsignarCliente(string nombre, string direccion, long telefono, string referencias)
        {
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(direccion) || telefono <= 0)
            {
                return false;
            }
            this.cliente = new Cliente(nombre, direccion, telefono, referencias);
            return true;
        }
    }
}