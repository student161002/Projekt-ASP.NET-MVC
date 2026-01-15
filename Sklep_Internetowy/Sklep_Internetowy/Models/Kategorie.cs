using System;
using System.Collections.Generic;

namespace Sklep_Internetowy.Models;

public partial class Kategorie
{
    public int KategoriaId { get; set; }

    public string Nazwa { get; set; } = null!;

    public string? Opis { get; set; }

    public virtual ICollection<Produkty> Produkties { get; set; } = new List<Produkty>();
}
