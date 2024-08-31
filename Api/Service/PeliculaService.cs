using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Api.Models;

namespace Api.Services
{
    public class PeliculaService
    {
        private readonly IMongoCollection<Pelicula> _peliculas;

        public PeliculaService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _peliculas = database.GetCollection<Pelicula>(settings.Value.CollectionName);
        }

        public List<Pelicula> Get() => _peliculas.Find(pelicula => true).ToList();

        public Pelicula Get(string id) => _peliculas.Find<Pelicula>(pelicula => pelicula.Id == id).FirstOrDefault();

        public Pelicula Create(Pelicula pelicula)
        {
            _peliculas.InsertOne(pelicula);
            return pelicula;
        }

        public void Update(string id, Pelicula pelicula) => _peliculas.ReplaceOne(p => p.Id == id, pelicula);

        public void Remove(string id) => _peliculas.DeleteOne(pelicula => pelicula.Id == id);
    }
}
