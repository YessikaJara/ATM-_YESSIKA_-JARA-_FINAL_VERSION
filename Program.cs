using ATM_YESSIKA_JARA;
using System;

namespace ATM_YESSIKA_JARA
{
    internal class Program
    {
        // Método para validar usuario en el archivo de cuentas
        // Se busca que el usuario y la clave digitada coincidan con los registrados en el archivo
        public static bool ValidarUsuario(string Usuario, string Clave, string RutaArchivo)
        {
            if (!File.Exists(RutaArchivo))
            {
                throw new FileNotFoundException("El archivo de cuentas no existe.", RutaArchivo);
            }

            string[] Lineas = File.ReadAllLines(RutaArchivo); // Se leen todas las líneas del archivo

            foreach (string Linea in Lineas)
            {
                if (string.IsNullOrWhiteSpace(Linea)) continue; // Se omiten líneas vacías

                string[] Partes = Linea.Split('|'); // Separar por “|” cada dato de la cuenta
                if (Partes.Length < 2) continue;

                string UsuarioArchivo = Partes[3].Trim(); // El usuario está en la posición 3
                string ClaveArchivo = Partes[1].Trim();   // La clave está en la posición 1

                // Si coincide usuario y clave => se valida el acceso
                if (UsuarioArchivo == Usuario && ClaveArchivo == Clave)
                {
                    return true;
                }
            }
            return false;
        }

        // Método para actualizar el saldo de una cuenta después de un depósito o retiro
        public static void ActualizarSaldo(string Ruta, CuentaBancaria CuentaLogin)
        {
            string[] Lineas = File.ReadAllLines(Ruta);

            for (int i = 0; i < Lineas.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(Lineas[i])) continue;

                string[] Partes = Lineas[i].Split('|');
                if (Partes.Length < 4) continue;

                string NumeroCuenta = Partes[0].Trim();
                string Pass = Partes[1].Trim();
                string User = Partes[3].Trim();

                // Buscar la cuenta activa y actualizar su saldo
                if (NumeroCuenta == CuentaLogin.NumeroCuenta && User == CuentaLogin.Usuario)
                {
                    Lineas[i] = $"{NumeroCuenta}|{Pass}|{CuentaLogin.Saldo}|{User}";
                    break;
                }
            }

            // Reescribir el archivo con la cuenta ya actualizada
            File.WriteAllLines(Ruta, Lineas);
        }

        // Método para actualizar la clave de la cuenta
        public static void ActualizarClave(string Ruta, CuentaBancaria CuentaLogin)
        {
            string[] Lineas = File.ReadAllLines(Ruta);

            for (int i = 0; i < Lineas.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(Lineas[i])) continue;

                string[] Partes = Lineas[i].Split('|');
                if (Partes.Length < 4) continue;

                string NumeroCuenta = Partes[0].Trim();
                string Pass = Partes[1].Trim();
                int Saldo = int.Parse(Partes[2].Trim());
                string User = Partes[3].Trim();

                // Buscar la cuenta activa y actualizar su clave
                if (NumeroCuenta == CuentaLogin.NumeroCuenta && User == CuentaLogin.Usuario)
                {
                    Lineas[i] = $"{NumeroCuenta}|{CuentaLogin.Clave}|{Saldo}|{User}";
                    break;
                }
            }

            File.WriteAllLines(Ruta, Lineas);
        }

        static void Main(string[] args)
        {
            // Rutas de archivos donde se guardan cuentas y movimientos
            //string RutaCuentas = "C:\\Users\\yessi\\OneDrive\\Escritorio\\CAJERO YESSIKA JARA\\ATM YESSIKA JARA\\Cuentas.txt";
            //string RutaMovimientos = "C:\\Users\\yessi\\OneDrive\\Escritorio\\CAJERO YESSIKA JARA\\ATM YESSIKA JARA\\Movimientos_Cuentas.txt";

            //Obtiene la ruta base del proyecto (donde se ejecuta el .exe)
            string rutaBase = AppDomain.CurrentDomain.BaseDirectory;

            // Subir tres niveles: bin -> Debug -> net8.0, debido a que los archivos se encuentran en la raíz del proyecto
            string rutaProyecto = Directory.GetParent(rutaBase).Parent.Parent.Parent.FullName;

            // Ahora construimos las rutas en esa carpeta
            string RutaCuentas = Path.Combine(rutaProyecto, "Cuentas.txt");
            string RutaMovimientos = Path.Combine(rutaProyecto, "Movimientos_Cuentas.txt");

            //Console.WriteLine("Ruta cuentas: " + RutaCuentas);
            //Console.WriteLine("Ruta movimientos: " + RutaMovimientos);


            string Terminar = "";
            int Opcion = -1;
            string Usuario = "";
            string Contrasena = "";
            int Monto = 0;
            bool LoginExitoso = false;
            string NumeroCuenta = "";
            string Pass = "";
            int Saldo = 0;
            string User = "";

            // Objetos para manejar los datos de la sesión
            CuentaBancaria CuentaLogin = new CuentaBancaria("", "", 0, "");
            MovimientoCuenta NuevoMovimiento = new MovimientoCuenta("", "", "", 0, 0, 0);

            // Bucle principal (programa solo termina si el usuario elige cerrar)
            do
            {
                Console.WriteLine("Bienvenido a su cajero electrónico");
                Console.WriteLine("1. Iniciar sesión");
                Console.WriteLine("0. Cerrar programa");

                string entradaMenu = Console.ReadLine();
                if (!int.TryParse(entradaMenu, out Opcion) || (Opcion != 0 && Opcion != 1))
                {
                    Console.WriteLine("Entrada inválida. Solo se aceptan números 0 o 1.");
                    Opcion = -1;
                    continue;
                }

                // Opción para iniciar sesión
                if (Opcion == 1)
                {
                    Console.WriteLine("Usuario:");
                    Usuario = Console.ReadLine();

                    Console.WriteLine("Contraseña:");
                    Contrasena = Console.ReadLine();

                    // Validar credenciales
                    LoginExitoso = ValidarUsuario(Usuario, Contrasena, RutaCuentas);

                    if (LoginExitoso)
                    {
                        Console.Clear();
                        Console.WriteLine("Bienvenido " + Usuario);

                        // Cargar datos de la cuenta activa
                        string[] Lineas = File.ReadAllLines(RutaCuentas);
                        foreach (string Linea in Lineas)
                        {
                            if (string.IsNullOrWhiteSpace(Linea)) continue;
                            string[] Partes = Linea.Split('|');
                            if (Partes.Length < 4) continue;

                            NumeroCuenta = Partes[0].Trim();
                            Pass = Partes[1].Trim();
                            Saldo = int.Parse(Partes[2].Trim());
                            User = Partes[3].Trim();

                            if (User == Usuario && Pass == Contrasena)
                            {
                                CuentaLogin.CargarDatos(NumeroCuenta, Pass, Saldo, User);
                                break;
                            }
                        }

                        // 🔹 Menú de operaciones disponible solo tras iniciar sesión correctamente
                        while (LoginExitoso)
                        {
                            Console.WriteLine("_____________________________________");
                            Console.WriteLine("--- MENÚ DE OPERACIONES ---");
                            Console.WriteLine("1. Depósito o Consignación");
                            Console.WriteLine("2. Retiro");
                            Console.WriteLine("3. Consulta de Saldo");
                            Console.WriteLine("4. Consulta últimos 5 movimientos");
                            Console.WriteLine("5. Cambio de Clave");
                            Console.WriteLine("6. Cerrar sesión");
                            Console.WriteLine("_____________________________________");

                            Console.WriteLine("\nSeleccione una opción:");
                            string entradaOpcion = Console.ReadLine();

                            if (!int.TryParse(entradaOpcion, out Opcion) || Opcion < 1 || Opcion > 6)
                            {
                                Console.WriteLine("Entrada inválida. Ingrese un número del 1 al 6.");
                                continue;
                            }

                            switch (Opcion)

                            {


                                case 1: // Depósito
                                    do
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Depósito o Consignación");
                                        Console.WriteLine("\nIngrese el monto a consignar");

                                        string entradaMonto = Console.ReadLine();
                                        if (!int.TryParse(entradaMonto, out Monto) || Monto <= 0)
                                        {
                                            Console.WriteLine("Monto inválido. Ingrese un número mayor que 0.");
                                            Monto = -1;
                                        }
                                        else
                                        {
                                            int saldoInicial = int.Parse(CuentaLogin.Saldo);
                                            Console.WriteLine("Saldo antiguo: " + CuentaLogin.Saldo);

                                            // Actualizar saldo en la sesión y en archivo
                                            Saldo = int.Parse(CuentaLogin.Saldo) + Monto;
                                            CuentaLogin.Saldo = Saldo.ToString();
                                            ActualizarSaldo(RutaCuentas, CuentaLogin);

                                            // Guardar movimiento en el historial
                                            NuevoMovimiento.CargarDatos(
                                                CuentaLogin.NumeroCuenta,
                                                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                                                "Depósito",
                                                Monto,
                                                saldoInicial,
                                                int.Parse(CuentaLogin.Saldo)
                                            );

                                            NuevoMovimiento.GuardarMovimiento(RutaMovimientos);
                                            Console.WriteLine("Depósito realizado exitosamente.");
                                        }

                                    } while (Monto <= 0);
                                    break;

                                case 2: // Retiro
                                    do
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Retiro");
                                        Console.WriteLine("\nIngrese el monto a retirar");

                                        string entradaMonto = Console.ReadLine();
                                        if (!int.TryParse(entradaMonto, out Monto) || Monto <= 0 || Monto > Saldo)
                                        {
                                            Console.WriteLine("Monto inválido. Debe ser un número positivo y no superar el saldo actual.");
                                            Monto = -1;
                                        }
                                        else
                                        {
                                            int saldoInicial = int.Parse(CuentaLogin.Saldo);
                                            Console.WriteLine("Saldo antiguo: " + CuentaLogin.Saldo);

                                            // Actualizar saldo tras retiro
                                            Saldo = int.Parse(CuentaLogin.Saldo) - Monto;
                                            CuentaLogin.Saldo = Saldo.ToString();
                                            ActualizarSaldo(RutaCuentas, CuentaLogin);

                                            // Guardar movimiento
                                            NuevoMovimiento.CargarDatos(
                                                CuentaLogin.NumeroCuenta,
                                                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                                                "Retiro",
                                                Monto,
                                                saldoInicial,
                                                int.Parse(CuentaLogin.Saldo)
                                            );

                                            NuevoMovimiento.GuardarMovimiento(RutaMovimientos);
                                            Console.WriteLine("Retiro realizado exitosamente.");
                                        }

                                    } while (Monto <= 0);
                                    break;

                                case 3: // Consulta de saldo
                                    Console.WriteLine("Su saldo actual es: $" + CuentaLogin.Saldo);
                                    break;

                                case 4: // Consulta últimos 5 movimientos
                                    Console.Clear();
                                    Console.WriteLine("Últimos 5 movimientos de la cuenta: " + CuentaLogin.NumeroCuenta);

                                    if (!File.Exists(RutaMovimientos))
                                    {
                                        Console.WriteLine("No se encontró el archivo de movimientos.");
                                        break;
                                    }

                                    string[] lineasMovimientos = File.ReadAllLines(RutaMovimientos);
                                    var movimientos = new List<MovimientoCuenta>();

                                    // Leer archivo de movimientos y filtrar solo de la cuenta activa
                                    foreach (string linea in lineasMovimientos)
                                    {
                                        if (string.IsNullOrWhiteSpace(linea)) continue;

                                        string[] partes = linea.Split('|');
                                        if (partes.Length < 6) continue;

                                        string numCuenta = partes[0].Trim();
                                        string fecha = partes[1].Trim();
                                        string tipo = partes[2].Trim();
                                        int monto = int.Parse(partes[3].Trim());
                                        int saldoIni = int.Parse(partes[4].Trim());
                                        int saldoFin = int.Parse(partes[5].Trim());

                                        if (numCuenta == CuentaLogin.NumeroCuenta)
                                        {
                                            movimientos.Add(new MovimientoCuenta(numCuenta, fecha, tipo, monto, saldoIni, saldoFin));
                                        }
                                    }

                                    // Ordenar movimientos y mostrar máximo 5
                                    var ultimosMovs = movimientos
                                        .OrderByDescending(m => DateTime.Parse(m.Fecha))
                                        .Take(5)
                                        .ToList();

                                    if (ultimosMovs.Count == 0)
                                    {
                                        Console.WriteLine("No se encontraron movimientos para esta cuenta.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("{0,-20} {1,-15} {2,-10} {3,-15} {4,-15}",
                                         "Fecha", "Tipo", "Monto", "SaldoInicial", "SaldoFinal");

                                        foreach (var mov in ultimosMovs)
                                        {
                                            Console.WriteLine("{0,-20} {1,-15} {2,-10} {3,-15} {4,-15}",
                                                              mov.Fecha,
                                                              mov.TipoMovimiento,
                                                              mov.Monto,
                                                              mov.SaldoInicial,
                                                              mov.SaldoFinal);
                                        }
                                    }
                                    break;

                                case 5: // Cambio de clave
                                    string ClaveActual = "";
                                    string ClaveNueva = "";
                                    string ConfirmarClave = "";

                                    Console.WriteLine("Ingrese su clave actual:");
                                    ClaveActual = Console.ReadLine();

                                    Console.WriteLine("Ingrese la nueva clave:");
                                    ClaveNueva = Console.ReadLine();

                                    Console.WriteLine("Confirme la nueva clave:");
                                    ConfirmarClave = Console.ReadLine();

                                    // Validar y actualizar clave
                                    if (CuentaLogin.CambiarClave(ClaveActual, ClaveNueva))
                                    {
                                        ActualizarClave(RutaCuentas, CuentaLogin);
                                        Console.WriteLine("Clave actualizada correctamente.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("La clave actual no coincide. Intente de nuevo.");
                                    }
                                    break;

                                case 6: // Cerrar sesión
                                    LoginExitoso = !LoginExitoso;
                                    Console.Clear();
                                    Opcion = -1;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Usuario o contraseña incorrectos.");
                    }
                }
                else if (Opcion == 0)
                {
                    Terminar = "cerrar";
                }

            } while (Terminar != "cerrar"); // Finaliza programa cuando el usuario decide cerrar
        }
    }
}