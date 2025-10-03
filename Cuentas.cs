namespace ATM_YESSIKA_JARA
{


    public class CuentaBancaria
    {
        // Atributos (campos privados para proteger los datos)
        private string numeroCuenta;
        private string clave;
        private int saldo;
        private string usuario;

        // Propiedades públicas para el acceso controlado
        public string NumeroCuenta
        {
            get { return numeroCuenta; }
            set { NumeroCuenta = value; }
        }

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        public string Saldo
        {
            get { return saldo.ToString(); }
            set { saldo = int.Parse(value); }
        }


        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }

        // Constructor para inicializar la cuenta
        public CuentaBancaria(string numCuenta, string pass, int sldInicial, string user)
        {
            this.numeroCuenta = numCuenta;
            this.clave = pass;
            this.saldo = sldInicial;
            this.usuario = user;
        }

        // --- Funciones (Métodos) ---

        // Método para actualizar/rellenar la instancia existente
        public void CargarDatos(string numCuenta, string pass, int sldInicial, string user)
        {
            this.numeroCuenta = numCuenta;
            this.clave = pass;
            this.saldo = sldInicial;
            this.usuario = user;
        }

        // 1. Cambio de Clave
        public bool CambiarClave(string claveActual, string nuevaClave)
        {
            if (claveActual == clave)
            {
                if (nuevaClave != clave)
                {
                    clave = nuevaClave;
                    Console.WriteLine("La clave ha sido cambiada exitosamente.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Error: La nueva clave no puede ser igual a la clave actual.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Error: La clave actual ingresada es incorrecta.");
                return false;
            }
        }
    }


}


