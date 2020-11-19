using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using sibintek.sibmobile.core;

namespace sibintek.http
{
    public interface IHttpContextResolver
    {
        Task<T> Resolve<T>(HttpContext context) where T : ICommand;
    }
}
