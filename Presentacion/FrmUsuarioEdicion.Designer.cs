namespace Presentacion;

partial class FrmUsuarioEdicion
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private Label lblProvincia;
    private Label lblProvinciaValor;
    private ComboBox cboProvincia;
    private Label lblRol;
    private Label lblRolValor;
    private Label lblNombreCompleto;
    private TextBox txtNombreCompleto;
    private Label lblNombreUsuario;
    private TextBox txtNombreUsuario;
    private Label lblContraseña;
    private TextBox txtContraseña;
    private Label lblAyuda;
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
        lblProvincia = new Label();
        lblProvinciaValor = new Label();
        cboProvincia = new ComboBox();
        lblRol = new Label();
        lblRolValor = new Label();
        lblNombreCompleto = new Label();
        txtNombreCompleto = new TextBox();
        lblNombreUsuario = new Label();
        txtNombreUsuario = new TextBox();
        lblContraseña = new Label();
        txtContraseña = new TextBox();
        lblAyuda = new Label();
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
        lblProvinciaValor.Location = new Point(170, 20);
        lblProvinciaValor.Name = "lblProvinciaValor";
        lblProvinciaValor.Size = new Size(0, 20);
        lblProvinciaValor.TabIndex = 1;
        // 
        // cboProvincia
        // 
        cboProvincia.DropDownStyle = ComboBoxStyle.DropDownList;
        cboProvincia.Location = new Point(170, 16);
        cboProvincia.Name = "cboProvincia";
        cboProvincia.Size = new Size(280, 28);
        cboProvincia.TabIndex = 2;
        cboProvincia.Visible = false;
        // 
        // lblRol
        // 
        lblRol.AutoSize = true;
        lblRol.Location = new Point(24, 52);
        lblRol.Name = "lblRol";
        lblRol.Size = new Size(36, 20);
        lblRol.TabIndex = 3;
        lblRol.Text = "Rol:";
        // 
        // lblRolValor
        // 
        lblRolValor.AutoSize = true;
        lblRolValor.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        lblRolValor.Location = new Point(170, 52);
        lblRolValor.Name = "lblRolValor";
        lblRolValor.Size = new Size(0, 20);
        lblRolValor.TabIndex = 4;
        // 
        // lblNombreCompleto
        // 
        lblNombreCompleto.AutoSize = true;
        lblNombreCompleto.Location = new Point(24, 84);
        lblNombreCompleto.Name = "lblNombreCompleto";
        lblNombreCompleto.Size = new Size(136, 20);
        lblNombreCompleto.TabIndex = 5;
        lblNombreCompleto.Text = "Nombre completo:";
        // 
        // txtNombreCompleto
        // 
        txtNombreCompleto.Location = new Point(170, 81);
        txtNombreCompleto.MaxLength = 150;
        txtNombreCompleto.Name = "txtNombreCompleto";
        txtNombreCompleto.Size = new Size(280, 27);
        txtNombreCompleto.TabIndex = 6;
        // 
        // lblNombreUsuario
        // 
        lblNombreUsuario.AutoSize = true;
        lblNombreUsuario.Location = new Point(24, 116);
        lblNombreUsuario.Name = "lblNombreUsuario";
        lblNombreUsuario.Size = new Size(131, 20);
        lblNombreUsuario.TabIndex = 7;
        lblNombreUsuario.Text = "Nombre de usuario:";
        // 
        // txtNombreUsuario
        // 
        txtNombreUsuario.Location = new Point(170, 113);
        txtNombreUsuario.MaxLength = 50;
        txtNombreUsuario.Name = "txtNombreUsuario";
        txtNombreUsuario.Size = new Size(280, 27);
        txtNombreUsuario.TabIndex = 8;
        // 
        // lblContraseña
        // 
        lblContraseña.AutoSize = true;
        lblContraseña.Location = new Point(24, 148);
        lblContraseña.Name = "lblContraseña";
        lblContraseña.Size = new Size(88, 20);
        lblContraseña.TabIndex = 9;
        lblContraseña.Text = "Contraseña:";
        // 
        // txtContraseña
        // 
        txtContraseña.Location = new Point(170, 145);
        txtContraseña.MaxLength = 100;
        txtContraseña.Name = "txtContraseña";
        txtContraseña.Size = new Size(280, 27);
        txtContraseña.TabIndex = 10;
        txtContraseña.UseSystemPasswordChar = true;
        // 
        // lblAyuda
        // 
        lblAyuda.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic, GraphicsUnit.Point);
        lblAyuda.Location = new Point(170, 175);
        lblAyuda.Name = "lblAyuda";
        lblAyuda.Size = new Size(280, 36);
        lblAyuda.TabIndex = 11;
        // 
        // chkActivo
        // 
        chkActivo.AutoSize = true;
        chkActivo.Location = new Point(170, 218);
        chkActivo.Name = "chkActivo";
        chkActivo.Size = new Size(81, 24);
        chkActivo.TabIndex = 12;
        chkActivo.Text = "Activo";
        chkActivo.UseVisualStyleBackColor = true;
        // 
        // btnGuardar
        // 
        btnGuardar.Location = new Point(262, 262);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(94, 32);
        btnGuardar.TabIndex = 13;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = true;
        btnGuardar.Click += btnGuardar_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.DialogResult = DialogResult.Cancel;
        btnCancelar.Location = new Point(362, 262);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(94, 32);
        btnCancelar.TabIndex = 14;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = true;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        // 
        // FrmUsuarioEdicion
        // 
        AcceptButton = btnGuardar;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancelar;
        ClientSize = new Size(474, 314);
        Controls.Add(btnCancelar);
        Controls.Add(btnGuardar);
        Controls.Add(chkActivo);
        Controls.Add(lblAyuda);
        Controls.Add(txtContraseña);
        Controls.Add(lblContraseña);
        Controls.Add(txtNombreUsuario);
        Controls.Add(lblNombreUsuario);
        Controls.Add(txtNombreCompleto);
        Controls.Add(lblNombreCompleto);
        Controls.Add(lblRolValor);
        Controls.Add(lblRol);
        Controls.Add(cboProvincia);
        Controls.Add(lblProvinciaValor);
        Controls.Add(lblProvincia);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmUsuarioEdicion";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Usuario";
        Load += FrmUsuarioEdicion_Load;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
