using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebPixUIAdmin.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Observe que o authenticationType deve corresponder àquele definido em CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Adicionar declarações de usuário personalizado aqui
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<WebPixUIAdmin.Models.PageViewModel> PageViewModels { get; set; }

        public System.Data.Entity.DbSet<WebPixUIAdmin.Models.UsuarioViewModel> UsuarioViewModels { get; set; }

        public System.Data.Entity.DbSet<WebPixUIAdmin.Models.PerfilViewModel> PerfilViewModels { get; set; }

        public System.Data.Entity.DbSet<WebPixUIAdmin.Models.MenuViewModel> MenuViewModels { get; set; }

        public System.Data.Entity.DbSet<WebPixUIAdmin.Models.Produto.ProdutoViewModel> ProdutoViewModels { get; set; }

        public System.Data.Entity.DbSet<WebPixUIAdmin.Models.Produto.ProdutoSkuViewModel> ProdutoSkuViewModels { get; set; }

        public System.Data.Entity.DbSet<WebPixUIAdmin.Models.Produto.PrecoViewModel> PrecoViewModels { get; set; }

        public System.Data.Entity.DbSet<WebPixUIAdmin.Models.Produto.PropiedadesViewModel> PropiedadesViewModels { get; set; }
    }
}