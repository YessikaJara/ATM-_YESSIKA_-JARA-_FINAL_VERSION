namespace ATM_YESSIKA_JARA
{
    public class MovimientoCuenta
    {
        // Atributos privados
        private string numeroCuenta;
        private string fecha;
        private string tipoMovimiento;
        private int monto;
        private int saldoInicial;
        private int saldoFinal;

        // Propiedades públicas
        public string NumeroCuenta
        {
            get { return numeroCuenta; }
            set { numeroCuenta = value; }
        }

        public string Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        public string TipoMovimiento
        {
            get { return tipoMovimiento; }
            set { tipoMovimiento = value; }
        }

        public int Monto
        {
            get { return monto; }
            set { monto = value; }
        }

        public int SaldoInicial
        {
            get { return saldoInicial; }
            set { saldoInicial = value; }
        }

        public int SaldoFinal
        {
            get { return saldoFinal; }
            set { saldoFinal = value; }
        }

        // Constructor
        public MovimientoCuenta(string numCuenta, string fecha, string tipo, int monto, int saldoIni, int saldoFin)
        {
            this.numeroCuenta = numCuenta;
            this.fecha = fecha;
            this.tipoMovimiento = tipo;
            this.monto = monto;
            this.saldoInicial = saldoIni;
            this.saldoFinal = saldoFin;
        }

        // Método para cargar o actualizar los datos
        public void CargarDatos(string numCuenta, string fecha, string tipo, int monto, int saldoIni, int saldoFin)
        {
            this.numeroCuenta = numCuenta;
            this.fecha = fecha;
            this.tipoMovimiento = tipo;
            this.monto = monto;
            this.saldoInicial = saldoIni;
            this.saldoFinal = saldoFin;
        }

        //Método para guardar el movimiento en el archivo
        public void GuardarMovimiento(string rutaMovimientos)
        {
            string linea = $"{NumeroCuenta}|{Fecha}|{TipoMovimiento}|{Monto}|{SaldoInicial}|{SaldoFinal}";

            //Siempre escribe primero un salto de línea y luego la nueva línea
            using (StreamWriter sw = new StreamWriter(rutaMovimientos, true))
            {
                sw.Write(Environment.NewLine + linea);
            }
        }

    }
}
