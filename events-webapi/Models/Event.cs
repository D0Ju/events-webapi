using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace events_webapi.Models;

public class Event
{
    public int Id { get; set; }
    public string Naziv { get; set; } = string.Empty;
    public string Lokacija { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime? DatumPocetka { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? DatumZavrsetka { get; set; }

    public int BrojPolaznika { get; set; }
    public decimal Cijena { get; set; }
    public string Opis { get; set; } = string.Empty;
    public bool Aktivan { get; set; }
    public int VrstaId { get; set; }

    // Navigation property
    [ForeignKey("VrstaId")]
    [BindNever]
    public EventType? EventType { get; set; }
}
