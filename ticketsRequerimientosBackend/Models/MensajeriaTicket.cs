﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ticketsRequerimientosBackend.Models;

public partial class MensajeriaTicket
{
    public int? IdRequerimiento { get; set; }

    public DateTime? Fechaemit { get; set; }

    public string Mensaje { get; set; }

    public string Coduser { get; set; }

    public int Idmensaje { get; set; }
}