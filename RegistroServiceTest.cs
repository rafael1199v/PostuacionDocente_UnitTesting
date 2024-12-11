
using Microsoft.EntityFrameworkCore;
using PostulacionDocente.ServicesApp.Models;

namespace AppTest;
public class RegistroServiceTest
{
   
    private static DbContextOptions<PostulacionDocenteContext> dbContextOptions = new DbContextOptionsBuilder<PostulacionDocenteContext>()
        .UseInMemoryDatabase(databaseName: "RegistroDbTest")
        .Options;
    private PostulacionDocenteContext context;
    private IRegistroService _service;

    [SetUp]
    public void SetUp()
    {
        context = new PostulacionDocenteContext(dbContextOptions);
        _service = new RegistroService();
        context.Database.EnsureCreated();

        SeedDatabase();
    }

    [TearDown]
    public void CleanUp()
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }

    [Test]
    public void RegistrarDocenteConNumeroOcupadoTest()
    {
        //Configuracion
        DocenteRegistroDTO nuevoDocente = new DocenteRegistroDTO{
            Nombre = "Gerardo Jimenez",
            Telefono = "7648909",
            CI = "33333333",
            FechaNacimiento = DateTime.Now.AddYears(-3),
            DescripcionPersonal = "Hola soy Gerardo",
            Materia = "Programacion II",
            Grado = "Ingeniero",
            AnhosExperiencia = 2,
            Correo = "gerardo@gmail.com",
            Contrasenha = "1234"
        };


        //Ejecucion
        bool registrado = _service.RegistrarDocente(nuevoDocente, context, out string mensaje);
        
        //Validacion
        Assert.That(!registrado);
        Assert.That(mensaje, Is.EqualTo("El email, el numero de telefono o el carnet de identidad ya esta en uso. Intentalo otra vez"));
         
    }


    [Test]
    public void RegistrarDocenteEmailOcupadoTest()
    {
        //Configuracion
        DocenteRegistroDTO nuevoDocente = new DocenteRegistroDTO{
            Nombre = "Gerardo Jimenez",
            Telefono = "7648909",
            CI = "33333333",
            FechaNacimiento = DateTime.Now.AddYears(-3),
            DescripcionPersonal = "Hola soy Gerardo",
            Materia = "Programacion II",
            Grado = "Ingeniero",
            AnhosExperiencia = 2,
            Correo = "matias@gmail.com",
            Contrasenha = "1234"
        };


        //Ejecucion
        bool registrado = _service.RegistrarDocente(nuevoDocente, context, out string mensaje);
      
        //Validacion
        Assert.That(!registrado);
        Assert.That(mensaje, Is.EqualTo("El email, el numero de telefono o el carnet de identidad ya esta en uso. Intentalo otra vez"));
         
    }


    [Test]
    public void RegistrarDocenteCarnetIdentidadOcupadoTest()
    {
        //Configuracion
        DocenteRegistroDTO nuevoDocente = new DocenteRegistroDTO{
            Nombre = "Gerardo Jimenez",
            Telefono = "7648909",
            CI = "13776453",
            FechaNacimiento = DateTime.Now.AddYears(-3),
            DescripcionPersonal = "Hola soy Gerardo",
            Materia = "Programacion II",
            Grado = "Ingeniero",
            AnhosExperiencia = 2,
            Correo = "matias@gmail.com",
            Contrasenha = "1234"
        };


        //Ejecucion
        bool registrado = _service.RegistrarDocente(nuevoDocente, context, out string mensaje);
     
        //Validacion
        Assert.That(!registrado);
        Assert.That(mensaje, Is.EqualTo("El email, el numero de telefono o el carnet de identidad ya esta en uso. Intentalo otra vez"));
         
    }

    [Test]
    public void RegistrarDocenteSinCredencialesNuevasOcupadasTest()
    {
        //Configuracion
        DocenteRegistroDTO nuevoDocente = new DocenteRegistroDTO{
            Nombre = "Gerardo Jimenez",
            Telefono = "12345578",
            CI = "33333333",
            FechaNacimiento = DateTime.Now.AddYears(-3),
            DescripcionPersonal = "Hola soy Gerardo",
            Materia = "Programacion II",
            Grado = "Ingeniero",
            AnhosExperiencia = 2,
            Correo = "gerardo@gmail.com",
            Contrasenha = "1234"
        };


        //Ejecucion
        bool registrado = _service.RegistrarDocente(nuevoDocente, context, out string mensaje);
        Docente? docente = context.Docentes.Include(d => d.Usuario).FirstOrDefault(d => d.Usuario.Ci == nuevoDocente.CI);

        //Validacion
        Assert.That(registrado);
        Assert.That(mensaje, Is.EqualTo("Docente registrado correctamente"));
        Assert.That(docente?.Usuario.Ci, Is.EqualTo("33333333"));
        
    }


    [Test]
    public void RegistrarJefeSinCredencialesNuevasOcupadasTest()
    {
        //Configuracion
        JefeCarreraRegistroDTO nuevoJefe = new JefeCarreraRegistroDTO {
            Nombre = "Juan Martinez",
            Telefono = "55544466",
            CI = "78449678",
            FechaNacimiento = DateTime.Now.AddMonths(-22),
            Correo = "juan@gmail.com",
            Contrasenha = "12345",
            Carreras = new List<string>(){"PSI", "ISW"}
        };

        //Ejecucion
        bool registrado = _service.RegistrarJefeCarrera(nuevoJefe, context, out string mensaje);
        JefeCarrera? jefe = context.JefeCarreras.Include(j => j.Usuario).FirstOrDefault(j => j.Usuario.Correo == "juan@gmail.com" && j.Usuario.Contrasenha == "12345" && j.Usuario.Ci == "78449678");

        //Validacion
        Assert.That(registrado);
        Assert.That(mensaje, Is.EqualTo("Jefe registrado correctamente"));
        Assert.That(jefe?.Usuario.Ci, Is.EqualTo("78449678"));
    }


    [Test]
    public void RegistrarJefeConEmailOcupadoTest()
    {
        //Configuracion
        JefeCarreraRegistroDTO nuevoJefe = new JefeCarreraRegistroDTO {
            Nombre = "Juan Martinez",
            Telefono = "55544466",
            CI = "78449678",
            FechaNacimiento = DateTime.Now.AddMonths(-22),
            Correo = "matias@gmail.com",
            Contrasenha = "12345",
            Carreras = new List<string>(){"PSI", "ISW"}
        };

        //Ejecucion
        bool registrado = _service.RegistrarJefeCarrera(nuevoJefe, context, out string mensaje);
      
        //Validacion
        Assert.That(!registrado);
        Assert.That(mensaje, Is.EqualTo("El email, el numero de telefono o el carnet de identidad ya esta en uso. Intentalo otra vez"));
    }


    [Test]
    public void RegistrarJefeConNumeroOcupadoTest()
    {
        //Configuracion
        JefeCarreraRegistroDTO nuevoJefe = new JefeCarreraRegistroDTO {
            Nombre = "Juan Martinez",
            Telefono = "7248909",
            CI = "78449678",
            FechaNacimiento = DateTime.Now.AddMonths(-22),
            Correo = "juan@gmail.com",
            Contrasenha = "12345",
            Carreras = new List<string>(){"PSI", "ISW"}
        };

        //Ejecucion
        bool registrado = _service.RegistrarJefeCarrera(nuevoJefe, context, out string mensaje);
        
        //Validacion
        Assert.That(!registrado);
        Assert.That(mensaje, Is.EqualTo("El email, el numero de telefono o el carnet de identidad ya esta en uso. Intentalo otra vez"));
    }


    [Test]
    public void RegistrarJefeConCarnetIdentidadOcupadoTest()
    {
        //Configuracion
        JefeCarreraRegistroDTO nuevoJefe = new JefeCarreraRegistroDTO {
            Nombre = "Juan Martinez",
            Telefono = "55544466",
            CI = "22222222",
            FechaNacimiento = DateTime.Now.AddMonths(-22),
            Correo = "juan@gmail.com",
            Contrasenha = "12345",
            Carreras = new List<string>(){"PSI", "ISW"}
        };

        //Ejecucion
        bool registrado = _service.RegistrarJefeCarrera(nuevoJefe, context, out string mensaje);
       
        //Validacion
        Assert.That(!registrado);
        Assert.That(mensaje, Is.EqualTo("El email, el numero de telefono o el carnet de identidad ya esta en uso. Intentalo otra vez"));
    }

    [Test]
    public void RegistrarJefeConCarrerasNoRegistradasTest()
    {
        //Configuracion
        JefeCarreraRegistroDTO nuevoJefe = new JefeCarreraRegistroDTO {
            Nombre = "Juan Martinez",
            Telefono = "77112253",
            CI = "12345612",
            FechaNacimiento = DateTime.Now.AddMonths(-22),
            Correo = "juan@gmail.com",
            Contrasenha = "12345",
            Carreras = new List<string>(){"OOO", "III"}
        };

        //Ejecucion
        bool registrado = _service.RegistrarJefeCarrera(nuevoJefe, context, out string mensaje);
       
        //Validacion
        Assert.That(!registrado);
        Assert.That(mensaje, Is.EqualTo("Hubo un error al selecccionar las carreras. Intentelo otra vez"));
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

        List<Carrera> carreras = new List<Carrera>()
        {
            new Carrera{ CarreraId = 1, NombreCarrera = "Ingenieria de software", Sigla = "ISW"},
            new Carrera{ CarreraId = 2, NombreCarrera = "Psicologia", Sigla = "PSI"}
        };

        context.Carreras.AddRange(carreras);
        
        List<JefeCarrera> jefes = new List<JefeCarrera>{
            new JefeCarrera{JefeCarreraId = 1, UsuarioId = 3}
        };


        context.JefeCarreras.AddRange(jefes);

        context.SaveChanges();

    }


    
}