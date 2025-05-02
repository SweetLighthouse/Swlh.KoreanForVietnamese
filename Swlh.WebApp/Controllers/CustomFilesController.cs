using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Swlh.WebApp.Context;
using Swlh.WebApp.Domain.Entities;
using System.Data;

namespace Swlh.WebApp.Controllers;

public class CustomFilesController(MainDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var files = await context.CustomFiles
            .OrderByDescending(f => f.CreatedAt)
            .Select(f => new CustomFile
            {
                Id = f.Id,
                FileName = f.FileName,
                ContentType = f.ContentType,
                CreatedAt = f.CreatedAt
                // Do NOT include FileData
            })
            .ToListAsync();

        return View(files);
    }


    //public async Task<IActionResult> Details(Guid? id)
    //{
    //    if (id == null) return NotFound();
    //    var customFile = await context.CustomFiles
    //        .AsNoTracking()
    //        .Where(m => m.Id == id)
    //        .Select(m => new { m.FileData, m.ContentType })
    //        .FirstOrDefaultAsync(); // <-- câu lệnh tốn thời gian
    //    // thử nghiệm trên file 40MB: 
    //    // debug: câu lệnh tốn rất nhiều thời gian. (15 phút hoặc hơn)
    //    // copy câu lệnh được compile để chạy ngay trong Microsoft SQL Server Management Studio (SSMS)
    //    // để kiểm tra tốc độ, chỉ mất 400ms

    //    if (customFile == null) return NotFound();

    //    var stream = new MemoryStream(customFile.FileData);
    //    return File(stream, customFile.ContentType);
    //}

    //public async Task<IActionResult> Details(Guid? id)
    //{
    //    if(id == null || id == Guid.Empty) return NotFound();
    //    using var conn = new SqlConnection(context.Database.GetConnectionString());
    //    await conn.OpenAsync();

    //    using var cmd = new SqlCommand("SELECT FileData, ContentType FROM CustomFile WHERE Id = @Id", conn);
    //    cmd.Parameters.AddWithValue("@Id", id);

    //    using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
    //    if (!await reader.ReadAsync()) return NotFound();

    //    const int bufferSize = 81920; // 80KB
    //    using var stream = new MemoryStream();

    //    long bytesRead = 0;
    //    long fieldOffset = 0;
    //    byte[] buffer = new byte[bufferSize];
    //    long totalBytes;

    //    do
    //    {
    //        totalBytes = reader.GetBytes(0, fieldOffset, buffer, 0, buffer.Length);
    //        stream.Write(buffer, 0, (int)totalBytes);
    //        fieldOffset += totalBytes;
    //    } while (totalBytes > 0);

    //    stream.Position = 0;
    //    string contentType = reader.GetString(1);

    //    return File(stream, contentType);
    //}

    //[HttpGet]
    //public async Task<IActionResult> Details(Guid? id)
    //{
    //    if (id == null || id == Guid.Empty) return NotFound();

    //    var conn = new SqlConnection(context.Database.GetConnectionString());
    //    await conn.OpenAsync();

    //    var cmd = new SqlCommand("SELECT ContentType, FileData FROM CustomFile WHERE Id = @Id", conn);
    //    cmd.Parameters.AddWithValue("@Id", id);

    //    var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection);
    //    if (!await reader.ReadAsync()) return NotFound();

    //    string contentType = reader.GetString(0); 

    //    // Stream without buffering entire file
    //    var stream = new CustomDbStream(reader, conn);

    //    return File(stream, contentType);
    //}

    [HttpGet]
    public async Task Details(Guid? id)
    {
        if (id == null || id == Guid.Empty)
        {
            Response.StatusCode = 400;
            return;
        }

        await using var conn = new SqlConnection(context.Database.GetConnectionString());
        await conn.OpenAsync();

        string sqlWhere = "@Id";
        var cmd = new SqlCommand(
            $"SELECT {nameof(CustomFile.ContentType)}, {nameof(CustomFile.FileData)} " +
            $"FROM {nameof(CustomFile)} " +
            $"WHERE {nameof(CustomFile.Id)} = {sqlWhere}",
            conn);
        cmd.Parameters.AddWithValue(sqlWhere, id);

        // tested, work just fine, but chatgpt insisted on not using it
        //var query = context.CustomFiles
        //    .Where(f => f.Id == id)
        //    .Select(f => new { f.ContentType, f.FileData });
        //var cmd2 = new SqlCommand(query.ToQueryString(), conn); 


        await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection);
        if (!await reader.ReadAsync())
        {
            Response.StatusCode = 404;
            return;
        }

        Response.ContentType = reader.GetString(reader.GetOrdinal(nameof(CustomFile.ContentType)));

        int fileDataOrdinal = reader.GetOrdinal(nameof(CustomFile.FileData));
        byte[] buffer = new byte[81920];
        long offset = 0;
        while (true)
        {
            long bytesRead = reader.GetBytes(fileDataOrdinal, offset, /*out*/ buffer, 0, buffer.Length);
            if (bytesRead == 0) break;

            await Response.Body.WriteAsync(buffer.AsMemory(0, (int)bytesRead));
            offset += bytesRead;
        }
    }






    //[HttpGet]
    //public async Task<IActionResult> Details(Guid id)
    //{
    //    var query = "SELECT FileData, ContentType FROM CustomFile WHERE Id = @id";
    //    using (var command = context.Database.GetDbConnection().CreateCommand())
    //    {
    //        command.CommandText = query;
    //        command.Parameters.Add(new SqlParameter("@id", id));

    //        await context.Database.OpenConnectionAsync();

    //        using (var reader = await command.ExecuteReaderAsync())
    //        {
    //            if (await reader.ReadAsync())
    //            {
    //                var contentType = reader["ContentType"].ToString();
    //                var fileStream = reader.GetStream(reader.GetOrdinal("FileData"));

    //                return File(fileStream, contentType);
    //            }
    //        }
    //    }

    //    return NotFound();
    //}

    //public IActionResult Create()
    //{
    //    return View();
    //}

    [HttpPost]
    [ValidateAntiForgeryToken]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Create(IFormFile file) // file is null
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream); // i got null

            var customFile = new CustomFile
            {
                FileName = file.FileName,
                FileData = memoryStream.ToArray(),
                ContentType = file.ContentType
            };

            context.CustomFiles.Add(customFile);
            await context.SaveChangesAsync();

            var uploadedPath = Url.Action("Details", "CustomFiles", new { id = customFile.Id });

            var referer = Request.Headers["Referer"].ToString();
            var accept = Request.Headers["Accept"].ToString();

            // If it's from TinyMCE or another programmatic source
            if (string.IsNullOrEmpty(referer) || !referer.Contains("/CustomFiles", StringComparison.OrdinalIgnoreCase) || accept.Contains("application/json"))
            {
                return Json(new { location = uploadedPath });
            }

            TempData["Msg"] = "Upload file thành công.";
        }
        catch
        {
            TempData["Msg"] = "Upload file thất bại.";
        }

        return RedirectToAction(nameof(Index));
    }


    // POST: CustomFiles/Delete/5
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var customFile = new CustomFile { Id = id };
        context.Entry(customFile).State = EntityState.Deleted;

        try
        {
            await context.SaveChangesAsync();
            TempData["Msg"] = "Xoá file thành công.";
        }
        catch (DbUpdateConcurrencyException)
        {
            TempData["Msg"] = "Không tìm thấy file.";
        }

        return RedirectToAction(nameof(Index));
    }

}
