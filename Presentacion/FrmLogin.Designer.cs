namespace Presentacion;

partial class FrmLogin
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private Label lblTitulo;
    private Label lblSubtitulo;
    private Label lblUsuario;
    private Label lblContraseña;
    private TextBox txtUsuario;
    private TextBox txtContraseña;
    private Label lblError;
    private Button btnIngresar;
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
        lblTitulo = new Label();
        lblSubtitulo = new Label();
        lblUsuario = new Label();
        lblContraseña = new Label();
        txtUsuario = new TextBox();
        txtContraseña = new TextBox();
        lblError = new Label();
        btnIngresar = new Button();
        btnCancelar = new Button();
        errorProvider = new ErrorProvider(components);
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
        lblTitulo.Location = new Point(30, 20);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(206, 25);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Salud Nacional";
        // 
        // lblSubtitulo
        // 
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Location = new Point(32, 52);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.Size = new Size(260, 20);
        lblSubtitulo.TabIndex = 1;
        lblSubtitulo.Text = "Ingrese sus credenciales para continuar";
        // 
        // lblUsuario
        // 
        lblUsuario.AutoSize = true;
        lblUsuario.Location = new Point(32, 95);
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Size = new Size(55, 20);
        lblUsuario.TabIndex = 2;
        lblUsuario.Text = "Usuario:";
        // 
        // txtUsuario
        // 
        txtUsuario.Location = new Point(32, 118);
        txtUsuario.Name = "txtUsuario";
        txtUsuario.Size = new Size(300, 27);
        txtUsuario.TabIndex = 3;
        // 
        // lblContraseña
        // 
        lblContraseña.AutoSize = true;
        lblContraseña.Location = new Point(32, 158);
        lblContraseña.Name = "lblContraseña";
        lblContraseña.Size = new Size(88, 20);
        lblContraseña.TabIndex = 4;
        lblContraseña.Text = "Contraseña:";
        // 
        // txtContraseña
        // 
        txtContraseña.Location = new Point(32, 181);
        txtContraseña.Name = "txtContraseña";
        txtContraseña.Size = new Size(300, 27);
        txtContraseña.TabIndex = 5;
        txtContraseña.UseSystemPasswordChar = true;
        // 
        // lblError
        // 
        lblError.ForeColor = Color.Firebrick;
        lblError.Location = new Point(32, 214);
        lblError.Name = "lblError";
        lblError.Size = new Size(300, 20);
        lblError.TabIndex = 6;
        // 
        // btnIngresar
        // 
        btnIngresar.Location = new Point(132, 240);
        btnIngresar.Name = "btnIngresar";
        btnIngresar.Size = new Size(100, 32);
        btnIngresar.TabIndex = 7;
        btnIngresar.Text = "Ingresar";
        btnIngresar.UseVisualStyleBackColor = true;
        btnIngresar.Click += btnIngresar_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.DialogResult = DialogResult.Cancel;
        btnCancelar.Location = new Point(238, 240);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(94, 32);
        btnCancelar.TabIndex = 8;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = true;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        // 
        // FrmLogin
        // 
        AcceptButton = btnIngresar;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancelar;
        ClientSize = new Size(364, 290);
        Controls.Add(btnCancelar);
        Controls.Add(btnIngresar);
        Controls.Add(lblError);
        Controls.Add(txtContraseña);
        Controls.Add(lblContraseña);
        Controls.Add(txtUsuario);
        Controls.Add(lblUsuario);
        Controls.Add(lblSubtitulo);
        Controls.Add(lblTitulo);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmLogin";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Iniciar sesión";
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
