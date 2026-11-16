namespace Presentacion;

partial class FrmEstablecimientos
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private Panel pnlFiltros;
    private Label lblTexto;
    private TextBox txtTexto;
    private Label lblTipo;
    private ComboBox cboTipo;
    private Label lblProvincia;
    private ComboBox cboProvincia;
    private CheckBox chkIncluirInactivos;
    private Button btnBuscar;
    private DataGridView dgvEstablecimientos;
    private DataGridViewTextBoxColumn colCodigo;
    private DataGridViewTextBoxColumn colNombre;
    private DataGridViewTextBoxColumn colTipo;
    private DataGridViewTextBoxColumn colProvincia;
    private DataGridViewTextBoxColumn colDireccion;
    private DataGridViewTextBoxColumn colTelefono;
    private DataGridViewTextBoxColumn colEstado;
    private Panel pnlAcciones;
    private Button btnNuevo;
    private Button btnEditar;
    private Button btnDesactivar;
    private Button btnAnterior;
    private Button btnSiguiente;
    private Label lblResumen;
    private Label lblModo;

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
        lblTexto = new Label();
        txtTexto = new TextBox();
        lblTipo = new Label();
        cboTipo = new ComboBox();
        lblProvincia = new Label();
        cboProvincia = new ComboBox();
        chkIncluirInactivos = new CheckBox();
        btnBuscar = new Button();
        dgvEstablecimientos = new DataGridView();
        colCodigo = new DataGridViewTextBoxColumn();
        colNombre = new DataGridViewTextBoxColumn();
        colTipo = new DataGridViewTextBoxColumn();
        colProvincia = new DataGridViewTextBoxColumn();
        colDireccion = new DataGridViewTextBoxColumn();
        colTelefono = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        pnlAcciones = new Panel();
        btnNuevo = new Button();
        btnEditar = new Button();
        btnDesactivar = new Button();
        btnAnterior = new Button();
        btnSiguiente = new Button();
        lblResumen = new Label();
        lblModo = new Label();
        pnlFiltros.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvEstablecimientos).BeginInit();
        pnlAcciones.SuspendLayout();
        SuspendLayout();
        // 
        // pnlFiltros
        // 
        pnlFiltros.Controls.Add(lblTexto);
        pnlFiltros.Controls.Add(txtTexto);
        pnlFiltros.Controls.Add(lblTipo);
        pnlFiltros.Controls.Add(cboTipo);
        pnlFiltros.Controls.Add(lblProvincia);
        pnlFiltros.Controls.Add(cboProvincia);
        pnlFiltros.Controls.Add(chkIncluirInactivos);
        pnlFiltros.Controls.Add(btnBuscar);
        pnlFiltros.Dock = DockStyle.Top;
        pnlFiltros.Location = new Point(0, 0);
        pnlFiltros.Name = "pnlFiltros";
        pnlFiltros.Padding = new Padding(10);
        pnlFiltros.Size = new Size(1000, 92);
        pnlFiltros.TabIndex = 0;
        // 
        // lblTexto
        // 
        lblTexto.AutoSize = true;
        lblTexto.Location = new Point(13, 16);
        lblTexto.Name = "lblTexto";
        lblTexto.Size = new Size(55, 20);
        lblTexto.TabIndex = 0;
        lblTexto.Text = "Buscar:";
        // 
        // txtTexto
        // 
        txtTexto.Location = new Point(13, 39);
        txtTexto.Name = "txtTexto";
        txtTexto.PlaceholderText = "Nombre o código";
        txtTexto.Size = new Size(280, 27);
        txtTexto.TabIndex = 1;
        // 
        // lblTipo
        // 
        lblTipo.AutoSize = true;
        lblTipo.Location = new Point(310, 16);
        lblTipo.Name = "lblTipo";
        lblTipo.Size = new Size(37, 20);
        lblTipo.TabIndex = 2;
        lblTipo.Text = "Tipo:";
        // 
        // cboTipo
        // 
        cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTipo.Location = new Point(310, 39);
        cboTipo.Name = "cboTipo";
        cboTipo.Size = new Size(200, 28);
        cboTipo.TabIndex = 3;
        // 
        // lblProvincia
        // 
        lblProvincia.AutoSize = true;
        lblProvincia.Location = new Point(525, 16);
        lblProvincia.Name = "lblProvincia";
        lblProvincia.Size = new Size(70, 20);
        lblProvincia.TabIndex = 4;
        lblProvincia.Text = "Provincia:";
        // 
        // cboProvincia
        // 
        cboProvincia.DropDownStyle = ComboBoxStyle.DropDownList;
        cboProvincia.Location = new Point(525, 39);
        cboProvincia.Name = "cboProvincia";
        cboProvincia.Size = new Size(200, 28);
        cboProvincia.TabIndex = 5;
        // 
        // chkIncluirInactivos
        // 
        chkIncluirInactivos.AutoSize = true;
        chkIncluirInactivos.Location = new Point(745, 41);
        chkIncluirInactivos.Name = "chkIncluirInactivos";
        chkIncluirInactivos.Size = new Size(140, 24);
        chkIncluirInactivos.TabIndex = 6;
        chkIncluirInactivos.Text = "Incluir inactivos";
        chkIncluirInactivos.UseVisualStyleBackColor = true;
        // 
        // btnBuscar
        // 
        btnBuscar.Location = new Point(900, 37);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(90, 32);
        btnBuscar.TabIndex = 7;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;
        // 
        // dgvEstablecimientos
        // 
        dgvEstablecimientos.AllowUserToAddRows = false;
        dgvEstablecimientos.AllowUserToDeleteRows = false;
        dgvEstablecimientos.AutoGenerateColumns = false;
        dgvEstablecimientos.BackgroundColor = SystemColors.Window;
        dgvEstablecimientos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvEstablecimientos.Columns.AddRange(new DataGridViewColumn[] { colCodigo, colNombre, colTipo, colProvincia, colDireccion, colTelefono, colEstado });
        dgvEstablecimientos.Dock = DockStyle.Fill;
        dgvEstablecimientos.Location = new Point(0, 92);
        dgvEstablecimientos.MultiSelect = false;
        dgvEstablecimientos.Name = "dgvEstablecimientos";
        dgvEstablecimientos.ReadOnly = true;
        dgvEstablecimientos.RowHeadersVisible = false;
        dgvEstablecimientos.RowHeadersWidth = 51;
        dgvEstablecimientos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvEstablecimientos.Size = new Size(1000, 316);
        dgvEstablecimientos.TabIndex = 1;
        dgvEstablecimientos.CellDoubleClick += dgvEstablecimientos_CellDoubleClick;
        // 
        // colCodigo
        // 
        colCodigo.DataPropertyName = "Codigo";
        colCodigo.HeaderText = "Código";
        colCodigo.MinimumWidth = 6;
        colCodigo.Name = "colCodigo";
        colCodigo.Width = 100;
        // 
        // colNombre
        // 
        colNombre.DataPropertyName = "Nombre";
        colNombre.HeaderText = "Nombre";
        colNombre.MinimumWidth = 6;
        colNombre.Name = "colNombre";
        colNombre.Width = 230;
        // 
        // colTipo
        // 
        colTipo.DataPropertyName = "TipoEstablecimiento";
        colTipo.HeaderText = "Tipo";
        colTipo.MinimumWidth = 6;
        colTipo.Name = "colTipo";
        colTipo.Width = 160;
        // 
        // colProvincia
        // 
        colProvincia.DataPropertyName = "NombreProvincia";
        colProvincia.HeaderText = "Provincia";
        colProvincia.MinimumWidth = 6;
        colProvincia.Name = "colProvincia";
        colProvincia.Width = 170;
        // 
        // colDireccion
        // 
        colDireccion.DataPropertyName = "Direccion";
        colDireccion.HeaderText = "Dirección";
        colDireccion.MinimumWidth = 6;
        colDireccion.Name = "colDireccion";
        colDireccion.Width = 230;
        // 
        // colTelefono
        // 
        colTelefono.DataPropertyName = "Telefono";
        colTelefono.HeaderText = "Teléfono";
        colTelefono.MinimumWidth = 6;
        colTelefono.Name = "colTelefono";
        colTelefono.Width = 120;
        // 
        // colEstado
        // 
        colEstado.DataPropertyName = "Estado";
        colEstado.HeaderText = "Estado";
        colEstado.MinimumWidth = 6;
        colEstado.Name = "colEstado";
        colEstado.Width = 90;
        // 
        // pnlAcciones
        // 
        pnlAcciones.Controls.Add(btnNuevo);
        pnlAcciones.Controls.Add(btnEditar);
        pnlAcciones.Controls.Add(btnDesactivar);
        pnlAcciones.Controls.Add(btnAnterior);
        pnlAcciones.Controls.Add(btnSiguiente);
        pnlAcciones.Controls.Add(lblResumen);
        pnlAcciones.Controls.Add(lblModo);
        pnlAcciones.Dock = DockStyle.Bottom;
        pnlAcciones.Location = new Point(0, 408);
        pnlAcciones.Name = "pnlAcciones";
        pnlAcciones.Padding = new Padding(10);
        pnlAcciones.Size = new Size(1000, 62);
        pnlAcciones.TabIndex = 2;
        // 
        // btnNuevo
        // 
        btnNuevo.Location = new Point(13, 14);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(100, 32);
        btnNuevo.TabIndex = 0;
        btnNuevo.Text = "Nuevo";
        btnNuevo.UseVisualStyleBackColor = true;
        btnNuevo.Click += btnNuevo_Click;
        // 
        // btnEditar
        // 
        btnEditar.Location = new Point(119, 14);
        btnEditar.Name = "btnEditar";
        btnEditar.Size = new Size(100, 32);
        btnEditar.TabIndex = 1;
        btnEditar.Text = "Editar";
        btnEditar.UseVisualStyleBackColor = true;
        btnEditar.Click += btnEditar_Click;
        // 
        // btnDesactivar
        // 
        btnDesactivar.Location = new Point(225, 14);
        btnDesactivar.Name = "btnDesactivar";
        btnDesactivar.Size = new Size(110, 32);
        btnDesactivar.TabIndex = 2;
        btnDesactivar.Text = "Desactivar";
        btnDesactivar.UseVisualStyleBackColor = true;
        btnDesactivar.Click += btnDesactivar_Click;
        // 
        // btnAnterior
        // 
        btnAnterior.Enabled = false;
        btnAnterior.Location = new Point(720, 14);
        btnAnterior.Name = "btnAnterior";
        btnAnterior.Size = new Size(100, 32);
        btnAnterior.TabIndex = 3;
        btnAnterior.Text = "« Anterior";
        btnAnterior.UseVisualStyleBackColor = true;
        btnAnterior.Click += btnAnterior_Click;
        // 
        // btnSiguiente
        // 
        btnSiguiente.Enabled = false;
        btnSiguiente.Location = new Point(826, 14);
        btnSiguiente.Name = "btnSiguiente";
        btnSiguiente.Size = new Size(100, 32);
        btnSiguiente.TabIndex = 4;
        btnSiguiente.Text = "Siguiente »";
        btnSiguiente.UseVisualStyleBackColor = true;
        btnSiguiente.Click += btnSiguiente_Click;
        // 
        // lblResumen
        // 
        lblResumen.Location = new Point(344, 19);
        lblResumen.Name = "lblResumen";
        lblResumen.Size = new Size(370, 20);
        lblResumen.TabIndex = 5;
        lblResumen.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModo
        // 
        lblModo.AutoSize = true;
        lblModo.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point);
        lblModo.Location = new Point(13, 52);
        lblModo.Name = "lblModo";
        lblModo.Size = new Size(0, 20);
        lblModo.TabIndex = 6;
        // 
        // FrmEstablecimientos
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 470);
        Controls.Add(dgvEstablecimientos);
        Controls.Add(pnlAcciones);
        Controls.Add(pnlFiltros);
        MinimumSize = new Size(800, 450);
        Name = "FrmEstablecimientos";
        Text = "Establecimientos";
        Load += FrmEstablecimientos_Load;
        pnlFiltros.ResumeLayout(false);
        pnlFiltros.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvEstablecimientos).EndInit();
        pnlAcciones.ResumeLayout(false);
        pnlAcciones.PerformLayout();
        ResumeLayout(false);
    }

    #endregion
}
