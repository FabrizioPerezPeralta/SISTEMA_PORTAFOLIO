namespace SISTEMA_PORTAFOLIO_LLICA_PEREZ
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chartnrodecursosdocentes = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.charttiposdecursos = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartcursos = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnregresar = new System.Windows.Forms.Button();
            this.btnreporte = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartnrodecursosdocentes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.charttiposdecursos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartcursos)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(910, 370);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Nro de tipos de cursos";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Nro de cursos por docentes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(792, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Nro de curso segun su tipo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(487, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "GRAFICAS ESTADISTICAS";
            // 
            // chartnrodecursosdocentes
            // 
            chartArea4.Name = "ChartArea1";
            this.chartnrodecursosdocentes.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartnrodecursosdocentes.Legends.Add(legend4);
            this.chartnrodecursosdocentes.Location = new System.Drawing.Point(30, 74);
            this.chartnrodecursosdocentes.Name = "chartnrodecursosdocentes";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chartnrodecursosdocentes.Series.Add(series4);
            this.chartnrodecursosdocentes.Size = new System.Drawing.Size(730, 571);
            this.chartnrodecursosdocentes.TabIndex = 25;
            this.chartnrodecursosdocentes.Text = "chart1";
            // 
            // charttiposdecursos
            // 
            chartArea5.Name = "ChartArea1";
            this.charttiposdecursos.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.charttiposdecursos.Legends.Add(legend5);
            this.charttiposdecursos.Location = new System.Drawing.Point(782, 386);
            this.charttiposdecursos.Name = "charttiposdecursos";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.charttiposdecursos.Series.Add(series5);
            this.charttiposdecursos.Size = new System.Drawing.Size(456, 259);
            this.charttiposdecursos.TabIndex = 27;
            this.charttiposdecursos.Text = "chart3";
            // 
            // chartcursos
            // 
            chartArea6.Name = "ChartArea1";
            this.chartcursos.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.chartcursos.Legends.Add(legend6);
            this.chartcursos.Location = new System.Drawing.Point(782, 74);
            this.chartcursos.Name = "chartcursos";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            this.chartcursos.Series.Add(series6);
            this.chartcursos.Size = new System.Drawing.Size(456, 293);
            this.chartcursos.TabIndex = 30;
            this.chartcursos.Text = "chart6";
            // 
            // btnregresar
            // 
            this.btnregresar.Location = new System.Drawing.Point(21, 651);
            this.btnregresar.Name = "btnregresar";
            this.btnregresar.Size = new System.Drawing.Size(108, 37);
            this.btnregresar.TabIndex = 31;
            this.btnregresar.Text = "Regresar";
            this.btnregresar.UseVisualStyleBackColor = true;
            this.btnregresar.Click += new System.EventHandler(this.btnregresar_Click);
            // 
            // btnreporte
            // 
            this.btnreporte.Location = new System.Drawing.Point(1103, 651);
            this.btnreporte.Name = "btnreporte";
            this.btnreporte.Size = new System.Drawing.Size(108, 37);
            this.btnreporte.TabIndex = 32;
            this.btnreporte.Text = "Reporte";
            this.btnreporte.UseVisualStyleBackColor = true;
            this.btnreporte.Click += new System.EventHandler(this.btnreporte_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 700);
            this.Controls.Add(this.btnreporte);
            this.Controls.Add(this.btnregresar);
            this.Controls.Add(this.chartcursos);
            this.Controls.Add(this.charttiposdecursos);
            this.Controls.Add(this.chartnrodecursosdocentes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartnrodecursosdocentes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.charttiposdecursos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartcursos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartnrodecursosdocentes;
        private System.Windows.Forms.DataVisualization.Charting.Chart charttiposdecursos;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartcursos;
        private System.Windows.Forms.Button btnregresar;
        private System.Windows.Forms.Button btnreporte;
    }
}