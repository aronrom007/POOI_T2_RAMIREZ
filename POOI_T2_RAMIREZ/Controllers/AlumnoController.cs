using Newtonsoft.Json;
using POOI_T2_RAMIREZ.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace POOI_T2_RAMIREZ.Controllers
{
    public class AlumnoController : Controller
    {
        static string lista = "[]";
       
        private List<Alumno> Deserializar()
        {
            return JsonConvert.DeserializeObject<List<Alumno>>(lista);
        }

        private void Serializar(List<Alumno> alumnos)
        {
            lista = JsonConvert.SerializeObject(alumnos, Formatting.Indented);
            string ruta = Server.MapPath("~/App_Data/alumnos.json");
            System.IO.File.WriteAllText(ruta, lista);
            TempData["Mensaje"] = "Todo los datos se guardaron en el archivo alumnos.json";
        }
        
        public ActionResult Index()
        {
            List <Alumno> alumnos = Deserializar();
            return View(alumnos);
        }
        [HttpGet]
        public ActionResult Agregar()
        {
            return View(new Alumno());
        }

        [HttpPost]
        public ActionResult Agregar(string dni, string nombres, string apellidos, string carrera, int ciclo)
        {
            List<Alumno> alumnos = Deserializar();
            if (alumnos.Any(a => a.dni == dni))
            {
                ViewBag.Mensaje = "El DNI ya esta registrado.";
                return View(new Alumno(dni, nombres, apellidos, carrera, ciclo));
            }

            Alumno alumno = new Alumno(dni, nombres, apellidos, carrera, ciclo);
            alumnos.Add(alumno);

            Serializar(alumnos);

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(string dni)
        {
            List<Alumno> alumnos = Deserializar();
            Alumno encontrado = alumnos.Find(a => a.dni == dni);
            if (encontrado != null)
            {
                alumnos.Remove(encontrado);
                Serializar(alumnos);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Detalles(string dni)
        {
            List<Alumno> alumnos = Deserializar();
            Alumno encontrado = alumnos.Find(a => a.dni == dni);
            if(encontrado == null)
            {
                return RedirectToAction("Index");
            }
            return View(encontrado);
        }

        [HttpGet]
        public ActionResult Actualizar(string dni)
        {
            List<Alumno> alumnos = Deserializar();
            Alumno encontrado = alumnos.Find(a => a.dni == dni);
            if (encontrado == null)
            {
                return RedirectToAction("Index");
            }
            return View(encontrado);
        }

        [HttpPost]
        public ActionResult Actualizar(string dni, string nombres, string apellidos, string carrera, int ciclo)
        {
            List<Alumno> alumnos = Deserializar();
            int index = alumnos.FindIndex(a => a.dni == dni);
            if (index == -1)
            {
                ViewBag.Mensaje = "El alumno no existe.";
                return View(new Alumno(dni, nombres, apellidos, carrera, ciclo));
            }

            alumnos[index] = new Alumno(dni, nombres, apellidos, carrera, ciclo);
            Serializar(alumnos);
            return RedirectToAction("Index");
        }

        
    }
}