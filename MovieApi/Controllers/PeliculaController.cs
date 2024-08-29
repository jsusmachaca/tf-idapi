using Microsoft.AspNetCore.Mvc;
using MovieApi.Models;
using MovieApi.Services;

[ApiController]
[Route("api/[controller]")]
public class PeliculaController : ControllerBase
{
    private readonly PeliculaService _peliculaService;
    private readonly PeliculaSearchService _peliculaSearchService;

    public PeliculaController(PeliculaService peliculaService, PeliculaSearchService peliculaSearchService)
    {
        _peliculaService = peliculaService;
        _peliculaSearchService = peliculaSearchService;
    }

    [HttpGet]
    public ActionResult<List<Pelicula>> Get() => _peliculaService.Get();

    [HttpGet("{id}")]
    public ActionResult<Pelicula> Get(string id)
    {
        var pelicula = _peliculaService.Get(id);

        if (pelicula == null)
        {
            return NotFound();
        }

        return pelicula;
    }

    [HttpGet("search/{query}")]
    public ActionResult<List<Pelicula>> Search(string query)
    {
        var results = _peliculaSearchService.Search(query);

        if (results == null || results.Count == 0)
        {
            return NotFound();
        }

        return results;
    }

    [HttpPost]
    public ActionResult<Pelicula> Create(Pelicula pelicula)
    {
        _peliculaService.Create(pelicula);
        return CreatedAtRoute("GetPelicula", new { id = pelicula.Id }, pelicula);
    }
}
