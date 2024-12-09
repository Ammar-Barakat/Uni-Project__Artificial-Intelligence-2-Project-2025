using System;
using System.Collections.Generic;

namespace bookhouseAPI.Models;

public partial class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }
    public string? Language { get; set; }
    public string? Genres { get; set; }
    public string? Format { get; set; }
    public string? Publisher { get; set; }
    public DateOnly? PublishDate { get; set; }
    public double? Pages { get; set; }
    public string? Cover { get; set; }
    public double? Rating { get; set; }
}
