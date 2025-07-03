using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace SISTEMA_PORTAFOLIO_LLICA_PEREZ
{
    public partial class Form1 : Form
    {
        private string rutaExcel = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos Excel (*.xlsx)|*.xlsx";
                ofd.Title = "Seleccionar archivo de carga";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    rutaExcel = ofd.FileName;
                    CargarExcelEnGrilla(rutaExcel);
                }
            }
        }

        private void dgvVistaPrevia_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rutaExcel) || !File.Exists(rutaExcel))
            {
                MessageBox.Show("Seleccione un archivo Excel válido antes de generar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Portafolio");
            Directory.CreateDirectory(basePath);

            try
            {
                GenerarPortafolioDesdeExcel(rutaExcel, basePath);
                MessageBox.Show("Portafolio generado correctamente en:\n" + basePath, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar portafolio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarExcelEnGrilla(string path)
        {
            using (var workbook = new XLWorkbook(path))
            {
                var worksheet = workbook.Worksheets.First();
                var dataTable = new DataTable();
                bool primeraFila = true;

                foreach (var row in worksheet.RowsUsed())
                {
                    if (primeraFila)
                    {
                        foreach (var cell in row.Cells())
                            dataTable.Columns.Add(cell.Value.ToString());
                        primeraFila = false;
                    }
                    else
                    {
                        dataTable.Rows.Add(row.Cells().Select(c => c.Value.ToString()).ToArray());
                    }
                }

                dgvVistaPrevia.DataSource = dataTable;
                dgvVistaPrevia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
        }

        private void GenerarPortafolioDesdeExcel(string excelPath, string basePath)
        {
            using (var workbook = new XLWorkbook(excelPath))
            {
                var worksheet = workbook.Worksheets.First();
                var rows = worksheet.RowsUsed().ToList();

                if (rows.Count < 2)
                    throw new Exception("El archivo Excel no tiene suficientes datos.");

                var headers = rows[0].Cells().Select(c => c.Value.ToString().Trim().ToUpperInvariant()).ToList();

                int iCodigo = headers.IndexOf("CODIGO");
                int iAsignatura = headers.IndexOf("ASIGNATURA");
                int iDocente = headers.IndexOf("DOCENTE");
                int iSeccion = headers.IndexOf("SECCION");

                if (iCodigo == -1 || iAsignatura == -1 || iDocente == -1 || iSeccion == -1)
                {
                    string columnas = string.Join(", ", headers);
                    throw new Exception("El Excel debe contener las columnas: Codigo, Asignatura, Docente y Seccion.\nColumnas detectadas: " + columnas);
                }

                foreach (var row in rows.Skip(1))
                {
                    var celdas = row.Cells().ToList();
                    if (celdas.Count <= Math.Max(iCodigo, Math.Max(iAsignatura, Math.Max(iDocente, iSeccion))))
                        continue;

                    string codigo = celdas[iCodigo].Value.ToString().Trim();
                    string asignatura = celdas[iAsignatura].Value.ToString().Trim();
                    string docente = celdas[iDocente].Value.ToString().Trim();
                    string seccion = celdas[iSeccion].Value.ToString().Trim();

                    if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(asignatura) || string.IsNullOrEmpty(docente) || string.IsNullOrEmpty(seccion))
                        continue;

                    GenerarCarpetasPorCurso(basePath, docente, codigo, asignatura, seccion);
                }
            }
        }

        private void GenerarPortafolioDesdeExcel(DataTable dt, string basePath)
        {
            var headers = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.Trim().ToUpperInvariant()).ToList();

            int iCodigo = headers.IndexOf("CODIGO");
            int iAsignatura = headers.IndexOf("ASIGNATURA");
            int iDocente = headers.IndexOf("DOCENTE");
            int iSeccion = headers.IndexOf("SECCION");

            if (iCodigo == -1 || iAsignatura == -1 || iDocente == -1 || iSeccion == -1)
                throw new Exception("El Excel cargado no tiene las columnas necesarias.");

            foreach (DataRow row in dt.Rows)
            {
                string codigo = row[iCodigo]?.ToString().Trim();
                string asignatura = row[iAsignatura]?.ToString().Trim();
                string docente = row[iDocente]?.ToString().Trim();
                string seccion = row[iSeccion]?.ToString().Trim();

                if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(asignatura) || string.IsNullOrEmpty(docente) || string.IsNullOrEmpty(seccion))
                    continue;

                GenerarCarpetasPorCurso(basePath, docente, codigo, asignatura, seccion);
            }
        }

        private void GenerarCarpetasPorCurso(string basePath, string docente, string codigo, string asignatura, string seccion)
        {
            string nombreDocente = LimpiarNombreCarpeta(docente);
            string nombreCurso = LimpiarNombreCarpeta($"{codigo} {asignatura} {seccion}");

            string docentePath = Path.Combine(basePath, nombreDocente);
            Directory.CreateDirectory(docentePath);

            string cvPath = Path.Combine(docentePath, "1.Curriculum_Vitae");
            if (!Directory.Exists(cvPath))
            {
                Directory.CreateDirectory(cvPath);
                File.WriteAllText(Path.Combine(cvPath, "curriculum_vitae_ICACIT.docx"), "");
                File.WriteAllText(Path.Combine(cvPath, "Nota_CV_Leer.txt"), "Este documento debe contener el CV del docente.");
            }

            string cursoPath = Path.Combine(docentePath, nombreCurso);
            Directory.CreateDirectory(cursoPath);

            CrearCarpetaConNota(Path.Combine(cursoPath, "2.Silabos_UPT_ICACIT"), "Silabo_Formato_ICACIT.docx", "Nota_Silabo_Leer.txt", "Aquí se encuentran los silabos correspondientes.");
            CrearCarpetaConNota(Path.Combine(cursoPath, "3.Prueba_de_Entrada"), "1_Informe_Prueba_Entrada_2025-I.docx", "Nota_InformePrueba_Entrada.txt", "Informe de prueba de entrada del curso.");

            string portafolioUnidadPath = Path.Combine(cursoPath, "4.Portafolio_por_Unidad");
            string[] unidades = { "I_Unidad", "II_Unidad", "III_Unidad" };

            foreach (string unidad in unidades)
            {
                string unidadPath = Path.Combine(portafolioUnidadPath, unidad);
                Directory.CreateDirectory(unidadPath);

                Directory.CreateDirectory(Path.Combine(unidadPath, "Formato_Notas_Asistencia"));

                string recursosDocentePath = Path.Combine(unidadPath, "Recursos_Docente");
                Directory.CreateDirectory(Path.Combine(recursosDocentePath, "1.Solucion_Examen"));
                Directory.CreateDirectory(Path.Combine(recursosDocentePath, "2.Diapositivas"));
                Directory.CreateDirectory(Path.Combine(recursosDocentePath, "3.Guias_Laboratorio"));
                Directory.CreateDirectory(Path.Combine(recursosDocentePath, "4.Otros_Recursos"));

                string recursosEstudiantesPath = Path.Combine(unidadPath, "Recursos_Estudiantes");
                Directory.CreateDirectory(Path.Combine(recursosEstudiantesPath, "1.Examenes"));
                Directory.CreateDirectory(Path.Combine(recursosEstudiantesPath, "2.Practicas_Calificadas"));
                Directory.CreateDirectory(Path.Combine(recursosEstudiantesPath, "3.Trabajos"));

                string proyectoFinalPath = Path.Combine(recursosEstudiantesPath, "4.Proyecto_Final");
                Directory.CreateDirectory(proyectoFinalPath);
                Directory.CreateDirectory(Path.Combine(proyectoFinalPath, "Formatos_requeridos_por_proyecto"));

                File.WriteAllText(Path.Combine(proyectoFinalPath, "Forma_de_Archivar_Proyecto_Importante.png"), "");

                string nombreArchivoCurso = GenerarNombreArchivoSeguro(nombreCurso);
                string resumenNombre = $"RESUMEN_{nombreArchivoCurso}.xlsx";
                string rutaResumen = Path.Combine(proyectoFinalPath, resumenNombre);

                // Validar que la ruta no sea demasiado larga
                if (rutaResumen.Length >= 250)
                {
                    // Generar acrónimo más corto si aún es muy largo
                    string acronimoCorto = GenerarAcronimo(nombreCurso);
                    resumenNombre = $"RESUMEN_{acronimoCorto}.xlsx";
                    rutaResumen = Path.Combine(proyectoFinalPath, resumenNombre);
                }

                // Intentar guardar el archivo Excel
                try
                {
                    using (var wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("Resumen");
                        ws.Cell("A1").Value = "Resumen de entregables para el curso.";
                        wb.SaveAs(rutaResumen);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar archivo Excel:\n" + ex.Message + "\nRuta: " + rutaResumen, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                Directory.CreateDirectory(Path.Combine(recursosEstudiantesPath, "5.Otros"));

                File.WriteAllText(Path.Combine(unidadPath, "Nota_Unidad.txt"), $"Esta carpeta contiene los archivos de la {unidad.Replace("_", " ")}.");
            }

            CrearCarpetaConNota(Path.Combine(cursoPath, "5.Informe_Final"), "5_Informe_Final_2025-I.xlsx", "Nota_InformeFinal.txt", "Informe final del curso.");
            File.WriteAllText(Path.Combine(cursoPath, "Nota_Curso.txt"), $"Portafolio para el curso {nombreCurso}");
        }

        private string GenerarAcronimo(string texto)
        {
            var palabras = texto.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string acronimo = string.Join("", palabras.Select(p => char.ToUpperInvariant(p[0])));

            if (string.IsNullOrWhiteSpace(acronimo) || acronimo.Length < 3)
                acronimo = "ACR_" + Math.Abs(texto.GetHashCode()).ToString();

            // Truncar si aún es largo
            return acronimo.Length > 20 ? acronimo.Substring(0, 20) : acronimo;
        }

        private string GenerarNombreArchivoSeguro(string nombreCurso)
        {
            string limpio = LimpiarNombreArchivo(nombreCurso);
            // Limitar el nombre limpio si sigue siendo largo
            if (limpio.Length > 100)
                return GenerarAcronimo(nombreCurso);
            return limpio;
        }


        private void CrearCarpetaConNota(string ruta, string archivo, string nota, string contenidoNota)
        {
            Directory.CreateDirectory(ruta);
            File.WriteAllText(Path.Combine(ruta, archivo), "");
            File.WriteAllText(Path.Combine(ruta, nota), contenidoNota);
        }

        private string LimpiarNombreCarpeta(string nombre)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                nombre = nombre.Replace(c, '_');
            return nombre.Trim();
        }

        private string LimpiarNombreArchivo(string nombre)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                nombre = nombre.Replace(c, '_');
            return nombre.Trim();
        }

        private void btnactualizar_Click(object sender, EventArgs e)
        {
            if (dgvVistaPrevia.DataSource == null)
            {
                MessageBox.Show("No hay datos cargados para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Portafolio");
            Directory.CreateDirectory(basePath);

            try
            {
                DataTable dt = (DataTable)dgvVistaPrevia.DataSource;
                GenerarPortafolioDesdeExcel(dt, basePath);

                MessageBox.Show("Portafolio actualizado correctamente con los datos editados.", "Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el portafolio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
