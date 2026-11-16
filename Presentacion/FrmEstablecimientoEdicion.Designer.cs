namespace Presentacion;

partial class FrmEstablecimientoEdicion
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private Label lblCodigo;
    private TextBox txtCodigo;
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblTipo;
    private ComboBox cboTipo;
    private Label lblDireccion;
    private TextBox txtDireccion;
    private Label lblTelefono;
    private TextBox txtTelefono;
    private Label lblCorreo;
    private TextBox txtCorreo;
    private Label lblProvincia;
    private Label lblProvinciaValor;
    private CheckBox chkActivo;
    private Button btnGuardar;
    private Button btnCancelar;
    private ErrorProvider errorProvider;

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
        components = new System.ComponentModel.Container();
        lblCodigo = new Label();
        txtCodigo = new TextBox();
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblTipo = new Label();
        cboTipo = new ComboBox();
        lblDireccion = new Label();
        txtDireccion = new TextBox();
        lblTelefono = new Label();
        txtTelefono = new TextBox();
        lblCorreo = new Label();
        txtCorreo = new TextBox();
        lblProvincia = new Label();
        lblProvinciaValor = new Label();
        chkActivo = new CheckBox();
        btnGuardar = new Button();
        btnCancelar = new Button();
        errorProvider = new ErrorProvider(components);
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblProvincia
        // 
        lblProvincia.AutoSize = true;
        lblProvincia.Location = new Point(24, 20);
        lblProvincia.Name = "lblProvincia";
        lblProvincia.Size = new Size(70, 20);
        lblProvincia.TabIndex = 0;
        lblProvincia.Text = "Provincia:";
        // 
        // lblProvinciaValor
        // 
        lblProvinciaValor.AutoSize = true;
        lblProvinciaValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        lblProvinciaValor.Location = new Point(130, 20);
        lblProvinciaValor.Name = "lblProvinciaValor";
        lblProvinciaValor.Size = new Size(0, 20);
        lblProvinciaValor.TabIndex = 1;
        // 
        // lblCodigo
        // 
        lblCodigo.AutoSize = true;
        lblCodigo.Location = new Point(24, 55);
        lblCodigo.Name = "lblCodigo";
        lblCodigo.Size = new Size(60, 20);
        lblCodigo.TabIndex = 2;
        lblCodigo.Text = "Código:";
        // 
        // txtCodigo
        // 
        txtCodigo.Location = new Point(130, 52);
        txtCodigo.MaxLength = 20;
        txtCodigo.Name = "txtCodigo";
        txtCodigo.Size = new Size(300, 27);
        txtCodigo.TabIndex = 3;
        // 
        // lblNombre
        // 
        lblNombre.AutoSize = true;
        lblNombre.Location = new Point(24, 92);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(64, 20);
        lblNombre.TabIndex = 4;
        lblNombre.Text = "Nombre:";
        // 
        // txtNombre
        // 
        txtNombre.Location = new Point(130, 89);
        txtNombre.MaxLength = 150;
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(300, 27);
        txtNombre.TabIndex = 5;
        // 
        // lblTipo
        // 
        lblTipo.AutoSize = true;
        lblTipo.Location = new Point(24, 129);
        lblTipo.Name = "lblTipo";
        lblTipo.Size = new Size(37, 20);
        lblTipo.TabIndex = 6;
        lblTipo.Text = "Tipo:";
        // 
        // cboTipo
        // 
        cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTipo.Location = new Point(130, 126);
        cboTipo.Name = "cboTipo";
        cboTipo.Size = new Size(300, 28);
        cboTipo.TabIndex = 7;
        // 
        // lblDireccion
        // 
        lblDireccion.AutoSize = true;
        lblDireccion.Location = new Point(24, 166);
        lblDireccion.Name = "lblDireccion";
        lblDireccion.Size = new Size(73, 20);
        lblDireccion.TabIndex = 8;
        lblDireccion.Text = "Dirección:";
        // 
        // txtDireccion
        // 
        txtDireccion.Location = new Point(130, 163);
        txtDireccion.MaxLength = 255;
        txtDireccion.Name = "txtDireccion";
        txtDireccion.Size = new Size(300, 27);
        txtDireccion.TabIndex = 9;
        // 
        // lblTelefono
        // 
        lblTelefono.AutoSize = true;
        lblTelefono.Location = new Point(24, 203);
        lblTelefono.Name = "lblTelefono";
        lblTelefono.Size = new Size(68, 20);
        lblTelefono.TabIndex = 10;
        lblTelefono.Text = "Teléfono:";
        // 
        // txtTelefono
        // 
        txtTelefono.Location = new Point(130, 200);
        txtTelefono.MaxLength = 30;
        txtTelefono.Name = "txtTelefono";
        txtTelefono.Size = new Size(200, 27);
        txtTelefono.TabIndex = 11;
        // 
        // lblCorreo
        // 
        lblCorreo.AutoSize = true;
        lblCorreo.Location = new Point(24, 240);
        lblCorreo.Name = "lblCorreo";
        lblCorreo.Size = new Size(56, 20);
        lblCorreo.TabIndex = 12;
        lblCorreo.Text = "Correo:";
        // 
        // txtCorreo
        // 
        txtCorreo.Location = new Point(130, 237);
        txtCorreo.MaxLength = 150;
        txtCorreo.Name = "txtCorreo";
        txtCorreo.Size = new Size(300, 27);
        txtCorreo.TabIndex = 13;
        // 
        // chkActivo
        // 
        chkActivo.AutoSize = true;
        chkActivo.Location = new Point(130, 276);
        chkActivo.Name = "chkActivo";
        chkActivo.Size = new Size(81, 24);
        chkActivo.TabIndex = 14;
        chkActivo.Text = "Activo";
        chkActivo.UseVisualStyleBackColor = true;
        // 
        // btnGuardar
        // 
        btnGuardar.Location = new Point(246, 318);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(94, 32);
        btnGuardar.TabIndex = 15;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = true;
        btnGuardar.Click += btnGuardar_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.DialogResult = DialogResult.Cancel;
        btnCancelar.Location = new Point(346, 318);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(94, 32);
        btnCancelar.TabIndex = 16;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = true;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        // 
        // FrmEstablecimientoEdicion
        // 
        AcceptButton = btnGuardar;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancelar;
        ClientSize = new Size(460, 370);
        Controls.Add(btnCancelar);
        Controls.Add(btnGuardar);
        Controls.Add(chkActivo);
        Controls.Add(txtCorreo);
        Controls.Add(lblCorreo);
        Controls.Add(txtTelefono);
        Controls.Add(lblTelefono);
        Controls.Add(txtDireccion);
        Controls.Add(lblDireccion);
        Controls.Add(cboTipo);
        Controls.Add(lblTipo);
        Controls.Add(txtNombre);
        Controls.Add(lblNombre);
        Controls.Add(txtCodigo);
        Controls.Add(lblCodigo);
        Controls.Add(lblProvinciaValor);
        Controls.Add(lblProvincia);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmEstablecimientoEdicion";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Establecimiento";
        Load += FrmEstablecimientoEdicion_Load;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
