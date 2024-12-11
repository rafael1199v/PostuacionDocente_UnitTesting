// using PostulacionDocente.ServicesApp.Models;

using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using PostulacionDocente.ServicesApp.Models;


namespace AppTest;


public class UsuarioServiceTest
{
   
    private static DbContextOptions<PostulacionDocenteContext> dbContextOptions = new DbContextOptionsBuilder<PostulacionDocenteContext>()
        .UseInMemoryDatabase(databaseName: "MateriaDbTest")
        .Options;
    private PostulacionDocenteContext context;
    private IUsuarioService _service;

    [OneTimeSetUp]
    public void SetUp()
    {
        context = new PostulacionDocenteContext(dbContextOptions);
        _service = new UsuarioService();
        context.Database.EnsureCreated();

        SeedDatabase();
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }


    [Test]
    public void LoginDocenteRegistradoTest()
    {
        //Configuracion
        LoginUsuarioDTO credenciales = new LoginUsuarioDTO{
            Email = "rafael1199v@gmail.com",
            Password = "1234"
        };

        //Ejecucion
        bool isLogged = _service.LoginDocente(credenciales, context, out string mensaje, out string usuarioCI);

        //Validacion


        Assert.That(isLogged);
        Assert.That(mensaje, Is.EqualTo("Usuario autenticado"));
        Assert.That(usuarioCI, Is.EqualTo("13776453"));
    }



    [Test]
    public void LoginDocenteNoRegistradoTest()
    {
        LoginUsuarioDTO credenciales = new LoginUsuarioDTO{
            Email = "noregistrado@gmail.com",
            Password = "1234"
        };

        //Ejecucion
        bool isLogged = _service.LoginDocente(credenciales, context, out string mensaje, out string usuarioCI);

        //Validacion


        Assert.That(!isLogged);
        Assert.That(mensaje, Is.EqualTo("Credenciales invalidas o el usuario no se encuentra registrado como docente."));
        Assert.That(usuarioCI, Is.EqualTo("-1"));
    }


    [Test]
    public void LoginJefeRegistradoTest()
    {
        //Configuracion
        LoginUsuarioDTO credenciales = new LoginUsuarioDTO{
            Email = "matias@gmail.com",
            Password = "12346"
        };

        //Ejecucion
        bool isLogged = _service.LoginJefeCarrera(credenciales, context, out string mensaje, out string usuarioCI);

        //Validacion
        Assert.That(isLogged);
        Assert.That(mensaje, Is.EqualTo("Usuario autenticado"));
        Assert.That(usuarioCI, Is.EqualTo("22222222"));
    }

   
    [Test]
    public void LoginJefeNoRegistradoTest()
    {
        //Configuracion
        LoginUsuarioDTO credenciales = new LoginUsuarioDTO{
            Email = "jefenoregistado@gmail.com",
            Password = "12346"
        };

        //Ejecucion
        bool isLogged = _service.LoginJefeCarrera(credenciales, context, out string mensaje, out string usuarioCI);

        //Validacion
        Assert.That(!isLogged);
        Assert.That(mensaje, Is.EqualTo("Credenciales invalidas o el usuario no se encuentra registrado como jefe de carrera."));
        Assert.That(usuarioCI, Is.EqualTo("-1"));
    }

    [Test]
    public void LoginDocenteConCredencialesJefeTest()
    {
        //Configuracion
        LoginUsuarioDTO credenciales = new LoginUsuarioDTO{
            Email = "matias@gmail.com",
            Password = "12346"
        };

        //Ejecucion
        bool isLogged = _service.LoginDocente(credenciales, context, out string mensaje, out string usuarioCI);

        //Validacion
        Assert.That(!isLogged);
        Assert.That(mensaje, Is.EqualTo("Credenciales invalidas o el usuario no se encuentra registrado como docente."));
        Assert.That(usuarioCI, Is.EqualTo("-1"));
    }

    [Test]
    public void LoginJefeConCredencialesDocenteTest()
    {
         //Configuracion
        LoginUsuarioDTO credenciales = new LoginUsuarioDTO{
            Email = "rafael1199v@gmail.com",
            Password = "1234"
        };

        //Ejecucion
        bool isLogged = _service.LoginJefeCarrera(credenciales, context, out string mensaje, out string usuarioCI);

        //Validacion


        Assert.That(!isLogged);
        Assert.That(mensaje, Is.EqualTo("Credenciales invalidas o el usuario no se encuentra registrado como jefe de carrera."));
        Assert.That(usuarioCI, Is.EqualTo("-1"));
    }

    private void SeedDatabase()
    {
        List<Usuario> usuarios = new List<Usuario>(){
            new Usuario{ UsuarioId = 1, Nombre = "Rafael", Ci = "13776453", FechaNacimiento = DateTime.Now.AddDays(-100), NumeroTelefono = "7648909", Correo = "rafael1199v@gmail.com", Contrasenha = "1234"},
            new Usuario{ UsuarioId = 2, Nombre = "Daniel", Ci = "11111111", FechaNacimiento = DateTime.Now.AddDays(-500), NumeroTelefono = "7638909", Correo = "daniel@gmail.com", Contrasenha = "12345"},
            new Usuario{ UsuarioId = 3, Nombre = "Matias", Ci = "22222222", FechaNacimiento = DateTime.Now.AddDays(-1000), NumeroTelefono = "7248909", Correo = "matias@gmail.com", Contrasenha = "12346"}
        };

        context.Usuarios.AddRange(usuarios);

        List<Docente> docentes = new List<Docente>(){
            new Docente {DocenteId = 1, Especialidad = "Programacion", Experiencia = 0, DescripcionPersonal = "Hola soy rafael", Grado = "Ingeniero", UsuarioId = 1},
            new Docente {DocenteId = 2, Especialidad = "Computacion grafica", Experiencia = 3, DescripcionPersonal = "Hola soy daniel", Grado = "Ingeniero", UsuarioId = 2}
        };


        context.Docentes.AddRange(docentes);

        List<JefeCarrera> jefes = new List<JefeCarrera>{
            new JefeCarrera{JefeCarreraId = 1, UsuarioId = 3}
        };


        context.JefeCarreras.AddRange(jefes);

        context.SaveChanges();

    }


    
}