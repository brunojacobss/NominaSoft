﻿using System;
using System.Collections.Generic;
using System.Text;


namespace NominaSoft.Core.Entities
{
    public class Planilla
    {
        public ICollection<DatosPlanilla> DatosPlanillas { get; set; }

        public BoletaPago GenerarBoleta(PeriodoPago periodoPago, Contrato contrato, ConceptosDePago conceptosDePago)
        {
            BoletaPago boletaPago = new BoletaPago()
            {
                FechaPago = periodoPago.FechaFin,
                Contrato = contrato,
                IdPeriodoPago = periodoPago.IdPeriodoPago,
                PeriodoPago = periodoPago,
                //ConceptosDePago = conceptos
            };
            return boletaPago;
        }
    }
}
