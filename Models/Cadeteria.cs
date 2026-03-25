using System.Data;
using System.Diagnostics.Contracts;
using System.IO.Compression;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using EspacioAccesoADatos;
using EspacioCadete;
using EspacioCliente;
using EspacioPedido;

namespace EspacioCadeteria
{
    public class Cadeteria
    {
        private string nombre;
        private long telefono;
        private List<Cadete> listadoCadete;
        private List<Pedido> listadoPedidos;

        public Cadeteria()
        {

        }
        public Cadeteria(string nombre, long telefono)
        {
            this.nombre = nombre;
            this.telefono = telefono;
            this.listadoCadete = new List<Cadete>();
            this.listadoPedidos = new List<Pedido>();
        }

        public string Nombre { get => nombre;}
        public long Telefono { get => telefono;}

        public double ObtenerRecaudacion()
        {
            double totalRecaudado = 0;
            foreach (Cadete c in listadoCadete)
            {
                totalRecaudado += c.ObtenerJornal();
            }
            return totalRecaudado;
        }

        // public bool AltaPedido(string? observacion, string rutaArchivoClientes)
        // {
        //     try
        //     {
        //         string[] datosClienteAleatorio = AccesoADatos.ObtenerCliente(rutaArchivoClientes);
        //         Pedido pedido = new(datosClienteAleatorio[0],datosClienteAleatorio[1],long.Parse(datosClienteAleatorio[2]), datosClienteAleatorio[3], observacion);
        //         pedido.AsignarNumeroPedido(listadoPedidos.Count == 0 ? 1:listadoPedidos.Count+1);
        //         pedido.CambiarEstado(Estados.SinAsignar);
        //         listadoPedidos.Add(pedido);
        //         return true;
        //     }catch
        //     {
        //         return false;
        //     }
        // }
        public bool AltaPedido(Pedido nuevoPedido)
        {
            Random rnd = new Random();
            if(nuevoPedido == null) return false;
            int idPedido = 1;
            if(listadoPedidos.Any())
            {
                idPedido = listadoPedidos.Max(p => p.ObtenerIdPedido()) + 1;
            }
            nuevoPedido.AsignarNumeroPedido(idPedido);
            // nuevoPedido.CambiarEstado(Estados.SinAsignar); //comentado solo para pruebas 
            // string[] datosCliente = AccesoADatos.ObtenerCliente(rutaArchivoClientes);
            nuevoPedido.AsignarCliente(nuevoPedido.Cliente.Nombre, nuevoPedido.Cliente.Direccion, nuevoPedido.Cliente.Telefono, nuevoPedido.Cliente.Referencias);
            this.listadoPedidos.Add(nuevoPedido);
            return true;
        }

        public bool AgregarCadete(string nombre, string direccion, long telefono)
        {
            // Las validaciones previas evitan el try-catch
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(direccion)) return false;

            int id_cadete = listadoCadete.Count + 1;
            listadoCadete.Add(new Cadete(id_cadete, nombre, direccion, telefono)); 
            return true;
        }

        public bool AsignarPedidoACadete(int numeroPedido)
        {
            // 1. Verificamos que haya cadetes disponibles
            if (!listadoCadete.Any()) return false;

            // 2. Buscamos el pedido con FirstOrDefault (devuelve null si no existe)
            Pedido? pedidoParaAsignar = listadoPedidos.FirstOrDefault(p => p.ObtenerIdPedido() == numeroPedido);
            
            // 3. Si es null, o ya está entregado, devolvemos false sin tirar excepciones
            if (pedidoParaAsignar == null || pedidoParaAsignar.ObtenerEstadoPedido() == Estados.Entregado.ToString()) return false;

            // 4. Lógica normal
            Cadete cadeteConMenosPedidos = listadoCadete.OrderBy(c => c.ObtenerPendientes()).First();
            pedidoParaAsignar.CambiarEstado(Estados.Pendiente);
            pedidoParaAsignar.AsignarIdCadete(cadeteConMenosPedidos.Id);
            
            return cadeteConMenosPedidos.AsignarPedido(pedidoParaAsignar);
        }

        // public void MostrarPedidos()
        // {
        //     foreach(Pedido p in listadoPedidos)
        //     {
        //         p.MostrarPedido();
        //     }
        // }
        // public void MostrarPedidosSinAsignar()
        // {
        //     foreach(Pedido p in listadoPedidos)
        //     {
        //         if(p.ObtenerEstadoPedido() == Estados.SinAsignar.ToString())
        //         {
        //             p.MostrarPedido();
        //         }
        //     }
        // }
        // public void MostrarPedidosAsignados()
        // {
        //     foreach(Pedido p in listadoPedidos)
        //     {
        //         if(p.ObtenerEstadoPedido() != Estados.SinAsignar.ToString() && p.ObtenerEstadoPedido() != Estados.Entregado.ToString())
        //         {
        //             p.MostrarPedido();
        //         }
        //     }
        // }

        // public void MostrarCadetes()
        // {
        //     foreach(Cadete c in listadoCadete)
        //     {
        //         c.MostrarCadete();
        //         Console.WriteLine("-------------------listado pedidos-------------------");
        //         foreach(Pedido p in c.ObtenerListadoPedidos())
        //         {
        //             p.MostrarPedido();
        //         }
        //         Console.WriteLine("-----------------------------------------");
        //     }
        // }
        
        public Pedido ObtenerPedido(int numeroPedido)
        {
            return listadoPedidos.First(p => p.ObtenerIdPedido() == numeroPedido);
        }
        // public void MostrarEstados()
        // {
        //     Console.WriteLine($"[0]{Estados.SinAsignar}");
        //     Console.WriteLine($"[1]{Estados.Pendiente}");
        //     Console.WriteLine($"[2]{Estados.Preparacion}");
        //     Console.WriteLine($"[3]{Estados.EnCamino}");
        //     Console.WriteLine($"[4]{Estados.Entregado}");
        // }
        
        public List<Pedido> ObtenerListadoPedido()
        {
            return listadoPedidos;
        }
        // agregar metodo que me permita reasignar pedido
        public bool ReasignarPedido(int numeroPedido, int numeroCadete)
        {
            //Buscamos el pedido de forma segura (retorna null si no existe)
            Pedido? pedidoReasignar = listadoPedidos.FirstOrDefault(p => p.ObtenerIdPedido() == numeroPedido);
            if (pedidoReasignar == null) return false;

            //Buscamos los cadetes involucrados
            Cadete? cadeteActual = listadoCadete.FirstOrDefault(c => c.Id == pedidoReasignar.ObtenerIdCadete());
            Cadete? cadeteNuevo = ObtenerCadete(numeroCadete);

            //Validamos que el nuevo cadete exista y no le estemos reasignando el pedido al mismo cadete
            if (cadeteNuevo == null || cadeteActual?.Id == cadeteNuevo.Id) return false;

            //Intentamos eliminar el pedido del cadete actual
            if (cadeteActual != null)
            {
                bool seBorro = cadeteActual.EliminarPedido(pedidoReasignar.ObtenerIdPedido());
                // Si no se pudo borrar (ej: porque ya estaba entregado), cortamos la ejecución acá
                if (!seBorro) return false; 
            }

            //Asignamos al nuevo cadete
            bool seAgrego = cadeteNuevo.AsignarPedido(pedidoReasignar);

            //Si se agregó correctamente, actualizamos la referencia dentro del objeto Pedido
            if (seAgrego)
            {
                pedidoReasignar.AsignarIdCadete(cadeteNuevo.Id);
            }
            return seAgrego;
        }
        public Cadete? ObtenerCadete(int numeroCadete)
        {
            return listadoCadete.Exists(c => c.Id ==numeroCadete) ? listadoCadete.First(c => c.Id == numeroCadete): null;
        }

        public List<Cadete> ObtenerListadoCadetes()
        {
            return listadoCadete;
        }
        public double ObtenerPromedioEntregasPorCadete(int idCadete)
        {
            if (!listadoCadete.Any(c => c.Id == idCadete)) return 0; //si no existe tal cadete en la lista no buscamos y retornamos 0
            List<Pedido> pedidosListado = listadoCadete.First(c => c.Id == idCadete).ObtenerListadoPedidos(); //si existe lo separamos y obtenemos la lista de pedidos del mismo
            if (pedidosListado.Count == 0) return 0; //si el cadete no tiene pedidos, evitamos la division por cero y retornamos 0
            int pedidosEntregado = pedidosListado.Count(p => p.ObtenerEstadoPedido() == Estados.Entregado.ToString()); //contamos los pedidos entregados
            int pedidosTotal = pedidosListado.Count(); //contamos el total de pedidos
            return (double)(pedidosEntregado / (double)pedidosTotal)*100; //retornamos el porcentaje de entregas realizadas por el cadete
        }

        public double ObtenerRecaudacionCadete(int idCadete)
        {
            if (!listadoCadete.Any(c => c.Id == idCadete)) return 0; //si no existe tal cadete en la lista no buscamos y retornamos 0
            return listadoCadete.First(c => c.Id == idCadete).ObtenerJornal(); //si existe lo separamos y obtenemos el jornal del mismo
        }

        public Informe GenerarInforme()
        {
            int totalPedidos = listadoPedidos.Count;
            int pedidosEntregados = 0;
            
            foreach (Pedido p in listadoPedidos)
            {
                if (p.ObtenerEstadoPedido() == Estados.Entregado.ToString())
                {
                    pedidosEntregados++;
                }
            }

            double promedioEntregas = 0;
            double recaudacionPorCadete = 0;
            List<string> listaPromedios = new List<string>();
            List<string> listaRecaudacion = new List<string>();

            //obtenemos el promedio de entregas por cadete y la recaudacion por cadete para luego agregarlos a una lista de strings que se muestra en el informe

            foreach (Cadete c in listadoCadete)
            {
                promedioEntregas = ObtenerPromedioEntregasPorCadete(c.Id);
                recaudacionPorCadete = ObtenerRecaudacionCadete(c.Id);
                listaPromedios.Add($"Cadete: {c.ObtenerNombreCadete()} - Promedio de entregas: {promedioEntregas}%");
                listaRecaudacion.Add($"Cadete: {c.ObtenerNombreCadete()} - Recaudacion: {recaudacionPorCadete}");
            }

            double totalRecaudado = ObtenerRecaudacion();
            
            return new Informe
            {
                TotalPedidos = listadoPedidos.Count,
                PedidosEntregados = pedidosEntregados,
                PromediosDeEntregaPorCadete = listaPromedios,
                RecaudadoPorCadete = listaRecaudacion,
                TotalRecaudado = totalRecaudado
            };
        }
    }
}