using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Api.Models;

namespace Api.Services
{
    public class MovieService
    {
        private readonly IMongoCollection<Movie> _movie;

        public MovieService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _movie = database.GetCollection<Movie>(settings.Value.CollectionName);
        }

        public List<Movie> Get() => _movie.Find(movie => true).ToList();

        public Movie Get(string id) => _movie.Find<Movie>(movie => movie.Id == id).FirstOrDefault();

        public Movie Create(Movie movie)
        {
            _movie.InsertOne(movie);
            return movie;
        }

        public void Update(string id, Movie movie) => _movie.ReplaceOne(p => p.Id == id, movie);

        public void Remove(string id) => _movie.DeleteOne(movie => movie.Id == id);
    }
}
