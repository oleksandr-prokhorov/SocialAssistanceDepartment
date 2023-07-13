using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QueriesToDBTest.Models;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace QueriesToDBTest.Pages.Client
{
    public class VisitorDisabledCertificateModel : PageModel
    {
        private readonly ApplicationContext _context;
        [BindProperty]
        public DisabledPersonCertificate disabledCertificate { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }
        static string connectString;
        static NpgsqlConnection connection;
        public string[] disability_groups = new[] { "I", "II", "III"};
        string sql;
        public VisitorDisabledCertificateModel(ApplicationContext db)
        {
            _context = db;
        }
        public void OnGet(int? id)
        {
            Id = (int)id;
            var visitor = _context.visitor.FromSqlInterpolated($"select * from visitor where visitor_no = {Id}").SingleOrDefault();
            connectString = $"Host=localhost;Username={visitor.login};Password=visitor;Database=DiplomaProject";
            connection = new NpgsqlConnection(connectString);
        }
        public IActionResult OnPost(int? id)
        {
            Id = (int)id;
            if (ModelState.IsValid)
            {
                connection.Open();
                sql = $"insert into disabledpersoncertificate(date_of_birth, f_name, patronymic, l_name, guardian_no, disability_group) values(\'{disabledCertificate.date_of_birth.ToString("yyyy-MM-dd")}\', " +
                    $"\'{disabledCertificate.f_name}\', \'{disabledCertificate.patronymic}\', \'{disabledCertificate.l_name}\', '{Id}', \'{disabledCertificate.disability_group}\')";
                var cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                Message = $"���������� ������ ������� �� �������";
                return Page();
            }
            return Page();
        }
    }
}
