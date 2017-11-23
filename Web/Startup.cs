using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //Server = myServerAddress; Database = myDataBase; User Id = myUsername; Password = myPassword;
            services.AddDbContext<MvcMovieContext>(options => options.UseSqlServer("Server=52.44.60.121;Database=EstudoMvcNetCore;User Id=sa;Password=Desenv123!!;"));

            //Adicionar Implementação para Cache Distribuido. // Bertuzzi
            services.AddDistributedMemoryCache();

            //Bertuzzi
            services.AddSession(options =>
            {
                //Tempo Curto Apenas para Teste
                options.IdleTimeout = TimeSpan.FromSeconds(10); //Tenpo para liberar após abandonar a Sessao
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".EstudoMvcNetCore.Session";
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
