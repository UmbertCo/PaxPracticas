using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsEnumerciones
/// </summary>
public class clsEnumeraciones
{
	public clsEnumeraciones()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public enum Servicios
    { 
        Timbrado = 1,
        Cancelacion = 2,
        RecepciónProveedores = 3,
        GeneracionTimbrado = 4,
        Validación = 5,
        Almacenamiento = 6,
        Recepción = 7,
        GeneraciónTimbradoEnvioEmail = 10,
        GeneracionTimbradoPDF = 11,
        CancelacionCD = 12,
        Nomina = 13
    }

    public enum TiposDocumentos
    {
	    Factura	= 1,
	    NotCrédito = 2,	
	    NotaCargo = 3,
	    ReciboArrendamiento= 4	,
	    CartaPorte = 5,
	    ReciboHonorarios = 6,
	    ComprobantePago	= 7,
	    ReciboDonativos = 8,
	    ReciboNomina = 10
    }

    public enum TiposDetalleNomina
    { 
        Percepcion = 1,
        Deduccion = 2
    }

    public enum TiposDeduccionesPercepciones
    { 
        SueldosSalariosRayasJornales = 1,
        SeguridadSocial = 2,
        ISR = 3,
        AportacionesRetiroCesantiaEdadAvanzadaVejez = 4,
        OtrosDeduccion = 5,
        AportacionesFondoVivienda = 6,
        DescuentoIncapacidad = 7,
        PensionAlimenticia = 8,
        Renta = 9,
        PrestamosProvenientesFondoNacionalViviendaTrabajadores = 10,
        PagoCreditoVivienda = 11,
        PagoAbonosINFONACOT = 12,
        AnticipoSalarios = 13,
        PagosHechosExcesoTrabajador = 14,
        Errores = 15,
        Pérdidas = 16,
        Averías = 17,
        AdquisicionArticulosProducidosEmpresaEstablecimiento = 18,
        CuotasConstitucionFomentoSociedadesCooperativasCajasAhorro = 19,
        CuotasSindicales = 20,
        Ausencia = 21,
        CuotasObreroPatronales = 22,
        GratificacionAnual = 23,
        ParticipacionTrabajadoresUtilidadesPTU = 24,
        ReembolsoGastosMedicosDentalesHospitalarios = 25,
        FondoAhorro = 26,
        Cajaahorro = 27,
        ContribucionesCargoTrabajadorPagadasPatron = 28,
        PremiosPuntualidad = 29,
        PrimaSeguroVida = 30,
        SeguroGastosMedicosMayores = 31,
        CuotasSindicalesPagadasPatron = 32,
        SubsidiosIncapacidad = 33,
        BecasTrabajadoresHijos = 34,
        OtrosPercepcion = 35,
        SubsidioEmpleo = 36,
        HorasExtra = 37,
        PrimaDominical = 38,
        PrimaVacacional = 39,
        PrimaAntiguedad = 40,
        PagosSeparación = 41,
        SeguroRetiro = 42,
        Indemnizaciones = 43,
        ReembolsoFuneral = 44,
        CuotasSeguridadSocialPagadasPatrón = 45,
        Comisiones = 46,
        ValesDespensa = 47,
        ValesRestaurante = 48,
        ValesGasolina = 49,
        ValesRopa = 50,
        AyudaRenta = 51,
        AyudaArticulosEscolares = 52,
        AyudaAnteojos = 53,
        AyudaTransporte = 54,
        AyudaGastosFuneral = 55,
        OtrosIngresosSalarios = 56,
        JubilacionesPensionesHaberesRetiro = 57, 
    }
}
