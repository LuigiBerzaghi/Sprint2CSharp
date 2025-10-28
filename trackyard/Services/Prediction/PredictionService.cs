using Microsoft.ML;
using Microsoft.ML.Data;

namespace Sprint1CSharp.Services.Prediction
{
    public class VeiculoRiskData
    {
        public float Ano { get; set; }
        public float Quilometragem { get; set; }
        public bool HighRisk { get; set; }
    }

    public class VeiculoRiskPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Predicted { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }

    public interface IPredictionService
    {
        VeiculoRiskPrediction Predict(int ano, int quilometragem);
    }

    public class PredictionService : IPredictionService
    {
        private readonly MLContext _ml;
        private readonly ITransformer _model;
        private readonly PredictionEngine<VeiculoRiskData, VeiculoRiskPrediction> _engine;

        public PredictionService()
        {
            _ml = new MLContext(seed: 123);
            var data = _ml.Data.LoadFromEnumerable(GenerateTraining());

            var pipeline = _ml.Transforms.Concatenate("Features", nameof(VeiculoRiskData.Ano), nameof(VeiculoRiskData.Quilometragem))
                .Append(_ml.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: nameof(VeiculoRiskData.HighRisk)));

            _model = pipeline.Fit(data);
            _engine = _ml.Model.CreatePredictionEngine<VeiculoRiskData, VeiculoRiskPrediction>(_model);
        }

        public VeiculoRiskPrediction Predict(int ano, int quilometragem)
        {
            var input = new VeiculoRiskData
            {
                Ano = ano,
                Quilometragem = quilometragem
            };
            return _engine.Predict(input);
        }

        private static IEnumerable<VeiculoRiskData> GenerateTraining()
        {
            var rnd = new Random(42);
            for (int ano = 2000; ano <= DateTime.UtcNow.Year; ano++)
            {
                for (int i = 0; i < 30; i++)
                {
                    var km = rnd.Next(5_000, 180_000);
                    var highRisk = (ano < 2018 && km > 40_000) || (km > 120_000);
                    yield return new VeiculoRiskData { Ano = ano, Quilometragem = km, HighRisk = highRisk };
                }
            }
        }
    }
}
