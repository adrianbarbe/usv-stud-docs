using USVStudDocs.Models.Admin;

namespace USVStudDocs.BLL.Services.NavigationService;

public interface INavigationService
{
    List<Faculty> GetNavigation();
}