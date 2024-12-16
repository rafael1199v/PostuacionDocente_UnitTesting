using Microsoft.EntityFrameworkCore;
using PostulacionDocente.ServicesApp.Models;

namespace AppTest;
public class ExamenFinalTest
{
    private static DbContextOptions<PostulacionDocenteContext> dbContextOptions = new DbContextOptionsBuilder<PostulacionDocenteContext>()
        .UseInMemoryDatabase(databaseName: "ExamenDbTest")
        .Options;
    private PostulacionDocenteContext context;
    private IJefeCarreraService _service;

    [SetUp]
    public void SetUp()
    {
        context = new PostulacionDocenteContext(dbContextOptions);
        _service = new JefeCarreraService();
        context.Database.EnsureCreated();

        SeedDatabase();
    }

    [TearDown]
    public void CleanUp()
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }

    //Descender una postulacion normalmente
    [Test]
    public void ExamenFinalTest1()
    {


        //Configurar

        int postulacionId = 1;

        //Ejecutar

        
        bool descendido = _service.DescenderSolicitud(context, postulacionId, out string mensaje);
        
        //Validar

        Assert.That(descendido);
        Assert.That(mensaje, Is.EqualTo("Postulacion descendida correctamente"));
    }

    //Id indefinido
    [Test]
    public void ExamenFinalTest2()
    {
          //Configurar

        int postulacionId = -1;

        //Ejecutar

        
        bool descendido = _service.DescenderSolicitud(context, postulacionId, out string mensaje);
        
        //Validar

        Assert.That(!descendido);
        Assert.That(mensaje, Is.EqualTo("Hubo un error al descender la postulacion. Intentelo otra vez"));
    }

    //Con id menor o igual a 1
    [Test]
    public void ExamenFinalTest3()
    {
        //Configurar

        int postulacionId = 3;

        //Ejecutar
        bool descendido = _service.DescenderSolicitud(context, postulacionId, out string mensaje);
        
        //Validar

        Assert.That(!descendido);
        Assert.That(mensaje, Is.EqualTo("No se puede descender mas la postulacion. Espera a que vuelva a ser ascendida"));
    }

    //Con estado mayor o igual a 3
    [Test]
    public void ExamenFinalTest4()
    {
        //Configurar

        int postulacionId = 4;

        //Ejecutar
        bool descendido = _service.DescenderSolicitud(context, postulacionId, out string mensaje);
        
        //Validar
        Assert.That(!descendido);
        Assert.That(mensaje, Is.EqualTo("No se puede modificar el estado de la postulacion"));
    }

    




    private void SeedDatabase()
    {
        List<Usuario> usuarios = new List<Usuario>(){
            new Usuario{ UsuarioId = 1, Nombre = "Rafael", Ci = "13776453", FechaNacimiento = DateTime.Now.AddDays(-100), NumeroTelefono = "7648909", Correo = "rafael1199v@gmail.com", Contrasenha = "1234"},
            new Usuario{ UsuarioId = 2, Nombre = "Daniel", Ci = "11111111", FechaNacimiento = DateTime.Now.AddDays(-500), NumeroTelefono = "7638909", Correo = "daniel@gmail.com", Contrasenha = "12345"},
            new Usuario{ UsuarioId = 3, Nombre = "Matias", Ci = "22222222", FechaNacimiento = DateTime.Now.AddDays(-1000), NumeroTelefono = "7248909", Correo = "matias@gmail.com", Contrasenha = "12346"},
            new Usuario{ UsuarioId = 4, Nombre = "Examen", Ci = "99999999", FechaNacimiento = DateTime.Now.AddDays(-1000), NumeroTelefono = "9999999", Correo = "examen@gmail.com", Contrasenha = "12346"}
        };

        context.Usuarios.AddRange(usuarios);

        List<Docente> docentes = new List<Docente>(){
            new Docente {DocenteId = 1, Especialidad = "Programacion", Experiencia = 0, DescripcionPersonal = "Hola soy rafael", Grado = "Ingeniero", UsuarioId = 1},
            new Docente {DocenteId = 2, Especialidad = "Computacion grafica", Experiencia = 3, DescripcionPersonal = "Hola soy daniel", Grado = "Ingeniero", UsuarioId = 2},
            new Docente {DocenteId = 3, Especialidad = "Examen final", Experiencia = 3, DescripcionPersonal = "Hola soy examen final", Grado = "Final", UsuarioId = 4}
        };


        context.Docentes.AddRange(docentes);

        List<JefeCarrera> jefes = new List<JefeCarrera>{
            new JefeCarrera{JefeCarreraId = 1, UsuarioId = 3}
        };

        context.JefeCarreras.AddRange(jefes);

        List<Materium> materias = new List<Materium>(){
            new Materium{ MateriaId = 1, NombreMateria = "Programacion I", Sigla = "PRO-I"},
            new Materium{ MateriaId = 2, NombreMateria = "Pensamiento Critico", Sigla = "PSC"}
        };

        context.Materia.AddRange(materias);

        List<Carrera> carreras = new List<Carrera>()
        {
            new Carrera{ CarreraId = 1, NombreCarrera = "Ingenieria de software", Sigla = "ISW"},
            new Carrera{ CarreraId = 2, NombreCarrera = "Psicologia", Sigla = "PSI"}
        };


        carreras[0].Materia.Add(materias[0]);
        carreras[1].Materia.Add(materias[1]);

        context.Carreras.AddRange(carreras);

        List<Vacante> vacantes = new List<Vacante>()
        {
            new Vacante{ VacanteId = 1, NombreVacante = "Programacion I", Descripcion = "Vacante programacion I", FechaInicio = DateTime.Now.AddDays(-10), FechaFin = DateTime.Now.AddDays(10), MateriaId = 1, JefeCarreraId = 1},
            new Vacante{ VacanteId = 2, NombreVacante = "Programacion II", Descripcion = "Vacante programacion II", FechaInicio = DateTime.Now.AddDays(-10), FechaFin = DateTime.Now.AddDays(5), MateriaId = 2, JefeCarreraId = 1},
            new Vacante{ VacanteId = 3, NombreVacante = "Programacion III", Descripcion = "Vacante programacion III", FechaInicio = DateTime.Now.AddDays(-10), FechaFin = DateTime.Now.AddDays(-1), MateriaId = 2, JefeCarreraId = 1}

        };

        context.Vacantes.AddRange(vacantes);

        List<Estado> estados = new List<Estado>()
        {
            new Estado{EstadoId = 1, Mensaje = "En revisión"},
            new Estado{EstadoId = 2, Mensaje = "Exposición"},
            new Estado{EstadoId = 3, Mensaje = "Entrevista"},
            new Estado{EstadoId = 4, Mensaje = "Aceptado"},
            new Estado{EstadoId = 5, Mensaje = "Rechazado"}
        };


        context.Estados.AddRange(estados);


        List<Postulacion> postulaciones = new List<Postulacion>()
        {
            new Postulacion{PostulacionId = 1, EstadoId = 3, DocenteId = 1, VacanteId  = 1},
            new Postulacion{PostulacionId = 2, EstadoId = 3, DocenteId = 2, VacanteId = 2},
            new Postulacion{PostulacionId = 3, EstadoId = 1, DocenteId = 1, VacanteId = 3},
            new Postulacion{PostulacionId = 4, EstadoId = 5, DocenteId = 1, VacanteId = 2}
        };

        context.Postulacions.AddRange(postulaciones);

        context.SaveChanges();

    }
}