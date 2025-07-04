namespace SISTEMA_PORTAFOLIO_LLICA_PEREZ
{
    partial class Form1
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

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSeleccionar = new System.Windows.Forms.Button();
            this.dgvVistaPrevia = new System.Windows.Forms.DataGridView();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnactualizar = new System.Windows.Forms.Button();
            this.btnestadistica = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.rbCodigo = new System.Windows.Forms.RadioButton();
            this.rbAsignatura = new System.Windows.Forms.RadioButton();
            this.rbDocente = new System.Windows.Forms.RadioButton();
            this.rbCiclo = new System.Windows.Forms.RadioButton();
            this.btnAnadirFormato = new System.Windows.Forms.Button();
            this.btnEliminarPortafolio = new System.Windows.Forms.Button();
            this.btnComprimir = new System.Windows.Forms.Button();
            this.btnsalir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVistaPrevia)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSeleccionar
            // 
            this.btnSeleccionar.Location = new System.Drawing.Point(1224, 128);
            this.btnSeleccionar.Name = "btnSeleccionar";
            this.btnSeleccionar.Size = new System.Drawing.Size(108, 45);
            this.btnSeleccionar.TabIndex = 0;
            this.btnSeleccionar.Text = "Seleccionar archivo";
            this.btnSeleccionar.UseVisualStyleBackColor = true;
            this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
            // 
            // dgvVistaPrevia
            // 
            this.dgvVistaPrevia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVistaPrevia.Location = new System.Drawing.Point(155, 128);
            this.dgvVistaPrevia.Name = "dgvVistaPrevia";
            this.dgvVistaPrevia.Size = new System.Drawing.Size(1015, 454);
            this.dgvVistaPrevia.TabIndex = 1;
            this.dgvVistaPrevia.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVistaPrevia_CellContentClick);
            // 
            // btnGenerar
            // 
            this.btnGenerar.Location = new System.Drawing.Point(1224, 179);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(108, 45);
            this.btnGenerar.TabIndex = 2;
            this.btnGenerar.Text = "Generar portafolio";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(500, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(417, 39);
            this.label1.TabIndex = 3;
            this.label1.Text = "SISTEMA PORTAFOLIO";
            // 
            // btnactualizar
            // 
            this.btnactualizar.Location = new System.Drawing.Point(1224, 230);
            this.btnactualizar.Name = "btnactualizar";
            this.btnactualizar.Size = new System.Drawing.Size(108, 45);
            this.btnactualizar.TabIndex = 4;
            this.btnactualizar.Text = "Actualizar";
            this.btnactualizar.UseVisualStyleBackColor = true;
            this.btnactualizar.Click += new System.EventHandler(this.btnactualizar_Click);
            // 
            // btnestadistica
            // 
            this.btnestadistica.Location = new System.Drawing.Point(1224, 332);
            this.btnestadistica.Name = "btnestadistica";
            this.btnestadistica.Size = new System.Drawing.Size(108, 45);
            this.btnestadistica.TabIndex = 5;
            this.btnestadistica.Text = "Estadistica";
            this.btnestadistica.UseVisualStyleBackColor = true;
            this.btnestadistica.Click += new System.EventHandler(this.btnestadistica_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(432, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Buscar";
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(493, 67);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(424, 20);
            this.txtBuscar.TabIndex = 7;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            // 
            // rbCodigo
            // 
            this.rbCodigo.AutoSize = true;
            this.rbCodigo.Location = new System.Drawing.Point(541, 93);
            this.rbCodigo.Name = "rbCodigo";
            this.rbCodigo.Size = new System.Drawing.Size(58, 17);
            this.rbCodigo.TabIndex = 8;
            this.rbCodigo.TabStop = true;
            this.rbCodigo.Text = "Codigo";
            this.rbCodigo.UseVisualStyleBackColor = true;
            this.rbCodigo.CheckedChanged += new System.EventHandler(this.rbCodigo_CheckedChanged);
            // 
            // rbAsignatura
            // 
            this.rbAsignatura.AutoSize = true;
            this.rbAsignatura.Location = new System.Drawing.Point(620, 93);
            this.rbAsignatura.Name = "rbAsignatura";
            this.rbAsignatura.Size = new System.Drawing.Size(75, 17);
            this.rbAsignatura.TabIndex = 9;
            this.rbAsignatura.TabStop = true;
            this.rbAsignatura.Text = "Asignatura";
            this.rbAsignatura.UseVisualStyleBackColor = true;
            this.rbAsignatura.CheckedChanged += new System.EventHandler(this.rbAsignatura_CheckedChanged);
            // 
            // rbDocente
            // 
            this.rbDocente.AutoSize = true;
            this.rbDocente.Location = new System.Drawing.Point(717, 93);
            this.rbDocente.Name = "rbDocente";
            this.rbDocente.Size = new System.Drawing.Size(66, 17);
            this.rbDocente.TabIndex = 10;
            this.rbDocente.TabStop = true;
            this.rbDocente.Text = "Docente";
            this.rbDocente.UseVisualStyleBackColor = true;
            this.rbDocente.CheckedChanged += new System.EventHandler(this.rbDocente_CheckedChanged);
            // 
            // rbCiclo
            // 
            this.rbCiclo.AutoSize = true;
            this.rbCiclo.Location = new System.Drawing.Point(802, 93);
            this.rbCiclo.Name = "rbCiclo";
            this.rbCiclo.Size = new System.Drawing.Size(48, 17);
            this.rbCiclo.TabIndex = 11;
            this.rbCiclo.TabStop = true;
            this.rbCiclo.Text = "Ciclo";
            this.rbCiclo.UseVisualStyleBackColor = true;
            this.rbCiclo.CheckedChanged += new System.EventHandler(this.rbCiclo_CheckedChanged);
            // 
            // btnAnadirFormato
            // 
            this.btnAnadirFormato.Location = new System.Drawing.Point(1224, 281);
            this.btnAnadirFormato.Name = "btnAnadirFormato";
            this.btnAnadirFormato.Size = new System.Drawing.Size(108, 45);
            this.btnAnadirFormato.TabIndex = 12;
            this.btnAnadirFormato.Text = "Añadir Formato";
            this.btnAnadirFormato.UseVisualStyleBackColor = true;
            this.btnAnadirFormato.Click += new System.EventHandler(this.btnAnadirFormato_Click);
            // 
            // btnEliminarPortafolio
            // 
            this.btnEliminarPortafolio.Location = new System.Drawing.Point(1224, 383);
            this.btnEliminarPortafolio.Name = "btnEliminarPortafolio";
            this.btnEliminarPortafolio.Size = new System.Drawing.Size(108, 45);
            this.btnEliminarPortafolio.TabIndex = 13;
            this.btnEliminarPortafolio.Text = "Eliminar Portafolio";
            this.btnEliminarPortafolio.UseVisualStyleBackColor = true;
            this.btnEliminarPortafolio.Click += new System.EventHandler(this.btnEliminarPortafolio_Click);
            // 
            // btnComprimir
            // 
            this.btnComprimir.Location = new System.Drawing.Point(1224, 434);
            this.btnComprimir.Name = "btnComprimir";
            this.btnComprimir.Size = new System.Drawing.Size(108, 45);
            this.btnComprimir.TabIndex = 14;
            this.btnComprimir.Text = "Comprimir Portafolio";
            this.btnComprimir.UseVisualStyleBackColor = true;
            this.btnComprimir.Click += new System.EventHandler(this.btnComprimir_Click);
            // 
            // btnsalir
            // 
            this.btnsalir.Location = new System.Drawing.Point(1224, 498);
            this.btnsalir.Name = "btnsalir";
            this.btnsalir.Size = new System.Drawing.Size(108, 45);
            this.btnsalir.TabIndex = 15;
            this.btnsalir.Text = "Salir";
            this.btnsalir.UseVisualStyleBackColor = true;
            this.btnsalir.Click += new System.EventHandler(this.btnsalir_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 667);
            this.Controls.Add(this.btnsalir);
            this.Controls.Add(this.btnComprimir);
            this.Controls.Add(this.btnEliminarPortafolio);
            this.Controls.Add(this.btnAnadirFormato);
            this.Controls.Add(this.rbCiclo);
            this.Controls.Add(this.rbDocente);
            this.Controls.Add(this.rbAsignatura);
            this.Controls.Add(this.rbCodigo);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnestadistica);
            this.Controls.Add(this.btnactualizar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.dgvVistaPrevia);
            this.Controls.Add(this.btnSeleccionar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVistaPrevia)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSeleccionar;
        private System.Windows.Forms.DataGridView dgvVistaPrevia;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnactualizar;
        private System.Windows.Forms.Button btnestadistica;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.RadioButton rbCodigo;
        private System.Windows.Forms.RadioButton rbAsignatura;
        private System.Windows.Forms.RadioButton rbDocente;
        private System.Windows.Forms.RadioButton rbCiclo;
        private System.Windows.Forms.Button btnAnadirFormato;
        private System.Windows.Forms.Button btnEliminarPortafolio;
        private System.Windows.Forms.Button btnComprimir;
        private System.Windows.Forms.Button btnsalir;
    }
}

