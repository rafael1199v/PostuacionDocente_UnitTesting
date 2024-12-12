using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PostulacionDocente.ServicesApp.Models;

namespace AppTest;

public class VacanteServiceTest
{
    private static DbContextOptions<PostulacionDocenteContext> dbContextOptions = new DbContextOptionsBuilder<PostulacionDocenteContext>()
        .UseInMemoryDatabase(databaseName: "VacanteDbTest")
        .Options;


    private PostulacionDocenteContext _context;
    private IVacanteService _service;

    [SetUp]
    public void SetUp()
    {
        _service = new VacanteService();
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
    public void CrearVacanteTest()
    {
        //Configuracion
        NuevaVacanteDTO nuevaVacante = new NuevaVacanteDTO{
            NombreVacante = "AYUDAAAAAAAAAAA",
            SiglaMateria = "PRO-I",
            DescripcionVacante = "Alguien venga y ayúdeme!!!!!",
            FechaInicio = DateTime.Now,
            FechaFinalizacion = DateTime.Now.AddDays(10),
            JefeCI = "22222222"
        };

        //Ejecucion
        bool vacanteCreadaCorrectamente = _service.CrearVacante(nuevaVacante, _context, out string mensaje);
        Vacante? vacante = _context.Vacantes.FirstOrDefault(p => p.NombreVacante == nuevaVacante.NombreVacante && p.Descripcion == nuevaVacante.DescripcionVacante);

        Assert.That(vacanteCreadaCorrectamente);
        Assert.That(mensaje, Is.EqualTo("Vacante creada correctamente"));
        Assert.That(vacante != null);

    }

    [Test]
    public void VacanteFechaIncorrectaTest()
    {
        //Configuracion
        NuevaVacanteDTO nuevaVacante = new NuevaVacanteDTO{
            NombreVacante = "SE NECESITA PROFE",
            SiglaMateria = "PRO-I",
            DescripcionVacante = "Acabo de darme cuenta que puedo usar la misma vacante xd",
            FechaInicio = DateTime.Now.AddDays(10),
            FechaFinalizacion = DateTime.Now.AddDays(1),
            JefeCI = "22222222"
        };

        //Ejecucion
        bool vacanteCorrecta = _service.VacanteValida(nuevaVacante);
        Assert.That(!vacanteCorrecta);

    }

    [Test]
    public void VacanteSinDescripcionTest()
    {
        //Configuracion
        NuevaVacanteDTO nuevaVacante = new NuevaVacanteDTO{
            NombreVacante = "SE NECESITA PROFE",
            SiglaMateria = "PRO-I",
            DescripcionVacante = "",
            FechaInicio = DateTime.Now,
            FechaFinalizacion = DateTime.Now.AddDays(10),
            JefeCI = "22222222"
        };

        //Ejecucion
        bool vacanteCorrecta = _service.VacanteValida(nuevaVacante);
        Assert.That(!vacanteCorrecta);

    }

    [Test]
    public void CrearVacanteSinJefeTest()
    {
        //Configuracion
        NuevaVacanteDTO nuevaVacante = new NuevaVacanteDTO{
            NombreVacante = "AYUDAAAAAAAAAAA",
            SiglaMateria = "PRO-I",
            DescripcionVacante = "Alguien venga y ayúdeme!!!!!",
            FechaInicio = DateTime.Now,
            FechaFinalizacion = DateTime.Now.AddDays(10)
        };

        //Ejecucion
        bool vacanteCreadaCorrectamente = _service.CrearVacante(nuevaVacante, _context, out string mensaje);

        Assert.That(!vacanteCreadaCorrectamente);
        Assert.That(mensaje, Is.EqualTo("Hubo un error al crear la vacante. Intentalo otra vez"));

    }

    [Test]
    public void ObtenerVacantesTest()
    {
        //Config
        List<VacanteDTO> dTOs = new List<VacanteDTO>(){
            new VacanteDTO{ VacanteId = 1, NombreVacante = "Programacion I", DescripcionVacante = "Vacante programacion I", FechaInicio = DateTime.Now.AddDays(-10), FechaFinalizacion = DateTime.Now.AddDays(10), Materia="Programacion I"},
            new VacanteDTO{ VacanteId = 2, NombreVacante = "Programacion II", DescripcionVacante = "Vacante programacion II", FechaInicio = DateTime.Now.AddDays(-10), FechaFinalizacion = DateTime.Now.AddDays(5), Materia="Pensamiento Critico"}
        };

        //Ejecucion
        List<VacanteDTO> vacantesObtenidas = _service.ConseguirVacantesDisponibles(_context, "22222222");
        Assert.That(vacantesObtenidas.Count, Is.EqualTo(dTOs.Count));
        for (int i = 0; i < dTOs.Count; i++)
        {
            Assert.That(vacantesObtenidas[i].VacanteId, Is.EqualTo(dTOs[i].VacanteId));
            Assert.That(vacantesObtenidas[i].NombreVacante, Is.EqualTo(dTOs[i].NombreVacante));
            Assert.That(vacantesObtenidas[i].Materia, Is.EqualTo(dTOs[i].Materia));
            Assert.That(vacantesObtenidas[i].FechaInicio.Date, Is.EqualTo(dTOs[i].FechaInicio.Date));
            Assert.That(vacantesObtenidas[i].FechaFinalizacion.Date, Is.EqualTo(dTOs[i].FechaFinalizacion.Date));
        }
    }

    [Test]
    public void ObtenerDetallesVacanteTest()
    {
        //Config
        VacanteDTO vacanteTest = new VacanteDTO(){
            VacanteId = 1,
            NombreVacante = "Programacion I",
            DescripcionVacante = "Vacante programacion I",
            FechaInicio = DateTime.Now.AddDays(-10),
            FechaFinalizacion = DateTime.Now.AddDays(10),
            Materia = "Programacion I"
        };

        //Ejecucion
        VacanteDTO? vacanteObtenida = _service.ConseguirDetalleVacante(_context, 1);

        Assert.That(vacanteObtenida.VacanteId, Is.EqualTo(vacanteTest.VacanteId));
        Assert.That(vacanteObtenida.NombreVacante, Is.EqualTo(vacanteTest.NombreVacante));
        Assert.That(vacanteObtenida.Materia, Is.EqualTo(vacanteTest.Materia));
        Assert.That(vacanteObtenida.FechaInicio.Date, Is.EqualTo(vacanteTest.FechaInicio.Date));
        Assert.That(vacanteObtenida.FechaFinalizacion.Date, Is.EqualTo(vacanteTest.FechaFinalizacion.Date));
    }

    [Test]
    public void ObtenerVacantesVigentesJefeTest()
    {
        //Config
        List<VacanteJefeCarreraDTO> dTOs = new List<VacanteJefeCarreraDTO>(){
            new VacanteJefeCarreraDTO{ VacanteId = 1, NombreVacante = "Programacion I", DescripcionVacante = "Vacante programacion I", NombreMateria="Programacion I", NumeroPostulantes = 0},
            new VacanteJefeCarreraDTO{ VacanteId = 2, NombreVacante = "Programacion II", DescripcionVacante = "Vacante programacion II", NombreMateria="Pensamiento Critico", NumeroPostulantes = 0}
        };

        //Ejecucion
        List<VacanteJefeCarreraDTO> vacantesObtenidas = _service.ConseguirVacantesVigentesJefe(_context, "22222222");
        Assert.That(vacantesObtenidas.Count, Is.EqualTo(dTOs.Count));
        for (int i = 0; i < dTOs.Count; i++)
        {
            Assert.That(vacantesObtenidas[i].VacanteId, Is.EqualTo(dTOs[i].VacanteId));
            Assert.That(vacantesObtenidas[i].NombreVacante, Is.EqualTo(dTOs[i].NombreVacante));
            Assert.That(vacantesObtenidas[i].NombreMateria, Is.EqualTo(dTOs[i].NombreMateria));
            Assert.That(vacantesObtenidas[i].NumeroPostulantes, Is.EqualTo(dTOs[i].NumeroPostulantes));
        }
    }

    [Test]
    public void ObtenerVacantesHistorialJefeTest()
    {
        //Config
        List<VacanteJefeCarreraDTO> dTOs = new List<VacanteJefeCarreraDTO>(){
            new VacanteJefeCarreraDTO{ VacanteId = 1, NombreVacante = "Programacion I", DescripcionVacante = "Vacante programacion I", NombreMateria="Programacion I", NumeroPostulantes = 0},
            new VacanteJefeCarreraDTO{ VacanteId = 2, NombreVacante = "Programacion II", DescripcionVacante = "Vacante programacion II", NombreMateria="Pensamiento Critico", NumeroPostulantes = 0}
        };

        //Ejecucion
        List<VacanteJefeCarreraDTO> vacantesObtenidas = _service.ConseguirVacanteHistorialJefe(_context, "22222222");
        Assert.That(vacantesObtenidas.Count, Is.EqualTo(0));
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

        List<Materium> materias = new List<Materium>(){
            new Materium{ MateriaId = 1, NombreMateria = "Programacion I", Sigla = "PRO-I"},
            new Materium{ MateriaId = 2, NombreMateria = "Pensamiento Critico", Sigla = "PSC"},
            new Materium{ MateriaId = 3, NombreMateria = "Programacion superior", Sigla = "PS"},
            new Materium{ MateriaId = 4, NombreMateria = "Anatomia humana", Sigla = "ANT"},
        };

        _context.Materia.AddRange(materias);

        List<Vacante> vacantes = new List<Vacante>()
        {
            new Vacante{ VacanteId = 1, NombreVacante = "Programacion I", Descripcion = "Vacante programacion I", FechaInicio = DateTime.Now.AddDays(-10), FechaFin = DateTime.Now.AddDays(10), MateriaId = 1, JefeCarreraId = 1},
            new Vacante{ VacanteId = 2, NombreVacante = "Programacion II", Descripcion = "Vacante programacion II", FechaInicio = DateTime.Now.AddDays(-10), FechaFin = DateTime.Now.AddDays(5), MateriaId = 2, JefeCarreraId = 1}
        };

        _context.Vacantes.AddRange(vacantes);

        _context.SaveChanges();

    }



}