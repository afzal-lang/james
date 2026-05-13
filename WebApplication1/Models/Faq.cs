using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Faq
{
    public int Faqid { get; set; }

    public string? Question { get; set; }

    public string? Answer { get; set; }
}
