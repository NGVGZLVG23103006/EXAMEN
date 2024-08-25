using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ExamenProgra.Models;
using ExamenProgra.Data;

namespace ExamenProgra.Pages
{
    public class IndexModel : PageModel
    {
        private readonly EstudianteRepository _estudianteRepository;

        public List<Estudiante> Estudiantes { get; set; }
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, EstudianteRepository estudianteRepository)
        {
            _logger = logger;
            _estudianteRepository = estudianteRepository;
        }

        public void OnGet()
        {
            Estudiantes = _estudianteRepository.ObtenerEstudiantes();

        }
    }
}
