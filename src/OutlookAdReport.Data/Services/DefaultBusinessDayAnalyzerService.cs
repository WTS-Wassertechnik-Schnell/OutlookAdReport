using OutlookAdReport.Data.Models;

namespace OutlookAdReport.Data.Services;

/// <summary> A service for accessing default business day analyzers information.</summary>
public class DefaultBusinessDayAnalyzerService : IBusinessDayAnalyzerService
{
    /// <summary> Enumerates analyze in this collection.</summary>
    /// <param name="appointments"> The appointments. </param>
    /// <returns>An enumerator that allows foreach to be used to process analyze in this collection.</returns>
    public IEnumerable<IBusinessDay> Analyze(IEnumerable<IAppointment> appointments)
    {
        return appointments
            .GroupBy(a => a.Start.Date)
            .Select(g => new DefaultBusinessDay(this, g));
    }
}