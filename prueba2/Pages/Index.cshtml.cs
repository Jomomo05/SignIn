using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using prueba2.model;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace prueba2.Pages
{
    public class IndexModel : PageModel
    {
        int check;

        public IList<aplicantes> Listaaplicantes { get; set; }
        [BindProperty]
        [Required]
        [EmailAddress]
        public string mail { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public void OnPost()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=alumnos;Uid=root;password=19062002jm;";
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            //MySqlCommand cmd = new MySqlCommand();
            MySqlCommand cmd = new MySqlCommand(connectionString, conexion);
            cmd.Connection = conexion;
            cmd.CommandText = "select * from aplicantes where correo=@mail and contrasena=@password;";

            cmd.Parameters.AddWithValue("@mail", mail);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Prepare();

            aplicantes usr1 = new aplicantes();
            Listaaplicantes = new List<aplicantes>();
            check = int.Parse(cmd.ExecuteScalar().ToString());
            if (check > 0)
            {
                Response.Redirect("pag1");
            }
            //Borrar desde aqui
            using (var reader = cmd.ExecuteReader())
            {


                while (reader.Read())
                {
                    usr1 = new aplicantes();
                    usr1.Correo = reader["correo"].ToString();
                    usr1.Contrasena = reader["contrasena"].ToString();
                    Listaaplicantes.Add(usr1);
                }
            }
            //A aqui si funciona
            conexion.Dispose();

        }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Listaaplicantes = new List<aplicantes>();
        }
    }
}