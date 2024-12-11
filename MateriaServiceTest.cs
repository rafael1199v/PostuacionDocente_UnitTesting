// using PostulacionDocente.ServicesApp.Models;

using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using PostulacionDocente.ServicesApp.Models;


namespace AppTest;


public class MateriaServiceTest
{
   
    private static DbContextOptions<PostulacionDocenteContext> dbContextOptions = new DbContextOptionsBuilder<PostulacionDocenteContext>()
        .UseInMemoryDatabase(databaseName: "MateriaDbTest")
        .Options;
    private PostulacionDocenteContext context;
    private IMateriaService _service;

    [OneTimeSetUp]
    public void SetUp()
    {
        context = new PostulacionDocenteContext(dbContextOptions);
        _service = new MateriaService();
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
    public void ConseguirMateriasCantidadTest()
    {
        //Configuracion
        List<MateriaDTO> materias = new List<MateriaDTO>();

        //Ejecucion
        materias = _service.conseguirMaterias(context);

        //Validacion
        Assert.That(materias.Count == 4);
    }

    [Test]
    public void ConseguirMateriasNoNullTest()
    {
        //Configuracion
        List<MateriaDTO> materias;

        //Ejecucion
        materias = _service.conseguirMaterias(context);

        //Validacion
        Assert.That(materias != null);
    }

    [Test]
    public void ConseguirMateriasElementosIgualesTest()
    {
         //Configuracion
        List<MateriaDTO> materias;

        List<MateriaDTO> materiasEsperadas = new List<MateriaDTO>(){
            new MateriaDTO{ nombre = "Programacion I", sigla = "PRO-I"},
            new MateriaDTO{ nombre = "Pensamiento Critico", sigla = "PSC"},
            new MateriaDTO{ nombre = "Programacion superior", sigla = "PS"},
            new MateriaDTO{ nombre = "Anatomia humana", sigla = "ANT"},
        };

        materiasEsperadas.Sort((x, y) => x.nombre.CompareTo(y.nombre));


        //Ejecucion
        materias = _service.conseguirMaterias(context);
        materias.Sort((x, y) => x.nombre.CompareTo(y.nombre));

        //Validacion
        for(int i = 0; i < materiasEsperadas.Count; i++)
        {
           Assert.That(materias[i].nombre, Is.EqualTo(materiasEsperadas[i].nombre));
           Assert.That(materias[i].sigla, Is.EqualTo(materiasEsperadas[i].sigla));
        }
    }

    private void SeedDatabase()
    {
        List<Materium> materias = new List<Materium>(){
            new Materium{ MateriaId = 1, NombreMateria = "Programacion I", Sigla = "PRO-I"},
            new Materium{ MateriaId = 2, NombreMateria = "Pensamiento Critico", Sigla = "PSC"},
            new Materium{ MateriaId = 3, NombreMateria = "Programacion superior", Sigla = "PS"},
            new Materium{ MateriaId = 4, NombreMateria = "Anatomia humana", Sigla = "ANT"},
        };

        context.Materia.AddRange(materias);

        List<Carrera> carreras = new List<Carrera>()
        {
            new Carrera{ CarreraId = 1, NombreCarrera = "Ingenieria de software", Sigla = "ISW"},
            new Carrera{ CarreraId = 2, NombreCarrera = "Psicologia", Sigla = "PSI"},
            new Carrera{ CarreraId = 3, NombreCarrera = "Ingenieria mecatronica", Sigla = "IMT"},
            new Carrera{ CarreraId = 4, NombreCarrera = "Medicina", Sigla = "MED"},
        };

        carreras[0].Materia.Add(materias[0]);
        carreras[1].Materia.Add(materias[1]);
        carreras[2].Materia.Add(materias[2]);
        carreras[3].Materia.Add(materias[3]);


        context.Carreras.AddRange(carreras);
        context.SaveChanges();

    }


    
}