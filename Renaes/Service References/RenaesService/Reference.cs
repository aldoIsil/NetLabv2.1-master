﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Renaes.Service_References.RenaesService {
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RenaesService.ServicioEstablecimientoSoap")]
    public interface ServicioEstablecimientoSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetEstablecimiento", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetEstablecimiento(string strCodigo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/listarModelesxRuc", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string listarModelesxRuc(string pruc, string pcod_sunasa);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServicioEstablecimientoSoapChannel : ServicioEstablecimientoSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioEstablecimientoSoapClient : System.ServiceModel.ClientBase<ServicioEstablecimientoSoap>, ServicioEstablecimientoSoap {
        
        public ServicioEstablecimientoSoapClient() {
        }
        
        public ServicioEstablecimientoSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicioEstablecimientoSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioEstablecimientoSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioEstablecimientoSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Data.DataSet GetEstablecimiento(string strCodigo) {
            return base.Channel.GetEstablecimiento(strCodigo);
        }
        
        public string listarModelesxRuc(string pruc, string pcod_sunasa) {
            return base.Channel.listarModelesxRuc(pruc, pcod_sunasa);
        }
    }
}
