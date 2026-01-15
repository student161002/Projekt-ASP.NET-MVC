using System;
using System.Collections.Generic;

namespace Sklep_Internetowy.Models;

public partial class PozycjeZamowienium
{
    public int PozycjaId { get; set; }

    public int ZamowienieId { get; set; }

    public int ProduktId { get; set; }

    public int Ilosc { get; set; }

    public decimal CenaJednostkowa { get; set; }

    public virtual Produkty Produkt { get; set; } = null!;

    public virtual Zamowienium Zamowienie { get; set; } = null!;
}
