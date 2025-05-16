using Swlh.WebApp.Domain.Entities;

namespace Swlh.WebApp.Application.Dtos;

public class SearchResultViewDto
{
    // từ khoá đã tìm kiếm
    public string Query { get; set; } = string.Empty;



    public string SuggestSearch { get; set; } = string.Empty;


    // danh sách từ tìm được
    public List<Word> Exacts { get; set; } = [];


    // danh sách từ gần giống
    public List<Word> Similars { get; set; } = [];


    // danh sách ví dụ
    public List<Example> Examples { get; set; } = [];
}
