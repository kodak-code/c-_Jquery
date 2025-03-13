using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using c__Jquery.Models;

using System.Data;
using System.Data.SqlClient;

namespace c__Jquery.Controllers;

public class HomeController : Controller
{
    private readonly string cadenaSQL;

    public HomeController(IConfiguration config)
    {
        cadenaSQL = config.GetConnectionString("CadenaSQL");    
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public JsonResult ListarEmpleados()
    {
        List<Empleado> lista = new List<Empleado>();

        using (var conexion = new SqlConnection(cadenaSQL)) { 
            conexion.Open();
            var cmd = new SqlCommand("sp_lista_empleados", conexion);
            cmd.CommandType = CommandType.StoredProcedure;

            using(var dr = cmd.ExecuteReader())
            {
                while (dr.Read()) // mientras lee cada registro
                {
                    lista.Add(new Empleado
                    {
                        IdEmpleado = Convert.ToInt32(dr["IdEmpleado"]),
                        Nombre = dr["Nombre"].ToString(),
                        Cargo = dr["Cargo"].ToString(),
                        Oficina = dr["Oficina"].ToString(),
                        Salario = dr["Salario"].ToString(),
                        Telefono = Convert.ToInt32(dr["Telefono"]),
                        FechaIngreso = dr["FechaIngreso"].ToString()
                    });
                }
            }
            return Json(new {data =lista});// siempre se pasa la info mediante el atributo data
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
