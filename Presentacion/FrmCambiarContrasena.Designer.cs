namespace Presentacion;

partial class FrmCambiarContraseña
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private Label lblActual;
    private TextBox txtActual;
    private Label lblNueva;
    private TextBox txtNueva;
    private Label lblConfirmacion;
    private TextBox txtConfirmacion;
    private Label lblAyuda;
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
        lblActual = new Label();
        txtActual = new TextBox();
        lblNueva = new Label();
        txtNueva = new TextBox();
        lblConfirmacion = new Label();
        txtConfirmacion = new TextBox();
        lblAyuda = new Label();
        btnGuardar = new Button();
        btnCancelar = new Button();
        errorProvider = new ErrorProvider(components);
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblActual
        // 
        lblActual.AutoSize = true;
        lblActual.Location = new Point(24, 24);
        lblActual.Name = "lblActual";
        lblActual.Size = new Size(140, 20);
        lblActual.TabIndex = 0;
        lblActual.Text = "Contraseña actual:";
        // 
        // txtActual
        // 
        txtActual.Location = new Point(190, 21);
        txtActual.Name = "txtActual";
        txtActual.Size = new Size(220, 27);
        txtActual.TabIndex = 1;
        txtActual.UseSystemPasswordChar = true;
        // 
        // lblNueva
        // 
        lblNueva.AutoSize = true;
        lblNueva.Location = new Point(24, 64);
        lblNueva.Name = "lblNueva";
        lblNueva.Size = new Size(140, 20);
        lblNueva.TabIndex = 2;
        lblNueva.Text = "Contraseña nueva:";
        // 
        // txtNueva
        // 
        txtNueva.Location = new Point(190, 61);
        txtNueva.MaxLength = 100;
        txtNueva.Name = "txtNueva";
        txtNueva.Size = new Size(220, 27);
        txtNueva.TabIndex = 3;
        txtNueva.UseSystemPasswordChar = true;
        // 
        // lblConfirmacion
        // 
        lblConfirmacion.AutoSize = true;
        lblConfirmacion.Location = new Point(24, 104);
        lblConfirmacion.Name = "lblConfirmacion";
        lblConfirmacion.Size = new Size(160, 20);
        lblConfirmacion.TabIndex = 4;
        lblConfirmacion.Text = "Confirmar contraseña:";
        // 
        // txtConfirmacion
        // 
        txtConfirmacion.Location = new Point(190, 101);
        txtConfirmacion.MaxLength = 100;
        txtConfirmacion.Name = "txtConfirmacion";
        txtConfirmacion.Size = new Size(220, 27);
        txtConfirmacion.TabIndex = 5;
        txtConfirmacion.UseSystemPasswordChar = true;
        // 
        // lblAyuda
        // 
        lblAyuda.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic, GraphicsUnit.Point);
        lblAyuda.Location = new Point(190, 134);
        lblAyuda.Name = "lblAyuda";
        lblAyuda.Size = new Size(220, 36);
        lblAyuda.TabIndex = 6;
        lblAyuda.Text = "Mínimo 8 caracteres. Debe ser distinta de la actual.";
        // 
        // btnGuardar
        // 
        btnGuardar.Location = new Point(246, 190);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(94, 32);
        btnGuardar.TabIndex = 7;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = true;
        btnGuardar.Click += btnGuardar_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.DialogResult = DialogResult.Cancel;
        btnCancelar.Location = new Point(346, 190);
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
        // FrmCambiarContraseña
        // 
        AcceptButton = btnGuardar;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancelar;
        ClientSize = new Size(460, 240);
        Controls.Add(btnCancelar);
        Controls.Add(btnGuardar);
        Controls.Add(lblAyuda);
        Controls.Add(txtConfirmacion);
        Controls.Add(lblConfirmacion);
        Controls.Add(txtNueva);
        Controls.Add(lblNueva);
        Controls.Add(txtActual);
        Controls.Add(lblActual);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmCambiarContraseña";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Cambiar mi contraseña";
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
