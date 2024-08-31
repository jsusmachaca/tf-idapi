using Microsoft.ML;
using Microsoft.ML.Data;
using Api.Models;
using Api.Services;
using System.Collections.Generic;
using System.Linq;

public class PeliculaSearchService
{
    private readonly List<Pelicula> _peliculas;
    private readonly MLContext _mlContext;
    private readonly ITransformer _model;

    public PeliculaSearchService(PeliculaService peliculaService)
    {
        _peliculas = peliculaService.Get();
        _mlContext = new MLContext();

        var dataView = _mlContext.Data.LoadFromEnumerable(_peliculas.Select(p => new TfidfData { Text = p.Description }));
        
        var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(TfidfData.Text));
        
        _model = pipeline.Fit(dataView);
    }

    public List<Pelicula> Search(string query)
    {
        var queryData = new TfidfData { Text = query };
        var queryTransformed = _model.Transform(_mlContext.Data.LoadFromEnumerable(new[] { queryData }));

        var queryFeatures = _mlContext.Data.CreateEnumerable<TfidfData>(queryTransformed, reuseRowObject: false).First().Features;

        var similarities = _peliculas.Select(p => 
        {
            var peliculaData = new TfidfData { Text = p.Description };
            var peliculaTransformed = _model.Transform(_mlContext.Data.LoadFromEnumerable(new[] { peliculaData }));
            var peliculaFeatures = _mlContext.Data.CreateEnumerable<TfidfData>(peliculaTransformed, reuseRowObject: false).First().Features;

            return new
            {
                Pelicula = p,
                Similarity = CosineSimilarity(queryFeatures, peliculaFeatures)
            };
        });

        return similarities
            .OrderByDescending(s => s.Similarity)
            .Select(s => s.Pelicula)
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

        return dotProduct / (float)(System.Math.Sqrt(magnitude1) * System.Math.Sqrt(magnitude2));
    }

    private class TfidfData
    {
        public string Text { get; set; }
        public float[] Features { get; set; }
    }
}
