using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Drawing.Layout;
using PdfSharp.Fonts;

namespace SISTEMA_PORTAFOLIO_LLICA_PEREZ
{
    public partial class Form2 : Form
    {
        private DataTable datosCursos;

        public Form2(DataTable datos)
        {
            InitializeComponent();
            datosCursos = datos;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string colTipo = "TIPO";
            string colDocente = "DOCENTE";
            string colCodigo = "CODIGO";

            if (datosCursos != null)
            {
                if (datosCursos.Columns.Contains(colTipo))
                    GenerarGraficoPieTipoCurso(colTipo);

                if (datosCursos.Columns.Contains(colDocente) && datosCursos.Columns.Contains(colCodigo))
                    GenerarGraficoCursosPorDocente(colDocente, colCodigo);

                if (datosCursos.Columns.Contains(colCodigo))
                    GenerarGraficoCursosPorCodigo(colCodigo);
            }
        }

        private void GenerarGraficoPieTipoCurso(string colTipo)
        {
            var tipos = datosCursos.AsEnumerable()
                .Where(row => !string.IsNullOrWhiteSpace(row[colTipo]?.ToString()))
                .GroupBy(row => row[colTipo].ToString().Trim().ToUpper())
                .Select(g => new { Tipo = g.Key, Cantidad = g.Count() })
                .ToList();

            chartcursos.Series.Clear();
            chartcursos.Titles.Clear();
            chartcursos.Titles.Add("Distribución de Cursos (Obligatorio vs Electivo)");

            Series serie = new Series("TipoCurso")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Black,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            foreach (var item in tipos)
            {
                int idx = serie.Points.AddXY(item.Tipo, item.Cantidad);
                var punto = serie.Points[idx];
                punto.Label = $"{item.Tipo}: {item.Cantidad} ({punto.YValues[0] * 100.0 / tipos.Sum(t => t.Cantidad):0.0}%)";
            }

            chartcursos.Series.Add(serie);
        }

        private void GenerarGraficoCursosPorDocente(string colDocente, string colCodigo)
        {
            var datos = datosCursos.AsEnumerable()
                .Where(row => !string.IsNullOrWhiteSpace(row[colDocente]?.ToString()))
                .GroupBy(row => row[colDocente].ToString().Trim())
                .Select(g => new { Docente = g.Key, Cursos = g.Select(r => r[colCodigo].ToString()).Distinct().Count() })
                .OrderByDescending(x => x.Cursos)
                .ToList();

            chartnrodecursosdocentes.Series.Clear();
            chartnrodecursosdocentes.Titles.Clear();
            chartnrodecursosdocentes.Titles.Add("Número de Cursos por Docente");

            // Configurar el área del gráfico
            chartnrodecursosdocentes.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 7, FontStyle.Regular);
            chartnrodecursosdocentes.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chartnrodecursosdocentes.ChartAreas[0].AxisX.Interval = 1;
            chartnrodecursosdocentes.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
            chartnrodecursosdocentes.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chartnrodecursosdocentes.ChartAreas[0].AxisX.ScaleView.Size = 10;
            // Ajustar márgenes para mejor espaciado
            chartnrodecursosdocentes.ChartAreas[0].InnerPlotPosition.Auto = false;
            chartnrodecursosdocentes.ChartAreas[0].InnerPlotPosition.X = 10;
            chartnrodecursosdocentes.ChartAreas[0].InnerPlotPosition.Y = 5;
            chartnrodecursosdocentes.ChartAreas[0].InnerPlotPosition.Width = 85;
            chartnrodecursosdocentes.ChartAreas[0].InnerPlotPosition.Height = 75;


            Series serie = new Series("CursosPorDocente")
            {
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Black,
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                ChartType = SeriesChartType.Column
            };

            chartnrodecursosdocentes.Series.Add(serie);

            foreach (var item in datos)
            {
                // Acortar nombres largos para mejor visualización
                string nombreDocente = item.Docente.Length > 20 ? 
                    item.Docente.Substring(0, 17) + "..." : item.Docente;
                serie.Points.AddXY(nombreDocente, item.Cursos);
            }
        }

        private void GenerarGraficoCursosPorCodigo(string colCodigo)
        {
            var datos = datosCursos.AsEnumerable()
                .Where(row => !string.IsNullOrWhiteSpace(row[colCodigo]?.ToString()))
                .GroupBy(row => row[colCodigo].ToString().Trim().Substring(0, 2).ToUpper())
                .Select(g => new { Codigo = g.Key, Cantidad = g.Count() })
                .ToList();

            charttiposdecursos.Series.Clear();
            charttiposdecursos.Titles.Clear();
            charttiposdecursos.Titles.Add("Distribución por Código (SI, EG, INE)");

            Series serie = new Series("Codigos")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Black,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            foreach (var item in datos)
            {
                int idx = serie.Points.AddXY(item.Codigo, item.Cantidad);
                var punto = serie.Points[idx];
                punto.Label = $"{item.Codigo}: {item.Cantidad} ({punto.YValues[0] * 100.0 / datos.Sum(t => t.Cantidad):0.0}%)";
            }

            charttiposdecursos.Series.Add(serie);
        }

        private void btnregresar_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1(datosCursos.Copy()); 
            frm1.Show();
            this.Close();
        }

        private void btnreporte_Click(object sender, EventArgs e)
        {
            try
            {
                ExportarGraficosAPdf();
                MessageBox.Show("Reporte PDF generado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportarGraficosAPdf()
        {
            GlobalFontSettings.UseWindowsFontsUnderWindows = true;
            // Crear documento PDF
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Reporte de Gráficos Estadísticos";
            document.Info.Author = "Sistema Portafolio";
            document.Info.Subject = "Análisis de Cursos";

            // Crear página
            PdfPage page = document.AddPage();
            page.Size = PdfSharp.PageSize.A4;
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Configurar fuentes usando fuentes básicas embebidas de PdfSharp
            XFont titleFont = new XFont("Arial", 16);
            XFont subtitleFont = new XFont("Arial", 12);
            XFont normalFont = new XFont("Arial", 10);

            // Título principal
             string titulo = "REPORTE DE GRÁFICOS ESTADÍSTICOS";
             XSize titleSize = gfx.MeasureString(titulo, titleFont);
             gfx.DrawString(titulo, titleFont, XBrushes.Black, 
                 new XRect(0, 30, page.Width.Point, titleSize.Height), XStringFormats.TopCenter);

            // Fecha
            string fecha = $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}";
            gfx.DrawString(fecha, normalFont, XBrushes.Black, 50, 70);

            double yPosition = 100;
            double chartWidth = 250;
            double chartHeight = 180;
            double margin = 20;

            // Exportar primer gráfico (Distribución de Cursos)
            if (chartcursos.Series.Count > 0)
            {
                gfx.DrawString("1. Distribución de Cursos (Obligatorio vs Electivo)", subtitleFont, XBrushes.Black, 50, yPosition);
                yPosition += 25;
                
                using (MemoryStream ms = new MemoryStream())
                {
                    chartcursos.SaveImage(ms, ChartImageFormat.Png);
                    ms.Position = 0;
                    XImage image = XImage.FromStream(ms);
                    gfx.DrawImage(image, 50, yPosition, chartWidth, chartHeight);
                }
                yPosition += chartHeight + margin;
            }

            // Exportar segundo gráfico (Distribución por Código)
            if (charttiposdecursos.Series.Count > 0)
            {
                gfx.DrawString("2. Distribución por Código (SI, EG, INE)", subtitleFont, XBrushes.Black, 320, 125);
                
                using (MemoryStream ms = new MemoryStream())
                {
                    charttiposdecursos.SaveImage(ms, ChartImageFormat.Png);
                    ms.Position = 0;
                    XImage image = XImage.FromStream(ms);
                    gfx.DrawImage(image, 320, 150, chartWidth, chartHeight);
                }
            }

            // Crear segunda página para el gráfico de barras
            if (chartnrodecursosdocentes.Series.Count > 0)
            {
                PdfPage page2 = document.AddPage();
                page2.Size = PdfSharp.PageSize.A4;
                XGraphics gfx2 = XGraphics.FromPdfPage(page2);

                gfx2.DrawString("3. Número de Cursos por Docente", subtitleFont, XBrushes.Black, 50, 50);
                
                // Guardar configuración actual del scroll
                bool scrollEnabled = chartnrodecursosdocentes.ChartAreas[0].AxisX.ScrollBar.Enabled;
                double scaleViewSize = chartnrodecursosdocentes.ChartAreas[0].AxisX.ScaleView.Size;
                
                try
                {
                    // Deshabilitar scroll y mostrar todos los datos para el PDF
                    chartnrodecursosdocentes.ChartAreas[0].AxisX.ScrollBar.Enabled = false;
                    chartnrodecursosdocentes.ChartAreas[0].AxisX.ScaleView.Size = double.NaN;
                    
                    using (MemoryStream ms = new MemoryStream())
                    {
                        chartnrodecursosdocentes.SaveImage(ms, ChartImageFormat.Png);
                        ms.Position = 0;
                        XImage image = XImage.FromStream(ms);
                        // Usar mayor tamaño para el gráfico de barras
                         gfx2.DrawImage(image, 50, 80, page2.Width.Point - 100, 400);
                    }
                }
                finally
                {
                    // Restaurar configuración original del scroll
                    chartnrodecursosdocentes.ChartAreas[0].AxisX.ScrollBar.Enabled = scrollEnabled;
                    chartnrodecursosdocentes.ChartAreas[0].AxisX.ScaleView.Size = scaleViewSize;
                }

                // Agregar nota al pie
                 gfx2.DrawString("Nota: Este gráfico muestra la distribución de cursos asignados por docente.", 
                     normalFont, XBrushes.Gray, 50, page2.Height.Point - 50);

                gfx2.Dispose();
            }

            // Agregar información adicional en la primera página
            gfx.DrawString("Resumen del Análisis:", subtitleFont, XBrushes.Black, 50, yPosition + 20);
            
            string resumen = "• Los gráficos muestran la distribución de cursos en el sistema.\n" +
                           "• Se analiza la proporción entre cursos obligatorios y electivos.\n" +
                           "• Se presenta la distribución por códigos de área.\n" +
                           "• Se muestra la carga de cursos por docente.";
            
            XTextFormatter tf = new XTextFormatter(gfx);
             XRect rect = new XRect(50, yPosition + 45, page.Width.Point - 100, 100);
             tf.DrawString(resumen, normalFont, XBrushes.Black, rect, XStringFormats.TopLeft);

            gfx.Dispose();

            // Guardar el PDF
            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                Title = "Guardar Reporte PDF",
                FileName = $"Reporte_Graficos_{DateTime.Now:yyyyMMdd_HHmm}.pdf"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                document.Save(saveDialog.FileName);
            }

            document.Close();
        }


    }
}
