using System.Runtime.CompilerServices;
using EspacioCadete;
using EspacioCadeteria;
using EspacioCliente;
using EspacioPedido;

namespace EspacioAccesoADatos
{
    public static class AccesoADatos
    {
        public static Cadeteria CargarDatosCadeteria(string rutaArchivo)
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                {
                    // Console.WriteLine("No se encontró el archivo para cargar los datos de cadeteria, revisar la ruta");
                    return null;
                }else
                {
                    string[] lineas = File.ReadAllLines(rutaArchivo);
                    string[] datos;
                    if(lineas.Length > 1)
                    {
                        datos = lineas[1].Split(",");
                    }else
                    {
                        datos = lineas[0].Split(",");
                    }
                    string nombre = datos[0];
                    if (long.TryParse(datos[1], out long telefono))
                    {
                        long.TryParse(datos[1], out telefono);
                        Cadeteria cadeteria = new Cadeteria(nombre, telefono);
                        return cadeteria;
                    }else
                    {
                        // Console.WriteLine("Error al cargar el numero de telefono de la cadeteria");
                        telefono = -1;
                        return null;
                    }
                }
            }catch (Exception e)
            {
                // Console.WriteLine($"Error no se pudo cargar los datos de la cadeteria: {e}");
            }
            return null;
        }
        public static List<Cadete> CargarListadoCadetes(string rutaArchivo)
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                {
                    Console.WriteLine("No se encontró el archivo para cargar los datos de cadeteria, revisar la ruta");
                    return null;
                }else
                {
                    List<Cadete> listadoCadetes = new List<Cadete>();
                    string[] lineas = File.ReadAllLines(rutaArchivo);
                    for (int i = 1; i <= lineas.Length-1; i++)
                    {
                        string[] datos = lineas[i].Split(",");
                        int idCadete = i;
                        string nombreCadete = datos[0];
                        string direccion = datos[1];
                        if (long.TryParse(datos[2], out long telefono))
                        {
                            long.TryParse(datos[2], out telefono);
                        }else
                        {
                            telefono = -1;
                            // Console.WriteLine("Error no se pudo cargar el telefono del cadete, revisar la informacion dentro del csv");
                            return null;
                        }
                        listadoCadetes.Add(new Cadete(idCadete, nombreCadete,direccion,telefono));
                    }
                    return listadoCadetes;
                }
            }catch(Exception e)
            {
                // Console.WriteLine("Error, no se pudo cargar la lista de cadetes, revisar el archivo o la ruta del mismo");
            }
            return null;
        }
        public static string[] ObtenerCliente(string rutaArchivoClientes)
        {
            if (!File.Exists(rutaArchivoClientes))
            {
                // Console.WriteLine("Error, el archivo de clientes no existe");
                return null;
            }
            string[] lineas = File.ReadAllLines(rutaArchivoClientes);
            Random random = new Random();
            string[] datosCliente = lineas[random.Next(1, lineas.Length)].Split(',');
            return datosCliente;
        }

        public static List<Pedido> CargarPedidos(string rutaArchivo, List<Cadete> cadetes)
        {
            try
            {
                List<Pedido> listadoPedidos = new List<Pedido>();
                if(!File.Exists(rutaArchivo))
                {
                    return null;
                }
                string[] lines = File.ReadAllLines(rutaArchivo);
                for(int i = 1; i < lines.Length; i++)
                {
                    string[] datos = lines[i].Split(",");
                    int idPedido = int.Parse(datos[0]);
                    string observacion = datos[1];
                    string nombreCliente = datos[2];
                    string direccionCliente = datos[3];
                    long telefonoCliente = long.Parse(datos[4]);
                    string referenciasEntrega = datos[5];
                    Pedido pedido = new Pedido(nombreCliente, direccionCliente, telefonoCliente, referenciasEntrega, observacion);
                    pedido.AsignarNumeroPedido(idPedido);
                    pedido.CambiarEstado((Estados)int.Parse(datos[6]));
                    pedido.AsignarIdCadete(int.Parse(datos[7]));
                    // Asignamos el cadete al pedido si existe
                    int idCadeteAsignado = int.Parse(datos[7]);
                    Cadete cadeteAsignado = cadetes.FirstOrDefault(c => c.Id == idCadeteAsignado);
                    if (cadeteAsignado != null)
                    {
                        cadeteAsignado.AsignarPedido(pedido);
                    }
                    listadoPedidos.Add(pedido);
                }   
                return listadoPedidos;
            }catch
            {
                return null;
            }
            return null;
        }
    }

}



