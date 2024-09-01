using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.Services;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly MovieService _movieService;
    private readonly MovieSearchService _movieSearchService;

    public MovieController(MovieService movieService, MovieSearchService movieSearchService)
    {
        _movieService = movieService;
        _movieSearchService = movieSearchService;
    }

    [HttpGet]
    public ActionResult<List<Movie>> Get() => _movieService.Get();

    [HttpGet("{id}")]
    public ActionResult<Movie> Get(string id)
    {
        var movie = _movieService.Get(id);

        if (movie == null)
        {
            return NotFound();
        }

        return movie;
    }

    [HttpGet("search/{query}")]
    public ActionResult<List<Movie>> Search(string query)
    {
        var results = _movieSearchService.Search(query);

        if (results == null || results.Count == 0)
        {
            return NotFound();
        }

        return results;
    }

    [HttpPost]
    public ActionResult<Movie> Create(Movie movie)
    {
        _movieService.Create(movie);
        return CreatedAtRoute("GetMovie", new { id = movie.Id }, movie);
    }
}
