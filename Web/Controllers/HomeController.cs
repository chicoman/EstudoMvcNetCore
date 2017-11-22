using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Microsoft.AspNetCore.Http;
using Web.Helpers;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        //Variaveis para testar a Sessao // Bertuzzi
        const string SessionKeyNome = "_Nome";
        const string SessionKeyMebroDesde = "_MembroDesde";
        const string SessionKeyData = "_Data";


        public IActionResult Index()
        {
            //Bertuzzi
            // Requer Microsoft.AspNetCore.Http;
            HttpContext.Session.SetString(SessionKeyNome, "John no Arms");
            HttpContext.Session.SetInt32(SessionKeyMebroDesde, 3);

            /* Para trabalhar com Objetos Serializaveis é Necessario Criar um Extension da Sessão.
             * Criei na pasta Extensions.
             */

            //Com a Sessao serializavel podemos guardar objetos
            HttpContext.Session.Set<DateTime>(SessionKeyData, DateTime.Now);

            //Exemplo de Conexão
            //SqlData sql = new SqlData();
            //var resultado = sql.ExecutaSelect("Select * from Usuario");

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SessionNomeAnos()
        {
            var nome = HttpContext.Session.GetString(SessionKeyNome);
            var anos = HttpContext.Session.GetInt32(SessionKeyMebroDesde);
            var data = HttpContext.Session.Get<DateTime>(SessionKeyData);

            ViewData["Nome"] = nome;
            ViewData["Ano"] = anos;
            ViewData["Data"] = data.ToString("dd/MM/yyyy HH:mm");
            return View();
        }
    }
}
