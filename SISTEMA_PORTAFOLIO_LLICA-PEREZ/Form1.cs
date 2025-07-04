using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.IO.Compression;

namespace SISTEMA_PORTAFOLIO_LLICA_PEREZ
{
    public partial class Form1 : Form
    {
        private string rutaExcel = string.Empty;
        private DataTable dtOriginal = null;  // Copia sin filtrar


        public Form1()
        {
            InitializeComponent();
            this.Shown += Form1_Shown;

        }

        public Form1(DataTable datos)
        {
            InitializeComponent();

            dtOriginal = datos.Copy(); // Guardamos una copia original sin filtro
            dgvVistaPrevia.DataSource = dtOriginal;
            dgvVistaPrevia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            ColorearFilasPorEstado(); // Aplicar colores
            this.Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ColorearFilasPorEstado();
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

            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Seleccione la carpeta donde se creará la carpeta Portafolio";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    string basePath = Path.Combine(selectedPath, "Portafolio");

                    if (Directory.Exists(basePath))
                    {
                        MessageBox.Show("Ya existe una carpeta 'Portafolio' en esa ruta.\nNo se generará una nueva para evitar duplicados.",
                                        "Generación detenida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    try
                    {
                        Directory.CreateDirectory(basePath);
                        GenerarPortafolioDesdeExcel(rutaExcel, basePath);
                        MessageBox.Show("Portafolio generado correctamente en:\n" + basePath, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al generar portafolio: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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

                dtOriginal = dataTable.Copy(); // ✅ GUARDAR COPIA SIN FILTRO
                dgvVistaPrevia.DataSource = dtOriginal;
                dgvVistaPrevia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            ColorearFilasPorEstado();
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

                    if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(asignatura) ||
                        string.IsNullOrEmpty(docente) || string.IsNullOrEmpty(seccion))
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
                string docenteNuevo = row[iDocente]?.ToString().Trim();
                string seccion = row[iSeccion]?.ToString().Trim();

                if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(asignatura) ||
                    string.IsNullOrEmpty(docenteNuevo) || string.IsNullOrEmpty(seccion))
                    continue;

                string nombreDocenteNuevo = LimpiarNombreCarpeta(docenteNuevo);
                string nombreCurso = LimpiarNombreCarpeta($"{codigo} {asignatura} {seccion}");

                string carpetaNuevoDocente = Path.Combine(basePath, nombreDocenteNuevo);
                string carpetaCursoNueva = Path.Combine(carpetaNuevoDocente, nombreCurso);

                Directory.CreateDirectory(carpetaNuevoDocente);

                // Buscar si el curso existe con otro docente
                var carpetasDocentes = Directory.GetDirectories(basePath);
                foreach (string carpetaDocente in carpetasDocentes)
                {
                    if (Path.GetFileName(carpetaDocente) != nombreDocenteNuevo)
                    {
                        string posibleCarpetaCurso = Path.Combine(carpetaDocente, nombreCurso);
                        if (Directory.Exists(posibleCarpetaCurso))
                        {
                            try
                            {
                                // Mover curso completo al nuevo docente
                                if (Directory.Exists(carpetaCursoNueva))
                                    Directory.Delete(carpetaCursoNueva, true);

                                Directory.Move(posibleCarpetaCurso, carpetaCursoNueva);

                                // Eliminar carpeta del docente anterior si queda vacía
                                if (!Directory.EnumerateFileSystemEntries(carpetaDocente).Any())
                                {
                                    Directory.Delete(carpetaDocente, true);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error al mover carpeta del curso:\n" + ex.Message,
                                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;
                        }
                    }
                }

                // Crear estructura si no existe
                if (!Directory.Exists(carpetaCursoNueva))
                {
                    GenerarCarpetasPorCurso(basePath, docenteNuevo, codigo, asignatura, seccion);
                }
            }
        }

        private void GenerarCarpetasPorCurso(string basePath, string docente, string codigo,
                                            string asignatura, string seccion)
        {
            string nombreDocente = LimpiarNombreCarpeta(docente);
            string nombreCurso = LimpiarNombreCarpeta($"{codigo} {asignatura} {seccion}");

            string docentePath = Path.Combine(basePath, nombreDocente);
            Directory.CreateDirectory(docentePath);

            // 1. CV
            string cvPath = Path.Combine(docentePath, "1.Curriculum_Vitae");
            if (!Directory.Exists(cvPath))
            {
                Directory.CreateDirectory(cvPath);
                File.WriteAllText(Path.Combine(cvPath, "curriculum_vitae_ICACIT.docx"), "");
                File.WriteAllText(Path.Combine(cvPath, "Nota_CV_Leer.txt"), "Este documento debe contener el CV del docente.");
            }

            // 2. Curso
            string cursoPath = Path.Combine(docentePath, nombreCurso);
            Directory.CreateDirectory(cursoPath);

            /* ---------- MODIFICACIÓN 1 ----------
             * Carpeta 2.Silabos_UPT_ICACIT:
             *  - Añadimos Silabo_ICACIT_Ejemplo.docx
             */
            string silabosPath = Path.Combine(cursoPath, "2.Silabos_UPT_ICACIT");                     // **NUEVO / MOD**
            Directory.CreateDirectory(silabosPath);                                                   // **NUEVO / MOD**
            File.WriteAllText(Path.Combine(silabosPath, "Silabo_Formato_ICACIT.docx"), "");           // **NUEVO / MOD**
            File.WriteAllText(Path.Combine(silabosPath, "Silabo_ICACIT_Ejemplo.docx"), "");           // **NUEVO / MOD**
            File.WriteAllText(Path.Combine(silabosPath, "Nota_Silabo_Leer.txt"),                      // **NUEVO / MOD**
                              "Aquí se encuentran los sílabos correspondientes.");                    // **NUEVO / MOD**

            /* ---------- MODIFICACIÓN 2 ----------
             * Carpeta 3.Prueba_de_Entrada:
             *  - El informe ahora es .xlsx
             */
            CrearCarpetaConNota(Path.Combine(cursoPath, "3.Prueba_de_Entrada"),                       // **NUEVO / MOD**
                                "1_Informe_Prueba_Entrada_2025-I.xlsx",                               // **NUEVO / MOD**
                                "Nota_InformePrueba_Entrada.txt",
                                "Informe de prueba de entrada del curso.");

            // 4. Portafolio por unidad
            string portafolioUnidadPath = Path.Combine(cursoPath, "4.Portafolio_por_Unidad");
            string[] unidades = { "I_Unidad", "II_Unidad", "III_Unidad" };

            foreach (string unidad in unidades)
            {
                string unidadPath = Path.Combine(portafolioUnidadPath, unidad);
                Directory.CreateDirectory(unidadPath);

                // 4.1 Formato_Notas_Asistencia
                string formatoNotasPath = Path.Combine(unidadPath, "Formato_Notas_Asistencia");
                Directory.CreateDirectory(formatoNotasPath);

                /* ---------- MODIFICACIÓN 3 ----------
                 *  - Nuevo documento 2_Portafolio_{U1/U2/U3}_2025-I.xlsx
                 *  - Nota_Portafolio_Leer.txt
                 */
                string sufijoUnidad = ObtenerPrefijoUnidad(unidad);                                  // **NUEVO / MOD**
                string nombrePortXls = $"2_Portafolio_{sufijoUnidad}_2025-I.xlsx";                     // **NUEVO / MOD**
                File.WriteAllText(Path.Combine(formatoNotasPath, nombrePortXls), "");                 // **NUEVO / MOD**
                File.WriteAllText(Path.Combine(formatoNotasPath, "Nota_Portafolio_Leer.txt"),         // **NUEVO / MOD**
                                  "Aquí irán los registros de notas y asistencia del portafolio.");   // **NUEVO / MOD**

                // 4.2 Recursos Docente
                string recursosDocentePath = Path.Combine(unidadPath, "Recursos_Docente");
                Directory.CreateDirectory(Path.Combine(recursosDocentePath, "1.Solucion_Examen"));
                Directory.CreateDirectory(Path.Combine(recursosDocentePath, "2.Diapositivas"));
                Directory.CreateDirectory(Path.Combine(recursosDocentePath, "3.Guias_Laboratorio"));
                Directory.CreateDirectory(Path.Combine(recursosDocentePath, "4.Otros_Recursos"));

                // 4.3 Recursos Estudiantes
                string recursosEstudiantesPath = Path.Combine(unidadPath, "Recursos_Estudiantes");
                Directory.CreateDirectory(Path.Combine(recursosEstudiantesPath, "1.Examenes"));
                Directory.CreateDirectory(Path.Combine(recursosEstudiantesPath, "2.Practicas_Calificadas"));
                Directory.CreateDirectory(Path.Combine(recursosEstudiantesPath, "3.Trabajos"));

                // 4.4 Proyecto Final
                string proyectoFinalPath = Path.Combine(recursosEstudiantesPath, "4.Proyecto_Final");
                Directory.CreateDirectory(proyectoFinalPath);
                Directory.CreateDirectory(Path.Combine(proyectoFinalPath, "Formatos_requeridos_por_proyecto"));
                File.WriteAllText(Path.Combine(proyectoFinalPath, "Forma_de_Archivar_Proyecto_Importante.png"), "");

                // 4.5 Resumen (Excel)
                string nombreArchivoCurso = GenerarNombreArchivoSeguro(nombreCurso);
                string resumenNombre = $"RESUMEN_{nombreArchivoCurso}.xlsx";
                string rutaResumen = Path.Combine(proyectoFinalPath, resumenNombre);

                if (rutaResumen.Length >= 250)
                {
                    string acronimoCorto = GenerarAcronimo(nombreCurso);
                    resumenNombre = $"RESUMEN_{acronimoCorto}.xlsx";
                    rutaResumen = Path.Combine(proyectoFinalPath, resumenNombre);
                }

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
                    MessageBox.Show("Error al guardar archivo Excel:\n" + ex.Message +
                                    "\nRuta: " + rutaResumen, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Directory.CreateDirectory(Path.Combine(recursosEstudiantesPath, "5.Otros"));

                File.WriteAllText(Path.Combine(unidadPath, "Nota_Unidad.txt"),
                                  $"Esta carpeta contiene los archivos de la {unidad.Replace("_", " ")}.");
            }

            // 5. Informe Final
            CrearCarpetaConNota(Path.Combine(cursoPath, "5.Informe_Final"),
                                "5_Informe_Final_2025-I.xlsx",
                                "Nota_InformeFinal.txt",
                                "Informe final del curso.");

            File.WriteAllText(Path.Combine(cursoPath, "Nota_Curso.txt"),
                              $"Portafolio para el curso {nombreCurso}");
        }

        /* ---------- MODIFICACIÓN 4 ----------
         *  Convierto I/II/III a U1/U2/U3 para nombrar los portafolios por unidad
         */
        private string ObtenerPrefijoUnidad(string unidad)
        {
            switch (unidad.Trim().ToUpperInvariant())
            {
                case "I_UNIDAD": return "U1";
                case "II_UNIDAD": return "U2";
                case "III_UNIDAD": return "U3";
                default: return "UX";
            }
        }                                                                                         // **NUEVO / MOD**

        private string GenerarAcronimo(string texto)
        {
            var palabras = texto.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string acronimo = string.Join("", palabras.Select(p => char.ToUpperInvariant(p[0])));

            if (string.IsNullOrWhiteSpace(acronimo) || acronimo.Length < 3)
                acronimo = "ACR_" + Math.Abs(texto.GetHashCode()).ToString();

            return acronimo.Length > 20 ? acronimo.Substring(0, 20) : acronimo;
        }

        private string GenerarNombreArchivoSeguro(string nombreCurso)
        {
            string limpio = LimpiarNombreArchivo(nombreCurso);
            return limpio.Length > 100 ? GenerarAcronimo(nombreCurso) : limpio;
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

        private void FiltrarGrilla()
        {
            if (dtOriginal == null)
                return;

            string texto = txtBuscar.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(texto))
            {
                dgvVistaPrevia.DataSource = dtOriginal;
                return;
            }

            string columnaFiltro = "";

            if (rbCodigo.Checked)
                columnaFiltro = "CODIGO";
            else if (rbAsignatura.Checked)
                columnaFiltro = "ASIGNATURA";
            else if (rbDocente.Checked)
                columnaFiltro = "DOCENTE";
            else if (rbCiclo.Checked)
                columnaFiltro = "CICLO";
            else
                return;

            if (!dtOriginal.Columns.Contains(columnaFiltro))
                return;

            DataTable dtFiltrado = dtOriginal.Clone();

            foreach (DataRow row in dtOriginal.Rows)
            {
                string valorCelda = row[columnaFiltro]?.ToString().Trim().ToLower();

                if (columnaFiltro == "CICLO")
                {
                    // Filtro exacto para ciclo
                    if (valorCelda == texto)
                        dtFiltrado.ImportRow(row);
                }
                else
                {
                    // Filtro parcial para los demás
                    if (valorCelda != null && valorCelda.Contains(texto))
                        dtFiltrado.ImportRow(row);
                }
            }

            dgvVistaPrevia.DataSource = dtFiltrado;
        }

        private void ColorearFilasPorEstado()
        {
            if (dgvVistaPrevia.DataSource == null)
                return;

            var tabla = (DataTable)dgvVistaPrevia.DataSource;
            int colDocente = tabla.Columns.Contains("DOCENTE") ? tabla.Columns["DOCENTE"].Ordinal : -1;
            int colSeccion = tabla.Columns.Contains("SECCION") ? tabla.Columns["SECCION"].Ordinal : -1;

            foreach (DataGridViewRow fila in dgvVistaPrevia.Rows)
            {
                if (fila.IsNewRow) continue;

                string docente = fila.Cells[colDocente]?.Value?.ToString().Trim();
                string seccion = fila.Cells[colSeccion]?.Value?.ToString().Trim();

                bool docenteVacio = string.IsNullOrWhiteSpace(docente);
                bool seccionVacia = string.IsNullOrWhiteSpace(seccion);

                if (docenteVacio || seccionVacia)
                {
                    // Rojo claro si falta alguno
                    fila.DefaultCellStyle.BackColor = Color.LightCoral;
                }
                else
                {
                    // Verde claro si ambos existen
                    fila.DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }
        }

        private void btnactualizar_Click(object sender, EventArgs e)
        {
            if (dgvVistaPrevia.DataSource == null)
            {
                MessageBox.Show("No hay datos cargados para actualizar.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Portafolio");
            Directory.CreateDirectory(basePath);

            try
            {
                DataTable dt = (DataTable)dgvVistaPrevia.DataSource;
                GenerarPortafolioDesdeExcel(dt, basePath);

                MessageBox.Show("Portafolio actualizado correctamente con los datos editados.",
                                "Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el portafolio: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ColorearFilasPorEstado();
        }

        private void btnestadistica_Click(object sender, EventArgs e)
        {
            if (dgvVistaPrevia.DataSource is DataTable dt && dt.Rows.Count > 0)
            {
                this.Hide();
                Form2 frm2 = new Form2(dt.Copy()); // 🔁 importante usar Copy()
                frm2.Show();
            }
            else
            {
                MessageBox.Show("No hay datos cargados para mostrar estadísticas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAnadirFormato_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Seleccionar archivo(s) para añadir";
                ofd.Multiselect = true;
                ofd.Filter = "Todos los archivos (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string[] archivosSeleccionados = ofd.FileNames;

                    using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                    {
                        fbd.Description = "Seleccionar carpeta donde se copiarán los archivos";

                        if (fbd.ShowDialog() == DialogResult.OK)
                        {
                            string carpetaDestino = fbd.SelectedPath;

                            try
                            {
                                foreach (string archivo in archivosSeleccionados)
                                {
                                    string nombreArchivo = Path.GetFileName(archivo);
                                    string destinoFinal = Path.Combine(carpetaDestino, nombreArchivo);

                                    // Evitar sobrescribir archivos con el mismo nombre
                                    if (File.Exists(destinoFinal))
                                    {
                                        DialogResult overwrite = MessageBox.Show(
                                            $"El archivo '{nombreArchivo}' ya existe en la carpeta destino.\n¿Desea reemplazarlo?",
                                            "Archivo duplicado",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question);

                                        if (overwrite == DialogResult.No)
                                            continue;
                                    }

                                    File.Copy(archivo, destinoFinal, true); // Copia con sobreescritura si se confirma
                                }

                                MessageBox.Show("Archivos añadidos correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error al copiar archivos:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            FiltrarGrilla();
            ColorearFilasPorEstado();
        }

        private void rbCodigo_CheckedChanged(object sender, EventArgs e)
        {
            FiltrarGrilla();
            ColorearFilasPorEstado();
        }

        private void rbAsignatura_CheckedChanged(object sender, EventArgs e)
        {
            FiltrarGrilla();
            ColorearFilasPorEstado();
        }

        private void rbDocente_CheckedChanged(object sender, EventArgs e)
        {
            FiltrarGrilla();
            ColorearFilasPorEstado();
        }

        private void rbCiclo_CheckedChanged(object sender, EventArgs e)
        {
            FiltrarGrilla();
            ColorearFilasPorEstado();
        }

        private void ComprimirPortafolio(string rutaCarpeta)
        {
            string rutaZip = rutaCarpeta.TrimEnd(Path.DirectorySeparatorChar) + ".zip";

            try
            {
                if (!Directory.Exists(rutaCarpeta))
                {
                    MessageBox.Show("La carpeta 'Portafolio' no existe en la ubicación seleccionada.",
                                    "No encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (File.Exists(rutaZip))
                {
                    DialogResult overwrite = MessageBox.Show("Ya existe un archivo ZIP con ese nombre.\n¿Deseas reemplazarlo?",
                                                              "ZIP existente", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (overwrite == DialogResult.No)
                        return;

                    File.Delete(rutaZip);
                }

                ZipFile.CreateFromDirectory(rutaCarpeta, rutaZip, CompressionLevel.Optimal, true);
                MessageBox.Show("Carpeta 'Portafolio' comprimida correctamente:\n" + rutaZip, "ZIP creado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Opcional: abrir ubicación
                // System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(rutaZip));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al comprimir portafolio:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnEliminarPortafolio_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Seleccione la ubicación donde está la carpeta Portafolio";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    string pathPortafolio = Path.Combine(selectedPath, "Portafolio");

                    if (Directory.Exists(pathPortafolio))
                    {
                        try
                        {
                            Directory.Delete(pathPortafolio, true); // true = borra recursivamente
                            MessageBox.Show("La carpeta 'Portafolio' ha sido eliminada correctamente.",
                                            "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al eliminar la carpeta: " + ex.Message,
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la carpeta 'Portafolio' en la ubicación seleccionada.",
                                        "No encontrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnComprimir_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Seleccione la ubicación que contiene la carpeta Portafolio";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string rutaPortafolio = Path.Combine(folderDialog.SelectedPath, "Portafolio");
                    ComprimirPortafolio(rutaPortafolio);
                }
            }
        }

        public void Salir()
        {
            DialogResult result = MessageBox.Show("¿Estás seguro de que deseas salir de la aplicación?",
                                      "Confirmar salida",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void btnsalir_Click(object sender, EventArgs e)
        {
            Salir();
        }
    }
}