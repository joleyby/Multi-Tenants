namespace Presentacion;

partial class FrmProvinciaEdicion
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblCodigo;
    private TextBox txtCodigo;
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
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblCodigo = new Label();
        txtCodigo = new TextBox();
        lblAyuda = new Label();
        chkActivo = new CheckBox();
        btnGuardar = new Button();
        btnCancelar = new Button();
        errorProvider = new ErrorProvider(components);
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblNombre
        // 
        lblNombre.AutoSize = true;
        lblNombre.Location = new Point(24, 24);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(64, 20);
        lblNombre.TabIndex = 0;
        lblNombre.Text = "Nombre:";
        // 
        // txtNombre
        // 
        txtNombre.Location = new Point(130, 21);
        txtNombre.MaxLength = 100;
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(300, 27);
        txtNombre.TabIndex = 1;
        // 
        // lblCodigo
        // 
        lblCodigo.AutoSize = true;
        lblCodigo.Location = new Point(24, 64);
        lblCodigo.Name = "lblCodigo";
        lblCodigo.Size = new Size(60, 20);
        lblCodigo.TabIndex = 2;
        lblCodigo.Text = "Código:";
        // 
        // txtCodigo
        // 
        txtCodigo.CharacterCasing = CharacterCasing.Upper;
        txtCodigo.Location = new Point(130, 61);
        txtCodigo.MaxLength = 10;
        txtCodigo.Name = "txtCodigo";
        txtCodigo.Size = new Size(160, 27);
        txtCodigo.TabIndex = 3;
        // 
        // lblAyuda
        // 
        lblAyuda.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic, GraphicsUnit.Point);
        lblAyuda.Location = new Point(130, 94);
        lblAyuda.Name = "lblAyuda";
        lblAyuda.Size = new Size(300, 36);
        lblAyuda.TabIndex = 4;
        lblAyuda.Text = "El código tiene de 2 a 10 letras o números y es único.";
        // 
        // chkActivo
        // 
        chkActivo.AutoSize = true;
        chkActivo.Location = new Point(130, 140);
        chkActivo.Name = "chkActivo";
        chkActivo.Size = new Size(81, 24);
        chkActivo.TabIndex = 5;
        chkActivo.Text = "Activa";
        chkActivo.UseVisualStyleBackColor = true;
        // 
        // btnGuardar
        // 
        btnGuardar.Location = new Point(236, 186);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(94, 32);
        btnGuardar.TabIndex = 6;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = true;
        btnGuardar.Click += btnGuardar_Click;
        // 
        // btnCancelar
        // 
        btnCancelar.DialogResult = DialogResult.Cancel;
        btnCancelar.Location = new Point(336, 186);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(94, 32);
        btnCancelar.TabIndex = 7;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = true;
        btnCancelar.Click += btnCancelar_Click;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        // 
        // FrmProvinciaEdicion
        // 
        AcceptButton = btnGuardar;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancelar;
        ClientSize = new Size(450, 240);
        Controls.Add(btnCancelar);
        Controls.Add(btnGuardar);
        Controls.Add(chkActivo);
        Controls.Add(lblAyuda);
        Controls.Add(txtCodigo);
        Controls.Add(lblCodigo);
        Controls.Add(txtNombre);
        Controls.Add(lblNombre);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmProvinciaEdicion";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Provincia";
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
