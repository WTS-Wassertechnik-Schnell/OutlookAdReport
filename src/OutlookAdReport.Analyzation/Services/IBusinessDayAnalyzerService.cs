using OutlookAdReport.Analyzation.Models;
using OutlookAdReport.Data.Models;

namespace OutlookAdReport.Analyzation.Services;

/// <summary> Interface for business day analyzer service.</summary>
public interface IBusinessDayAnalyzerService
{
    /// <summary> Enumerates analyze in this collection.</summary>
    /// <param name="appointments"> The appointments. </param>
    /// <returns>An enumerator that allows foreach to be used to process analyze in this collection.</returns>
    public IEnumerable<IBusinessDay> Analyze(IEnumerable<IAppointment> appointments);
}