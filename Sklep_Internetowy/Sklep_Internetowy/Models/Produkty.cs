using System;
using System.Collections.Generic;

namespace Sklep_Internetowy.Models;

public partial class Produkty
{
    public int ProduktId { get; set; }

    public string Nazwa { get; set; } = null!;

    public decimal Cena { get; set; }

    public string? Opis { get; set; }

    public int KategoriaId { get; set; }

    public virtual Kategorie Kategoria { get; set; } = null!;

    public virtual ICollection<PozycjeZamowienium> PozycjeZamowienia { get; set; } = new List<PozycjeZamowienium>();
}
