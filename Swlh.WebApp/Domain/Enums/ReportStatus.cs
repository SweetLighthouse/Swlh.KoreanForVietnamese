using System.ComponentModel.DataAnnotations;

namespace Swlh.WebApp.Domain.Enums;

public enum ReportStatus
{
    [Display(Name = "Đã nhận")]
    Pending,            // New, unsolved
    [Display(Name = "Đã giải quyết")]
    Resolved,           // Solved with an answer
    [Display(Name = "Đã từ chối")]
    Rejected            // Rejected by the admin
}
