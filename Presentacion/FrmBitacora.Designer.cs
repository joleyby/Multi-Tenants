namespace Presentacion;

partial class FrmBitacora
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private Panel pnlFiltros;
    private Label lblDesde;
    private DateTimePicker dtpDesde;
    private Label lblHasta;
    private DateTimePicker dtpHasta;
    private Label lblOperacion;
    private ComboBox cboOperacion;
    private Label lblProvincia;
    private ComboBox cboProvincia;
    private Button btnBuscar;
    private DataGridView dgvBitacora;
    private DataGridViewTextBoxColumn colFecha;
    private DataGridViewTextBoxColumn colUsuario;
    private DataGridViewTextBoxColumn colProvincia;
    private DataGridViewTextBoxColumn colOperacion;
    private DataGridViewTextBoxColumn colEntidad;
    private DataGridViewTextBoxColumn colDescripcion;
    private DataGridViewCheckBoxColumn colExito;
    private Panel pnlAcciones;
    private Button btnAnterior;
    private Button btnSiguiente;
    private Label lblResumen;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        pnlFiltros = new Panel();
        lblDesde = new Label();
        dtpDesde = new DateTimePicker();
        lblHasta = new Label();
        dtpHasta = new DateTimePicker();
        lblOperacion = new Label();
        cboOperacion = new ComboBox();
        lblProvincia = new Label();
        cboProvincia = new ComboBox();
        btnBuscar = new Button();
        dgvBitacora = new DataGridView();
        colFecha = new DataGridViewTextBoxColumn();
        colUsuario = new DataGridViewTextBoxColumn();
        colProvincia = new DataGridViewTextBoxColumn();
        colOperacion = new DataGridViewTextBoxColumn();
        colEntidad = new DataGridViewTextBoxColumn();
        colDescripcion = new DataGridViewTextBoxColumn();
        colExito = new DataGridViewCheckBoxColumn();
        pnlAcciones = new Panel();
        btnAnterior = new Button();
        btnSiguiente = new Button();
        lblResumen = new Label();
        pnlFiltros.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBitacora).BeginInit();
        pnlAcciones.SuspendLayout();
        SuspendLayout();
        // 
        // pnlFiltros
        // 
        pnlFiltros.Controls.Add(lblDesde);
        pnlFiltros.Controls.Add(dtpDesde);
        pnlFiltros.Controls.Add(lblHasta);
        pnlFiltros.Controls.Add(dtpHasta);
        pnlFiltros.Controls.Add(lblOperacion);
        pnlFiltros.Controls.Add(cboOperacion);
        pnlFiltros.Controls.Add(lblProvincia);
        pnlFiltros.Controls.Add(cboProvincia);
        pnlFiltros.Controls.Add(btnBuscar);
        pnlFiltros.Dock = DockStyle.Top;
        pnlFiltros.Location = new Point(0, 0);
        pnlFiltros.Name = "pnlFiltros";
        pnlFiltros.Padding = new Padding(10);
        pnlFiltros.Size = new Size(1100, 70);
        pnlFiltros.TabIndex = 0;
        // 
        // lblDesde
        // 
        lblDesde.AutoSize = true;
        lblDesde.Location = new Point(13, 13);
        lblDesde.Name = "lblDesde";
        lblDesde.Size = new Size(55, 20);
        lblDesde.TabIndex = 0;
        lblDesde.Text = "Desde:";
        // 
        // dtpDesde
        // 
        dtpDesde.Format = DateTimePickerFormat.Short;
        dtpDesde.Location = new Point(13, 36);
        dtpDesde.Name = "dtpDesde";
        dtpDesde.Size = new Size(130, 27);
        dtpDesde.TabIndex = 1;
        // 
        // lblHasta
        // 
        lblHasta.AutoSize = true;
        lblHasta.Location = new Point(160, 13);
        lblHasta.Name = "lblHasta";
        lblHasta.Size = new Size(52, 20);
        lblHasta.TabIndex = 2;
        lblHasta.Text = "Hasta:";
        // 
        // dtpHasta
        // 
        dtpHasta.Format = DateTimePickerFormat.Short;
        dtpHasta.Location = new Point(160, 36);
        dtpHasta.Name = "dtpHasta";
        dtpHasta.Size = new Size(130, 27);
        dtpHasta.TabIndex = 3;
        // 
        // lblOperacion
        // 
        lblOperacion.AutoSize = true;
        lblOperacion.Location = new Point(307, 13);
        lblOperacion.Name = "lblOperacion";
        lblOperacion.Size = new Size(81, 20);
        lblOperacion.TabIndex = 4;
        lblOperacion.Text = "Operación:";
        // 
        // cboOperacion
        // 
        cboOperacion.DropDownStyle = ComboBoxStyle.DropDownList;
        cboOperacion.Location = new Point(307, 36);
        cboOperacion.Name = "cboOperacion";
        cboOperacion.Size = new Size(180, 28);
        cboOperacion.TabIndex = 5;
        // 
        // lblProvincia
        // 
        lblProvincia.AutoSize = true;
        lblProvincia.Location = new Point(500, 13);
        lblProvincia.Name = "lblProvincia";
        lblProvincia.Size = new Size(70, 20);
        lblProvincia.TabIndex = 6;
        lblProvincia.Text = "Provincia:";
        // 
        // cboProvincia
        // 
        cboProvincia.DropDownStyle = ComboBoxStyle.DropDownList;
        cboProvincia.Location = new Point(500, 36);
        cboProvincia.Name = "cboProvincia";
        cboProvincia.Size = new Size(200, 28);
        cboProvincia.TabIndex = 7;
        // 
        // btnBuscar
        // 
        btnBuscar.Location = new Point(720, 34);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(90, 32);
        btnBuscar.TabIndex = 8;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;
        // 
        // dgvBitacora
        // 
        dgvBitacora.AllowUserToAddRows = false;
        dgvBitacora.AllowUserToDeleteRows = false;
        dgvBitacora.AutoGenerateColumns = false;
        dgvBitacora.BackgroundColor = SystemColors.Window;
        dgvBitacora.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvBitacora.Columns.AddRange(new DataGridViewColumn[] { colFecha, colUsuario, colProvincia, colOperacion, colEntidad, colDescripcion, colExito });
        dgvBitacora.Dock = DockStyle.Fill;
        dgvBitacora.Location = new Point(0, 70);
        dgvBitacora.MultiSelect = false;
        dgvBitacora.Name = "dgvBitacora";
        dgvBitacora.ReadOnly = true;
        dgvBitacora.RowHeadersVisible = false;
        dgvBitacora.RowHeadersWidth = 51;
        dgvBitacora.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBitacora.Size = new Size(1100, 390);
        dgvBitacora.TabIndex = 1;
        // 
        // colFecha
        // 
        colFecha.DataPropertyName = "FechaHora";
        colFecha.DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm:ss" };
        colFecha.HeaderText = "Fecha";
        colFecha.MinimumWidth = 6;
        colFecha.Name = "colFecha";
        colFecha.Width = 150;
        // 
        // colUsuario
        // 
        colUsuario.DataPropertyName = "NombreUsuario";
        colUsuario.HeaderText = "Usuario";
        colUsuario.MinimumWidth = 6;
        colUsuario.Name = "colUsuario";
        colUsuario.Width = 130;
        // 
        // colProvincia
        // 
        colProvincia.DataPropertyName = "NombreProvincia";
        colProvincia.HeaderText = "Provincia";
        colProvincia.MinimumWidth = 6;
        colProvincia.Name = "colProvincia";
        colProvincia.Width = 160;
        // 
        // colOperacion
        // 
        colOperacion.DataPropertyName = "Operacion";
        colOperacion.HeaderText = "Operación";
        colOperacion.MinimumWidth = 6;
        colOperacion.Name = "colOperacion";
        colOperacion.Width = 120;
        // 
        // colEntidad
        // 
        colEntidad.DataPropertyName = "Entidad";
        colEntidad.HeaderText = "Entidad";
        colEntidad.MinimumWidth = 6;
        colEntidad.Name = "colEntidad";
        colEntidad.Width = 130;
        // 
        // colDescripcion
        // 
        colDescripcion.DataPropertyName = "Descripcion";
        colDescripcion.HeaderText = "Descripción";
        colDescripcion.MinimumWidth = 6;
        colDescripcion.Name = "colDescripcion";
        colDescripcion.Width = 380;
        // 
        // colExito
        // 
        colExito.DataPropertyName = "Exito";
        colExito.HeaderText = "Éxito";
        colExito.MinimumWidth = 6;
        colExito.Name = "colExito";
        colExito.Width = 70;
        // 
        // pnlAcciones
        // 
        pnlAcciones.Controls.Add(btnAnterior);
        pnlAcciones.Controls.Add(btnSiguiente);
        pnlAcciones.Controls.Add(lblResumen);
        pnlAcciones.Dock = DockStyle.Bottom;
        pnlAcciones.Location = new Point(0, 460);
        pnlAcciones.Name = "pnlAcciones";
        pnlAcciones.Padding = new Padding(10);
        pnlAcciones.Size = new Size(1100, 58);
        pnlAcciones.TabIndex = 2;
        // 
        // btnAnterior
        // 
        btnAnterior.Enabled = false;
        btnAnterior.Location = new Point(760, 13);
        btnAnterior.Name = "btnAnterior";
        btnAnterior.Size = new Size(100, 32);
        btnAnterior.TabIndex = 0;
        btnAnterior.Text = "« Anterior";
        btnAnterior.UseVisualStyleBackColor = true;
        btnAnterior.Click += btnAnterior_Click;
        // 
        // btnSiguiente
        // 
        btnSiguiente.Enabled = false;
        btnSiguiente.Location = new Point(866, 13);
        btnSiguiente.Name = "btnSiguiente";
        btnSiguiente.Size = new Size(100, 32);
        btnSiguiente.TabIndex = 1;
        btnSiguiente.Text = "Siguiente »";
        btnSiguiente.UseVisualStyleBackColor = true;
        btnSiguiente.Click += btnSiguiente_Click;
        // 
        // lblResumen
        // 
        lblResumen.Location = new Point(13, 18);
        lblResumen.Name = "lblResumen";
        lblResumen.Size = new Size(500, 20);
        lblResumen.TabIndex = 2;
        // 
        // FrmBitacora
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 518);
        Controls.Add(dgvBitacora);
        Controls.Add(pnlAcciones);
        Controls.Add(pnlFiltros);
        MinimumSize = new Size(900, 450);
        Name = "FrmBitacora";
        Text = "Bitácora";
        Load += FrmBitacora_Load;
        pnlFiltros.ResumeLayout(false);
        pnlFiltros.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBitacora).EndInit();
        pnlAcciones.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion
}
