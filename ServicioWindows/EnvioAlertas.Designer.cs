﻿namespace ServicioWindows
{
    partial class EnvioAlertas
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.tmLapso = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.tmLapso)).BeginInit();
            // 
            // tmLapso
            // 
            this.tmLapso.Enabled = true;
            this.tmLapso.Interval = 86400000D;
            this.tmLapso.Elapsed += new System.Timers.ElapsedEventHandler(this.tmLapso_Elapsed);
            // 
            // EnvioAlertas
            // 
            this.ServiceName = "EnvioAlertas";
            ((System.ComponentModel.ISupportInitialize)(this.tmLapso)).EndInit();

        }

        #endregion

        private System.Timers.Timer tmLapso;
    }
}
