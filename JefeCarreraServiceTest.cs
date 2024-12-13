using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PostulacionDocente.ServicesApp.Models;

namespace AppTest
{
    [TestFixture]
    public class JefeCarreraServiceTest
    {
        private static DbContextOptions<PostulacionDocenteContext> dbContextOptions = new DbContextOptionsBuilder<PostulacionDocenteContext>()
            .UseInMemoryDatabase(databaseName: "JefeCarreraDbTest")
            .Options;

        private PostulacionDocenteContext _context;
        private IJefeCarreraService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new JefeCarreraService(); // Asumiendo que tienes esta implementación
            _context = new PostulacionDocenteContext(dbContextOptions);
            _context.Database.EnsureCreated();

            SeedDatabase();
        }

        [TearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void AscenderSolicitud_CorrectamenteTest()
        {
            // Configuración
            int postulacionId = 1;

            // Ejecución
            bool resultado = _service.AscenderSolicitud(_context, postulacionId, out string mensaje);

            // Validación
            Assert.That(resultado, Is.True);
            Assert.That(mensaje, Is.EqualTo("Postulacion ascendida correctamente"));

            // Verifica que el estado haya sido actualizado correctamente
            var postulacion = _context.Postulacions.Include(p => p.Estado).FirstOrDefault(p => p.PostulacionId == postulacionId);
            Assert.That(postulacion?.Estado.Mensaje, Is.EqualTo("Exposición"));
        }

        [Test]
        public void AscenderSolicitud_PostulacionNoExistenteTest()
        {
            // Configuración
            int postulacionId = -1;

            // Ejecución
            bool resultado = _service.AscenderSolicitud(_context, postulacionId, out string mensaje);

            // Validación
            Assert.That(resultado, Is.False);
            Assert.That(mensaje, Is.EqualTo("Hubo un error, no hemos podido ascender la postulacion. Intentalo otra vez"));
        }
        // [Test]
        // public void ObtenerSolicitudes_VacanteConSolicitudesTest()
        // {
        //     // Configuración
        //     int vacanteId = 1;

        //     // Ejecución
        //     List<DocenteDatosPostulacionDTO> solicitudes = _service.ObtenerSolicitudes(_context, vacanteId);

        //     // Validación
        //     Assert.That(solicitudes.Count, Is.EqualTo(1)); // Se espera 1 solicitud
        //     Assert.That(solicitudes.First().PostulacionId, Is.EqualTo(1));  // Validación de PostulacionId
        // }


        [Test]
        public void ObtenerSolicitudes_VacanteSinSolicitudesTest()
        {
            // Configuración
            int vacanteId = 999;

            // Ejecución
            List<DocenteDatosPostulacionDTO> solicitudes = _service.ObtenerSolicitudes(_context, vacanteId);

            // Validación
            Assert.That(solicitudes.Count, Is.EqualTo(0)); // No debe haber solicitudes
        }

        private void SeedDatabase()
        {
            List<Estado> estados = new List<Estado>
            {
                new Estado { EstadoId = 1, Mensaje = "En revisión" },
                new Estado { EstadoId = 2, Mensaje = "Exposición" },
                new Estado { EstadoId = 3, Mensaje = "Entrevista" },
                new Estado { EstadoId = 4, Mensaje = "Aceptado" },
                new Estado { EstadoId = 5, Mensaje = "Rechazado" }
            };


            _context.Estados.AddRange(estados);

            // Sembrar datos iniciales
            List<Usuario> usuarios = new List<Usuario>()
            {
                new Usuario { UsuarioId = 1, Nombre = "Rafael", Ci = "13776453", FechaNacimiento = DateTime.Now.AddYears(-30), NumeroTelefono = "7648909", Correo = "rafael@gmail.com", Contrasenha = "1234" },
                new Usuario { UsuarioId = 2, Nombre = "Daniel", Ci = "13774453", FechaNacimiento = DateTime.Now.AddYears(-30), NumeroTelefono = "8648909", Correo = "daniel@gmail.com", Contrasenha = "1234" }
            };

            _context.Usuarios.AddRange(usuarios);

            List<JefeCarrera> jefes = new List<JefeCarrera>
            {
                new JefeCarrera { JefeCarreraId = 1, UsuarioId = 1 }
            };

            _context.JefeCarreras.AddRange(jefes);

            List<Docente> docentes = new List<Docente>()
            {
                new Docente {DocenteId = 1, Especialidad = "Computacion grafica", Experiencia = 3, DescripcionPersonal = "Hola soy daniel", Grado = "Ingeniero", UsuarioId = 2}
            };

            _context.Docentes.AddRange(docentes);

            List<Vacante> vacantes = new List<Vacante>
            {
                new Vacante { VacanteId = 1, NombreVacante = "Programación", FechaInicio = DateTime.Now.AddDays(-10), FechaFin = DateTime.Now.AddDays(10), JefeCarreraId = 1 }
            };


            _context.Vacantes.AddRange(vacantes);

            List<Postulacion> postulaciones = new List<Postulacion>
            {
                new Postulacion { PostulacionId = 1, VacanteId = 1, DocenteId = 1, EstadoId = 1}
            };

            _context.Postulacions.AddRange(postulaciones);


    

            _context.SaveChanges();
        }
    }
}
