using System.ComponentModel.DataAnnotations;

namespace ProyectoEventos.Models
{
    public class Evento
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime FechaHora { get; set; }
        public string Ubicacion { get; set; }
        [Url]
        public string? LinkCompra { get; set; }
        public EstadoEvento Estado { get; set; } = EstadoEvento.Pendiente;
        public string? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public string? ImagenUrl { get; set; }
    }

    public class EventoViewModell
    {
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime FechaHora { get; set; }
        public string Ubicacion { get; set; }
        [Url]
        public string? LinkCompra { get; set; }

        public IFormFile? Imagen { get; set; }
        public string? ImagenUrl { get; set; }  
    }

    public enum EstadoEvento
    {
        Pendiente,
        Aprobado,
        Rechazado
    }
}
