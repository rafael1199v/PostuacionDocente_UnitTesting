

using Microsoft.EntityFrameworkCore;
using PostulacionDocente.ServicesApp.Models;

namespace AppTest;
public class PostulacionServiceTest
{
    private static DbContextOptions<PostulacionDocenteContext> dbContextOptions = new DbContextOptionsBuilder<PostulacionDocenteContext>()
        .UseInMemoryDatabase(databaseName: "PostulacionDbTest")
        .Options;
    private PostulacionDocenteContext context;
    private IPostulacionService _service;

    [OneTimeSetUp]
    public void SetUp()
    {
        context = new PostulacionDocenteContext(dbContextOptions);
        _service = new PostulacionService();
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
    public void ConseguirDetallesPostulacionTest()
    {
        //Configuracion
        PostulacionDetallesDTO postulacionEsperada = new PostulacionDetallesDTO{
            PostulacionId = 1,
            TituloMateria = "Programacion I",
            Estado = 1,
            DescripcionEstado = "En revisión",
            NombreVacante = "Programacion I",
            DescripcionVacante = "Vacante programacion I",
            JefeCorreo = "matias@gmail.com",
            JefeNombre = "Matias"
        };
        int postulacionId = 1;

        //Ejecucion
        
        PostulacionDetallesDTO? postulacionDetalles = _service.ConseguirDetallesPostulacion(context, postulacionId);

        //Validacion
        Assert.That(postulacionDetalles, Is.Not.Null);
        Assert.That(postulacionDetalles?.PostulacionId, Is.EqualTo(postulacionEsperada.PostulacionId));
        Assert.That(postulacionDetalles?.TituloMateria, Is.EqualTo(postulacionEsperada.TituloMateria));
        Assert.That(postulacionDetalles?.Estado, Is.EqualTo(postulacionEsperada.Estado));
        Assert.That(postulacionDetalles?.DescripcionVacante, Is.EqualTo(postulacionEsperada.DescripcionVacante));
        Assert.That(postulacionDetalles?.NombreVacante, Is.EqualTo(postulacionEsperada.NombreVacante));
        Assert.That(postulacionDetalles?.DescripcionVacante, Is.EqualTo(postulacionEsperada.DescripcionVacante));
        Assert.That(postulacionDetalles?.JefeCorreo, Is.EqualTo(postulacionEsperada.JefeCorreo));
        Assert.That(postulacionDetalles?.JefeNombre, Is.EqualTo(postulacionEsperada.JefeNombre));
    }

    [Test]
    public void ConseguirDetallesPostulacionConPostulacionIdNoExistente()
    {
        //Configuracion
        int postulacionId = -1;
        //Ejecucion
        
        PostulacionDetallesDTO? postulacionDetalles = _service.ConseguirDetallesPostulacion(context, postulacionId);

        //Validacion
        Assert.That(postulacionDetalles, Is.Null);
    }

    [Test]
    public void ConseguirPostulacionesVigentesTest()
    {
        //Configuracion
        List<PostulacionDetallesDTO> postulacionDetallesEsperado = new List<PostulacionDetallesDTO>()
        {
            new PostulacionDetallesDTO{
                PostulacionId = 1,
                TituloMateria = "Programacion I",
                Estado = 1,
                DescripcionEstado = "En revisión",
                NombreVacante = "Programacion I",
                DescripcionVacante = "Vacante programacion I",
                JefeCorreo = "matias@gmail.com",
                JefeNombre = "Matias"
            }
        };

        string CI = "13776453";
        postulacionDetallesEsperado.Sort((x , y) => x.PostulacionId.CompareTo(y.PostulacionId));

        //Ejecucion
        
        List<PostulacionDetallesDTO> postulacionDetalles = _service.ConseguirPostulacionesVigentes(context, CI);
        postulacionDetalles.Sort((x, y) => x.PostulacionId.CompareTo(y.PostulacionId));
        //Validacion
      
        
        Assert.That(postulacionDetalles.Count, Is.Not.EqualTo(0));
        Assert.That(postulacionDetalles.Count, Is.EqualTo(postulacionDetallesEsperado.Count));
        
        for(int i = 0; i < postulacionDetallesEsperado.Count; i++)
        {
            Assert.That(postulacionDetalles?[i].PostulacionId, Is.EqualTo(postulacionDetallesEsperado[i].PostulacionId));
            Assert.That(postulacionDetalles?[i].TituloMateria, Is.EqualTo(postulacionDetallesEsperado[i].TituloMateria));
            Assert.That(postulacionDetalles?[i].Estado, Is.EqualTo(postulacionDetallesEsperado[i].Estado));
            Assert.That(postulacionDetalles?[i].DescripcionVacante, Is.EqualTo(postulacionDetallesEsperado[i].DescripcionVacante));
            Assert.That(postulacionDetalles?[i].NombreVacante, Is.EqualTo(postulacionDetallesEsperado[i].NombreVacante));
            Assert.That(postulacionDetalles?[i].DescripcionVacante, Is.EqualTo(postulacionDetallesEsperado[i].DescripcionVacante));
            Assert.That(postulacionDetalles?[i].JefeCorreo, Is.EqualTo(postulacionDetallesEsperado[i].JefeCorreo));
            Assert.That(postulacionDetalles?[i].JefeNombre, Is.EqualTo(postulacionDetallesEsperado[i].JefeNombre));
        }
       
        
        
    }


    [Test]
    public void ConseguirPostulacionVigentesSinCarnetValidoTest()
    {
        //Configuracion
        string CI = "-1";

        //Ejecucion
        List<PostulacionDetallesDTO> postulacionesVigentes = _service.ConseguirPostulacionesVigentes(context, CI);

        //Validacion
        Assert.That(postulacionesVigentes.Count, Is.EqualTo(0));

    }

    [Test]
    public void ConseguirPostulacionesHistorialTest()
    {
        //Configuracion
        List<PostulacionDetallesDTO> postulacionesHistorialEsperadas = new List<PostulacionDetallesDTO>()
        {
            new PostulacionDetallesDTO{
                PostulacionId = 3,
                TituloMateria = "Pensamiento Critico",
                Estado = 1,
                DescripcionEstado = "En revisión",
                NombreVacante = "Programacion III",
                DescripcionVacante = "Vacante programacion III",
                JefeCorreo = "matias@gmail.com",
                JefeNombre = "Matias"
            },
            new PostulacionDetallesDTO{
                PostulacionId = 4,
                TituloMateria = "Pensamiento Critico",
                Estado = 5,
                DescripcionEstado = "Rechazado",
                NombreVacante = "Programacion II",
                DescripcionVacante = "Vacante programacion II",
                JefeCorreo = "matias@gmail.com",
                JefeNombre = "Matias"
            }
        };

        postulacionesHistorialEsperadas.Sort((x, y) => x.PostulacionId.CompareTo(y.PostulacionId));
        string CI = "13776453";

        //Ejecucion
        List<PostulacionDetallesDTO> postulacionesHistorial = _service.ConseguirPostulacionesHistorial(context, CI);
        postulacionesHistorial.Sort((x, y) => x.PostulacionId.CompareTo(y.PostulacionId));


        //Validacion
        Assert.That(postulacionesHistorial.Count, Is.Not.EqualTo(0));
        Assert.That(postulacionesHistorial.Count, Is.EqualTo(postulacionesHistorialEsperadas.Count));
        

        for(int i = 0; i < postulacionesHistorialEsperadas.Count; i++)
        {
            Assert.That(postulacionesHistorial?[i].PostulacionId, Is.EqualTo(postulacionesHistorialEsperadas[i].PostulacionId));
            Assert.That(postulacionesHistorial?[i].TituloMateria, Is.EqualTo(postulacionesHistorialEsperadas[i].TituloMateria));
            Assert.That(postulacionesHistorial?[i].Estado, Is.EqualTo(postulacionesHistorialEsperadas[i].Estado));
            Assert.That(postulacionesHistorial?[i].DescripcionVacante, Is.EqualTo(postulacionesHistorialEsperadas[i].DescripcionVacante));
            Assert.That(postulacionesHistorial?[i].NombreVacante, Is.EqualTo(postulacionesHistorialEsperadas[i].NombreVacante));
            Assert.That(postulacionesHistorial?[i].DescripcionVacante, Is.EqualTo(postulacionesHistorialEsperadas[i].DescripcionVacante));
            //La parte de jefe de carrera no se utiliza en el historial
        }


    }

    [Test]
    public void ConseguirPostulacionesHistorialConCarnetInvalidoTest()
    {
        //Configuracion
        string CI = "-1";

        //Ejecucion
        List<PostulacionDetallesDTO> postulacionesHistorial = _service.ConseguirPostulacionesHistorial(context, CI);

        //Validacion
        Assert.That(postulacionesHistorial, Is.Empty);
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
            new Postulacion{PostulacionId = 1, EstadoId = 1, DocenteId = 1, VacanteId  = 1},
            new Postulacion{PostulacionId = 2, EstadoId = 4, DocenteId = 2, VacanteId = 2},
            new Postulacion{PostulacionId = 3, EstadoId = 1, DocenteId = 1, VacanteId = 3},
            new Postulacion{PostulacionId = 4, EstadoId = 5, DocenteId = 1, VacanteId = 2}
        };

        context.Postulacions.AddRange(postulaciones);

        context.SaveChanges();

    }
}