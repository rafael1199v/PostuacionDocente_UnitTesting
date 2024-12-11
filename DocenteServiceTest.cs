


using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PostulacionDocente.ServicesApp.Models;

namespace AppTest;

public class DocenteServiceTest
{
    private static DbContextOptions<PostulacionDocenteContext> dbContextOptions = new DbContextOptionsBuilder<PostulacionDocenteContext>()
        .UseInMemoryDatabase(databaseName: "DocenteDbTest")
        .Options;


    private PostulacionDocenteContext _context;
    private IDocenteService _service;

    [SetUp]
    public void SetUp()
    {
        _service = new DocenteService();
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
    public void DocenteSePostulaAVacanteDisponibleTest()
    {
        //Configuracion
        NuevaPostulacionDTO nuevaPostulacion = new NuevaPostulacionDTO{
            VacanteId = 1,
            CI = "13776453",
            FechaFinalizacionVacante = DateTime.Now.AddDays(10)
        };

        //Ejecucion
        bool postuladoCorrectamente = _service.Postularse(_context, nuevaPostulacion, out string mensaje);
        Postulacion? postulacion = _context.Postulacions.FirstOrDefault(p => p.VacanteId == nuevaPostulacion.VacanteId && p.DocenteId == 1);

        Assert.That(postuladoCorrectamente);
        Assert.That(mensaje, Is.EqualTo("Se ha postulado correctamente"));
        Assert.That(postulacion != null);

    }

    [Test]
    public void DocenteSePostulaAVacanteConVigenciaExpiradaTest()
    {
        //Configuracion
        NuevaPostulacionDTO nuevaPostulacion = new NuevaPostulacionDTO{
            VacanteId = 1,
            CI = "13776453",
            FechaFinalizacionVacante = DateTime.Now.AddDays(-1)
        };

        //Ejecucion
        bool postuladoCorrectamente = _service.Postularse(_context, nuevaPostulacion, out string mensaje);
       
        Assert.That(!postuladoCorrectamente);
        Assert.That(mensaje, Is.EqualTo( "La vacante a dejado de ser vigente. Recargue la pagina"));
    }


    [Test]
    public void DocenteSePostulaConCarnetDeIdentidadNoRegistradoTest()
    {
        //Configuracion
        NuevaPostulacionDTO nuevaPostulacion = new NuevaPostulacionDTO{
            VacanteId = 1,
            CI = "-1",
            FechaFinalizacionVacante = DateTime.Now.AddDays(-1)
        };

        //Ejecucion
        bool postuladoCorrectamente = _service.Postularse(_context, nuevaPostulacion, out string mensaje);
       
        Assert.That(!postuladoCorrectamente);
        Assert.That(mensaje, Is.EqualTo("Hubo un error al registrar la docente en la vacante. Intentelo otra vez"));
    }

    private void SeedDatabase()
    {
        List<Usuario> usuarios = new List<Usuario>(){
            new Usuario{ UsuarioId = 1, Nombre = "Rafael", Ci = "13776453", FechaNacimiento = DateTime.Now.AddDays(-100), NumeroTelefono = "7648909", Correo = "rafael1199v@gmail.com", Contrasenha = "1234"},
            new Usuario{ UsuarioId = 2, Nombre = "Daniel", Ci = "11111111", FechaNacimiento = DateTime.Now.AddDays(-500), NumeroTelefono = "7638909", Correo = "daniel@gmail.com", Contrasenha = "12345"},
            new Usuario{ UsuarioId = 3, Nombre = "Matias", Ci = "22222222", FechaNacimiento = DateTime.Now.AddDays(-1000), NumeroTelefono = "7248909", Correo = "matias@gmail.com", Contrasenha = "12346"}
        };

        _context.Usuarios.AddRange(usuarios);

        List<Docente> docentes = new List<Docente>(){
            new Docente {DocenteId = 1, Especialidad = "Programacion", Experiencia = 0, DescripcionPersonal = "Hola soy rafael", Grado = "Ingeniero", UsuarioId = 1},
            new Docente {DocenteId = 2, Especialidad = "Computacion grafica", Experiencia = 3, DescripcionPersonal = "Hola soy daniel", Grado = "Ingeniero", UsuarioId = 2}
        };

        _context.Docentes.AddRange(docentes);

        List<JefeCarrera> jefes = new List<JefeCarrera>{
            new JefeCarrera{JefeCarreraId = 1, UsuarioId = 3}
        };

        _context.JefeCarreras.AddRange(jefes);

        List<Carrera> carreras = new List<Carrera>()
        {
            new Carrera{ CarreraId = 1, NombreCarrera = "Ingenieria de software", Sigla = "ISW"},
            new Carrera{ CarreraId = 2, NombreCarrera = "Psicologia", Sigla = "PSI"}
        };

        _context.Carreras.AddRange(carreras);

        List<Vacante> vacantes = new List<Vacante>()
        {
            new Vacante{ VacanteId = 1, NombreVacante = "Programacion I", Descripcion = "Vacante programacion I", FechaInicio = DateTime.Now.AddDays(-10), FechaFin = DateTime.Now.AddDays(10), MateriaId = 1, JefeCarreraId = 1},
            new Vacante{ VacanteId = 2, NombreVacante = "Programacion II", Descripcion = "Vacante programacion II", FechaInicio = DateTime.Now.AddDays(-10), FechaFin = DateTime.Now.AddDays(5), MateriaId = 2, JefeCarreraId = 1}
        };

        _context.Vacantes.AddRange(vacantes);

        _context.SaveChanges();

    }



}