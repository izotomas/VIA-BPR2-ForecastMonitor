using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using MathNet.Numerics.LinearAlgebra;

namespace ForecastMonitor.Service.DomainLogic.Mapping.TypeConverters
{
    public class VectorConverter : 
        ITypeConverter<IEnumerable<DaoTimeSerie>, Vector<double>>, 
        ITypeConverter<IEnumerable<DaoPrediction>, Vector<double>>
    {
        public Vector<double> Convert(IEnumerable<DaoTimeSerie> source, Vector<double> destination, ResolutionContext context)
        {
            var doubles = source.Select(context.Mapper.Map<double>);
            var result = Vector<double>.Build.DenseOfEnumerable(doubles);
            return result;
        }

        public Vector<double> Convert(IEnumerable<DaoPrediction> source, Vector<double> destination, ResolutionContext context)
        {
            var doubles = source.Select(context.Mapper.Map<double>);
            var result = Vector<double>.Build.DenseOfEnumerable(doubles);
            return result;
        }
    }
}
