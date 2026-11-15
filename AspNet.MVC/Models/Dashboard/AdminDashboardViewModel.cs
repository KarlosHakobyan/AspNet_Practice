
namespace AspNet.MVC.Models.Dashboard;

public class AdminDashboardViewModel
{
    public int? TotalProducts { get; set; }

    public int? TotalCategories { get; set; }

    public int? TotalTags { get; set; }

    public bool IsSystemHealthy { get; set; }

    public string SystemStatus =>
        IsSystemHealthy ? "Healthy" : "Unavailable";
}
