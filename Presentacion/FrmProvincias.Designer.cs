namespace Presentacion;

partial class FrmProvincias
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private Panel pnlFiltros;
    private Label lblTexto;
    private TextBox txtTexto;
    private CheckBox chkIncluirInactivas;
    private Button btnBuscar;
    private DataGridView dgvProvincias;
    private DataGridViewTextBoxColumn colCodigo;
    private DataGridViewTextBoxColumn colNombre;
    private DataGridViewTextBoxColumn colEstado;
    private Panel pnlAcciones;
    private Button btnNuevo;
    private Button btnEditar;
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
        lblTexto = new Label();
        txtTexto = new TextBox();
        chkIncluirInactivas = new CheckBox();
        btnBuscar = new Button();
        dgvProvincias = new DataGridView();
        colCodigo = new DataGridViewTextBoxColumn();
        colNombre = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        pnlAcciones = new Panel();
        btnNuevo = new Button();
        btnEditar = new Button();
        lblResumen = new Label();
        pnlFiltros.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvProvincias).BeginInit();
        pnlAcciones.SuspendLayout();
        SuspendLayout();
        // 
        // pnlFiltros
        // 
        pnlFiltros.Controls.Add(lblTexto);
        pnlFiltros.Controls.Add(txtTexto);
        pnlFiltros.Controls.Add(chkIncluirInactivas);
        pnlFiltros.Controls.Add(btnBuscar);
        pnlFiltros.Dock = DockStyle.Top;
        pnlFiltros.Location = new Point(0, 0);
        pnlFiltros.Name = "pnlFiltros";
        pnlFiltros.Padding = new Padding(10);
        pnlFiltros.Size = new Size(700, 70);
        pnlFiltros.TabIndex = 0;
        // 
        // lblTexto
        // 
        lblTexto.AutoSize = true;
        lblTexto.Location = new Point(13, 13);
        lblTexto.Name = "lblTexto";
        lblTexto.Size = new Size(55, 20);
        lblTexto.TabIndex = 0;
        lblTexto.Text = "Buscar:";
        // 
        // txtTexto
        // 
        txtTexto.Location = new Point(13, 36);
        txtTexto.Name = "txtTexto";
        txtTexto.PlaceholderText = "Nombre o código";
        txtTexto.Size = new Size(300, 27);
        txtTexto.TabIndex = 1;
        // 
        // chkIncluirInactivas
        // 
        chkIncluirInactivas.AutoSize = true;
        chkIncluirInactivas.Location = new Point(330, 38);
        chkIncluirInactivas.Name = "chkIncluirInactivas";
        chkIncluirInactivas.Size = new Size(140, 24);
        chkIncluirInactivas.TabIndex = 2;
        chkIncluirInactivas.Text = "Incluir inactivas";
        chkIncluirInactivas.UseVisualStyleBackColor = true;
        // 
        // btnBuscar
        // 
        btnBuscar.Location = new Point(490, 34);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(90, 32);
        btnBuscar.TabIndex = 3;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;
        // 
        // dgvProvincias
        // 
        dgvProvincias.AllowUserToAddRows = false;
        dgvProvincias.AllowUserToDeleteRows = false;
        dgvProvincias.AutoGenerateColumns = false;
        dgvProvincias.BackgroundColor = SystemColors.Window;
        dgvProvincias.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvProvincias.Columns.AddRange(new DataGridViewColumn[] { colCodigo, colNombre, colEstado });
        dgvProvincias.Dock = DockStyle.Fill;
        dgvProvincias.Location = new Point(0, 70);
        dgvProvincias.MultiSelect = false;
        dgvProvincias.Name = "dgvProvincias";
        dgvProvincias.ReadOnly = true;
        dgvProvincias.RowHeadersVisible = false;
        dgvProvincias.RowHeadersWidth = 51;
        dgvProvincias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvProvincias.Size = new Size(700, 300);
        dgvProvincias.TabIndex = 1;
        dgvProvincias.CellDoubleClick += dgvProvincias_CellDoubleClick;
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
        colNombre.Width = 400;
        // 
        // colEstado
        // 
        colEstado.DataPropertyName = "Estado";
        colEstado.HeaderText = "Estado";
        colEstado.MinimumWidth = 6;
        colEstado.Name = "colEstado";
        colEstado.Width = 110;
        // 
        // pnlAcciones
        // 
        pnlAcciones.Controls.Add(btnNuevo);
        pnlAcciones.Controls.Add(btnEditar);
        pnlAcciones.Controls.Add(lblResumen);
        pnlAcciones.Dock = DockStyle.Bottom;
        pnlAcciones.Location = new Point(0, 370);
        pnlAcciones.Name = "pnlAcciones";
        pnlAcciones.Padding = new Padding(10);
        pnlAcciones.Size = new Size(700, 58);
        pnlAcciones.TabIndex = 2;
        // 
        // btnNuevo
        // 
        btnNuevo.Location = new Point(13, 13);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(100, 32);
        btnNuevo.TabIndex = 0;
        btnNuevo.Text = "Nuevo";
        btnNuevo.UseVisualStyleBackColor = true;
        btnNuevo.Click += btnNuevo_Click;
        // 
        // btnEditar
        // 
        btnEditar.Location = new Point(119, 13);
        btnEditar.Name = "btnEditar";
        btnEditar.Size = new Size(100, 32);
        btnEditar.TabIndex = 1;
        btnEditar.Text = "Editar";
        btnEditar.UseVisualStyleBackColor = true;
        btnEditar.Click += btnEditar_Click;
        // 
        // lblResumen
        // 
        lblResumen.Location = new Point(400, 18);
        lblResumen.Name = "lblResumen";
        lblResumen.Size = new Size(280, 20);
        lblResumen.TabIndex = 2;
        lblResumen.TextAlign = ContentAlignment.MiddleRight;
        // 
        // FrmProvincias
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(700, 428);
        Controls.Add(dgvProvincias);
        Controls.Add(pnlAcciones);
        Controls.Add(pnlFiltros);
        MinimumSize = new Size(560, 380);
        Name = "FrmProvincias";
        Text = "Provincias";
        Load += FrmProvincias_Load;
        pnlFiltros.ResumeLayout(false);
        pnlFiltros.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvProvincias).EndInit();
        pnlAcciones.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion
}
