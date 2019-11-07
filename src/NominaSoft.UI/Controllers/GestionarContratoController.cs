﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NominaSoft.Core.Entities;
using NominaSoft.Core.Interfaces;
using NominaSoft.Core.Specifications;
using NominaSoft.UI.ViewModels;

namespace NominaSoft.UI.Controllers
{
    public class GestionarContratoController : Controller
    {
        private readonly IRepository<Empleado> _repositoryEmpleado;
        private readonly IRepository<AFP> _repositoryAFP;
        private readonly IRepository<Contrato> _repositoryContrato;

        public GestionarContratoController(IRepository<Empleado> repositoryEmpleado,
                                            IRepository<AFP> repositoryAFP,
                                            IRepository<Contrato> repositoryContrato)
        {
            _repositoryEmpleado = repositoryEmpleado;
            _repositoryAFP = repositoryAFP;
            _repositoryContrato = repositoryContrato;
        }

        //public String Index()
        //{
            //return _repositoryEmpleado.GetById(1).NombreEmpleado;
        //}

        [HttpGet]
        public IActionResult GestionarContrato()
        {
            ViewModelGestionarContrato viewModelGestionarContrato = new ViewModelGestionarContrato();

            return View(viewModelGestionarContrato);
        }

        [HttpGet]
        public IActionResult BuscarEmpleado(String dni)
        {
            ViewModelGestionarContrato viewModelGestionarContrato = new ViewModelGestionarContrato
            {
                Empleado = _repositoryEmpleado.Get(new BusquedaPorDniSpecification(dni)),
                AFPs = _repositoryAFP.List()
            };
            
            if (viewModelGestionarContrato.Empleado != null)
            {
                foreach (Contrato contrato in viewModelGestionarContrato.Empleado.Contratos.ToList())
                {
                    if (!contrato.VerificarVigencia())
                        viewModelGestionarContrato.Empleado.Contratos.Remove(contrato);
                    else
                        viewModelGestionarContrato.Contrato = contrato;
                } 
            }
            else
            {
                viewModelGestionarContrato.EmpleadoNoEncontrado = 1;
            }

            return View("~/Views/GestionarContrato/GestionarContrato.cshtml", viewModelGestionarContrato);
        }

        [HttpPost]
        public IActionResult CrearContrato(ViewModelGestionarContrato viewModelGestionarContrato, int empleadoId)
        {
            Boolean afpInvalida = false;

            Contrato contrato = new Contrato()
            {
                Empleado = _repositoryEmpleado.GetById(empleadoId),
                FechaInicio = viewModelGestionarContrato.Contrato.FechaInicio,
                FechaFin = viewModelGestionarContrato.Contrato.FechaFin,
                Cargo = viewModelGestionarContrato.Contrato.Cargo,
                EsAsignacionFamiliar = viewModelGestionarContrato.Contrato.EsAsignacionFamiliar,
                ValorHora = viewModelGestionarContrato.Contrato.ValorHora,
                TotalHorasSemanales = viewModelGestionarContrato.Contrato.TotalHorasSemanales,
                EsAnulado = false
            };

            if (viewModelGestionarContrato.Contrato.IdAFP == 0)
                afpInvalida = true;
            else
                contrato.AFP = _repositoryAFP.GetById(viewModelGestionarContrato.Contrato.IdAFP);

            viewModelGestionarContrato = new ViewModelGestionarContrato();

            // AFP
            if (afpInvalida)
                viewModelGestionarContrato.MensajeError += "AFP no seleccionada.";

            // R01
            if (!contrato.VerificarVigencia())
                viewModelGestionarContrato.MensajeError = "El contrato no es vigente.";

            // RO2
            if (!contrato.VerificarFechaInicio(_repositoryContrato.LastList(new BusquedaContratoUltimoCreadoSpecification()).FirstOrDefault()))
                viewModelGestionarContrato.MensajeError += "La fecha inicio no es superior a la fecha fin del último contrato.";

            // R03
            if (!contrato.VerificarFechaFin())
                viewModelGestionarContrato.MensajeError += "La fecha fin es incorrecta.";

            // R04
            if(!contrato.VerificarTotalHorasSemanales())
                viewModelGestionarContrato.MensajeError += "El total de horas semanales es incorrecto.";

            // R05
            if (!contrato.VerificarValorHora())
                viewModelGestionarContrato.MensajeError += "El valor por hora es incorrecto.";

            if (!String.IsNullOrEmpty(viewModelGestionarContrato.MensajeError))
            {
                viewModelGestionarContrato.ErrorDatosContrato = 1;
                return View("~/Views/GestionarContrato/GestionarContrato.cshtml", viewModelGestionarContrato);
            }

            _repositoryContrato.Add(contrato);

            viewModelGestionarContrato.ContratoCreado = 1;

            return View("~/Views/GestionarContrato/GestionarContrato.cshtml", viewModelGestionarContrato);
        }

        [HttpPost]
        public IActionResult EditarContrato(ViewModelGestionarContrato viewModelGestionarContrato, int contratoId, int empleadoId)
        {

            viewModelGestionarContrato.Contrato.Empleado = _repositoryEmpleado.GetById(empleadoId);

            // AFP
            if (viewModelGestionarContrato.Contrato.IdAFP == 0)
                viewModelGestionarContrato.MensajeError = "AFP no seleccionada.";

            // R01
            if (!viewModelGestionarContrato.Contrato.VerificarVigencia())
                viewModelGestionarContrato.MensajeError += "El contrato no es vigente.";

            // R02
            if (!viewModelGestionarContrato.Contrato.VerificarFechaInicio(_repositoryContrato.LastList(new BusquedaContratoUltimoCreadoSpecification()).FirstOrDefault()))
                viewModelGestionarContrato.MensajeError += "La fecha inicio no es superior a la fecha fin del último contrato.";

            // R03
            if (!viewModelGestionarContrato.Contrato.VerificarFechaFin())
                viewModelGestionarContrato.MensajeError += "La fecha fin es incorrecta.";

            // R04
            if (!viewModelGestionarContrato.Contrato.VerificarTotalHorasSemanales())
                viewModelGestionarContrato.MensajeError += "El total de horas semanales es incorrecto.";

            // R05
            if (!viewModelGestionarContrato.Contrato.VerificarValorHora())
                viewModelGestionarContrato.MensajeError += "El valor por hora es incorrecto.";

            if (!String.IsNullOrEmpty(viewModelGestionarContrato.MensajeError))
            {
                viewModelGestionarContrato.ErrorDatosContrato = 1;
                viewModelGestionarContrato.Contrato = null;
                return View("~/Views/GestionarContrato/GestionarContrato.cshtml", viewModelGestionarContrato);
            }

            Contrato contrato = _repositoryContrato.GetById(contratoId);

            contrato.FechaInicio = viewModelGestionarContrato.Contrato.FechaInicio;
            contrato.FechaFin = viewModelGestionarContrato.Contrato.FechaFin;
            contrato.Cargo = viewModelGestionarContrato.Contrato.Cargo;
            contrato.AFP = _repositoryAFP.GetById(viewModelGestionarContrato.Contrato.IdAFP);
            contrato.EsAsignacionFamiliar = viewModelGestionarContrato.Contrato.EsAsignacionFamiliar;
            contrato.ValorHora = viewModelGestionarContrato.Contrato.ValorHora;
            contrato.TotalHorasSemanales = viewModelGestionarContrato.Contrato.TotalHorasSemanales;

            viewModelGestionarContrato = new ViewModelGestionarContrato();

            _repositoryContrato.Edit(contrato);
            viewModelGestionarContrato.ModificacionesContrato = 1;

            return View("~/Views/GestionarContrato/GestionarContrato.cshtml", viewModelGestionarContrato);
        }

        [HttpPost]
        public IActionResult AnularContrato(int contratoId)
        {
            Contrato contrato = _repositoryContrato.GetById(contratoId);
            contrato.EsAnulado = true;

            _repositoryContrato.Edit(contrato);

            ViewModelGestionarContrato viewModelGestionarContrato = new ViewModelGestionarContrato
            {
                ContratoAnulado = 1
            };

            return View("~/Views/GestionarContrato/GestionarContrato.cshtml", viewModelGestionarContrato);
        }
        
    }
}
