using System;
using System.IO;

namespace Peajes.Controllers
{
    public class CreateTxt
    {
        public void Create()
        {
            string path = @"C:\Users\Joel\source\repos\Peajes\Peajes\Resumen\Resumen.txt"; // Asegúrate de incluir una extensión de archivo
            try
            {
                // Verifica si el directorio existe, de lo contrario, créalo
                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Usar 'using' para asegurar que el StreamWriter se cierre correctamente
                using (StreamWriter sw = new StreamWriter(path))
                {
                    // Escribir líneas de texto
                    sw.WriteLine("Hello World!!");
                    sw.WriteLine("From the StreamWriter class");
                }

                Console.WriteLine("Archivo guardado exitosamente.");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Error de acceso: " + e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Directorio no encontrado: " + e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine("Error de I/O: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }
    }
}
