namespace Presentacion;

partial class FrmUsuarios
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private Panel pnlFiltros;
    private Label lblTexto;
    private TextBox txtTexto;
    private CheckBox chkIncluirInactivos;
    private Button btnBuscar;
    private DataGridView dgvUsuarios;
    private DataGridViewTextBoxColumn colNombreUsuario;
    private DataGridViewTextBoxColumn colNombreCompleto;
    private DataGridViewTextBoxColumn colRol;
    private DataGridViewTextBoxColumn colProvincia;
    private DataGridViewTextBoxColumn colEstado;
    private Panel pnlAcciones;
    private Button btnNuevo;
    private Button btnEditar;
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
        chkIncluirInactivos = new CheckBox();
        btnBuscar = new Button();
        dgvUsuarios = new DataGridView();
        colNombreUsuario = new DataGridViewTextBoxColumn();
        colNombreCompleto = new DataGridViewTextBoxColumn();
        colRol = new DataGridViewTextBoxColumn();
        colProvincia = new DataGridViewTextBoxColumn();
        colEstado = new DataGridViewTextBoxColumn();
        pnlAcciones = new Panel();
        btnNuevo = new Button();
        btnEditar = new Button();
        btnAnterior = new Button();
        btnSiguiente = new Button();
        lblResumen = new Label();
        lblModo = new Label();
        pnlFiltros.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
        pnlAcciones.SuspendLayout();
        SuspendLayout();
        // 
        // pnlFiltros
        // 
        pnlFiltros.Controls.Add(lblTexto);
        pnlFiltros.Controls.Add(txtTexto);
        pnlFiltros.Controls.Add(chkIncluirInactivos);
        pnlFiltros.Controls.Add(btnBuscar);
        pnlFiltros.Dock = DockStyle.Top;
        pnlFiltros.Location = new Point(0, 0);
        pnlFiltros.Name = "pnlFiltros";
        pnlFiltros.Padding = new Padding(10);
        pnlFiltros.Size = new Size(900, 70);
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
        txtTexto.PlaceholderText = "Nombre de usuario o nombre completo";
        txtTexto.Size = new Size(340, 27);
        txtTexto.TabIndex = 1;
        // 
        // chkIncluirInactivos
        // 
        chkIncluirInactivos.AutoSize = true;
        chkIncluirInactivos.Location = new Point(370, 38);
        chkIncluirInactivos.Name = "chkIncluirInactivos";
        chkIncluirInactivos.Size = new Size(140, 24);
        chkIncluirInactivos.TabIndex = 2;
        chkIncluirInactivos.Text = "Incluir inactivos";
        chkIncluirInactivos.UseVisualStyleBackColor = true;
        // 
        // btnBuscar
        // 
        btnBuscar.Location = new Point(530, 34);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(90, 32);
        btnBuscar.TabIndex = 3;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;
        // 
        // dgvUsuarios
        // 
        dgvUsuarios.AllowUserToAddRows = false;
        dgvUsuarios.AllowUserToDeleteRows = false;
        dgvUsuarios.AutoGenerateColumns = false;
        dgvUsuarios.BackgroundColor = SystemColors.Window;
        dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvUsuarios.Columns.AddRange(new DataGridViewColumn[] { colNombreUsuario, colNombreCompleto, colRol, colProvincia, colEstado });
        dgvUsuarios.Dock = DockStyle.Fill;
        dgvUsuarios.Location = new Point(0, 70);
        dgvUsuarios.MultiSelect = false;
        dgvUsuarios.Name = "dgvUsuarios";
        dgvUsuarios.ReadOnly = true;
        dgvUsuarios.RowHeadersVisible = false;
        dgvUsuarios.RowHeadersWidth = 51;
        dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUsuarios.Size = new Size(900, 330);
        dgvUsuarios.TabIndex = 1;
        dgvUsuarios.CellDoubleClick += dgvUsuarios_CellDoubleClick;
        // 
        // colNombreUsuario
        // 
        colNombreUsuario.DataPropertyName = "NombreUsuario";
        colNombreUsuario.HeaderText = "Usuario";
        colNombreUsuario.MinimumWidth = 6;
        colNombreUsuario.Name = "colNombreUsuario";
        colNombreUsuario.Width = 160;
        // 
        // colNombreCompleto
        // 
        colNombreCompleto.DataPropertyName = "NombreCompleto";
        colNombreCompleto.HeaderText = "Nombre completo";
        colNombreCompleto.MinimumWidth = 6;
        colNombreCompleto.Name = "colNombreCompleto";
        colNombreCompleto.Width = 260;
        // 
        // colRol
        // 
        colRol.DataPropertyName = "Rol";
        colRol.HeaderText = "Rol";
        colRol.MinimumWidth = 6;
        colRol.Name = "colRol";
        colRol.Width = 180;
        // 
        // colProvincia
        // 
        colProvincia.DataPropertyName = "NombreProvincia";
        colProvincia.HeaderText = "Provincia";
        colProvincia.MinimumWidth = 6;
        colProvincia.Name = "colProvincia";
        colProvincia.Width = 170;
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
        pnlAcciones.Controls.Add(btnAnterior);
        pnlAcciones.Controls.Add(btnSiguiente);
        pnlAcciones.Controls.Add(lblResumen);
        pnlAcciones.Controls.Add(lblModo);
        pnlAcciones.Dock = DockStyle.Bottom;
        pnlAcciones.Location = new Point(0, 400);
        pnlAcciones.Name = "pnlAcciones";
        pnlAcciones.Padding = new Padding(10);
        pnlAcciones.Size = new Size(900, 62);
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
        // btnAnterior
        // 
        btnAnterior.Enabled = false;
        btnAnterior.Location = new Point(520, 14);
        btnAnterior.Name = "btnAnterior";
        btnAnterior.Size = new Size(100, 32);
        btnAnterior.TabIndex = 2;
        btnAnterior.Text = "« Anterior";
        btnAnterior.UseVisualStyleBackColor = true;
        btnAnterior.Click += btnAnterior_Click;
        // 
        // btnSiguiente
        // 
        btnSiguiente.Enabled = false;
        btnSiguiente.Location = new Point(626, 14);
        btnSiguiente.Name = "btnSiguiente";
        btnSiguiente.Size = new Size(100, 32);
        btnSiguiente.TabIndex = 3;
        btnSiguiente.Text = "Siguiente »";
        btnSiguiente.UseVisualStyleBackColor = true;
        btnSiguiente.Click += btnSiguiente_Click;
        // 
        // lblResumen
        // 
        lblResumen.Location = new Point(440, 19);
        lblResumen.Name = "lblResumen";
        lblResumen.Size = new Size(370, 20);
        lblResumen.TabIndex = 4;
        lblResumen.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblModo
        // 
        lblModo.AutoSize = true;
        lblModo.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point);
        lblModo.Location = new Point(13, 52);
        lblModo.Name = "lblModo";
        lblModo.Size = new Size(0, 20);
        lblModo.TabIndex = 5;
        // 
        // FrmUsuarios
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 462);
        Controls.Add(dgvUsuarios);
        Controls.Add(pnlAcciones);
        Controls.Add(pnlFiltros);
        MinimumSize = new Size(700, 400);
        Name = "FrmUsuarios";
        Text = "Usuarios";
        Load += FrmUsuarios_Load;
        pnlFiltros.ResumeLayout(false);
        pnlFiltros.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
        pnlAcciones.ResumeLayout(false);
        pnlAcciones.PerformLayout();
        ResumeLayout(false);
    }

    #endregion
}
