using Microsoft.ML;
using Microsoft.ML.Data;
using Api.Models;
using Api.Services;
using System.Collections.Generic;
using System.Linq;

namespace Api.Services
{
    public class MovieSearchService
    {
        private readonly List<Movie> _movies;
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public MovieSearchService(MovieService movieService)
        {
            _movies = movieService.Get();
            _mlContext = new MLContext();

            var dataView = _mlContext.Data.LoadFromEnumerable(_movies.Select(p => new TfidfData { Text = p.Description }));
            
            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(TfidfData.Text));
            
            _model = pipeline.Fit(dataView);
        }

        public List<Movie> Search(string query)
        {
            var queryData = new TfidfData { Text = query };
            var queryTransformed = _model.Transform(_mlContext.Data.LoadFromEnumerable(new[] { queryData }));

            var queryFeatures = _mlContext.Data.CreateEnumerable<TfidfData>(queryTransformed, reuseRowObject: false).First().Features;

            var similarities = _movies.Select(p => 
            {
                var movieData = new TfidfData { Text = p.Description };
                var movieTransformed = _model.Transform(_mlContext.Data.LoadFromEnumerable(new[] { movieData }));
                var movieFeatures = _mlContext.Data.CreateEnumerable<TfidfData>(movieTransformed, reuseRowObject: false).First().Features;

                return new
                {
                    Movie = p,
                    Similarity = CosineSimilarity(queryFeatures, movieFeatures)
                };
            });

            return similarities
                .OrderByDescending(s => s.Similarity)
                .Select(s => s.Movie)
                .ToList();
        }

        private static float CosineSimilarity(float[] vector1, float[] vector2)
        {
            float dotProduct = 0;
            float magnitude1 = 0;
            float magnitude2 = 0;

            for (int i = 0; i < vector1.Length; i++)
            {
                dotProduct += vector1[i] * vector2[i];
                magnitude1 += vector1[i] * vector1[i];
                magnitude2 += vector2[i] * vector2[i];
            }
            if (magnitude1 == 0 || magnitude2 == 0)
                return 0;

            return dotProduct / (float)(System.Math.Sqrt(magnitude1) * System.Math.Sqrt(magnitude2));
        }

        private class TfidfData
        {
            public string Text { get; set; } = string.Empty;
            public float[] Features { get; set; } = new float[0];
        }
    }
}