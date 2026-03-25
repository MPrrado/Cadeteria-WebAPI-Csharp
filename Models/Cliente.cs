namespace EspacioCliente
{
    public class Cliente
    {
        private string nombre;
        private string direccion;
        private long telefono;
        private string referencias;

        public string Nombre {get; init;} 
        public string Direccion {get; init;}
        public long Telefono {get; init;}
        public string Referencias {get; init;}
        public Cliente()
        {
            
        }
        public Cliente(string nombre, string direccion, long telefono, string referencias)
        {
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
            Referencias = referencias;
        }
        public Cliente(Cliente cliente)
        {
            this.nombre = cliente.Nombre;
            this.direccion = cliente.Direccion;
            this.telefono = cliente.Telefono;
            this.referencias = cliente.Referencias;
        }
    }
}